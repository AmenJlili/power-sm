using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using SolidWorks.Interop.sldworks;
using SolidWorks.Interop.swconst;

namespace PowerSM
{
    public partial class PowerGeometryForm : Form
    {
        #region Constants

        private const string BendZoneKFactor = "KFactor";
        private const string BendZoneAllowance = "Bend Allowance";
        private const string BendZoneDeduction = "Bend Deduction";
        private const string ArchiveFileName = "powersm.zip";
        private const string HelpUrl = "https://github.com/AmenJlili/power-sm/wiki/PowerGeometry";
        private const string PartExtension = ".sldprt";

        // Feature type names (Feature.GetTypeName) that expose each sheet metal parameter.
        private static readonly string[] FeatureTypesWithRadius =
            { "SMBaseFlange", "EdgeFlange", "SketchBend", "SheetMetal", "OneBend", "SMMiteredFlange", "Jog", "ProcessBends" };

        private static readonly string[] FeatureTypesWithKFactor =
            { "EdgeFlange", "SheetMetal", "OneBend", "Hem", "SMMiteredFlange", "Jog", "Bends" };

        private static readonly string[] FeatureTypesWithThickness =
            { "SMBaseFlange", "LoftedBends", "SheetMetal", "OneBend" };

        // Feature type names (Feature.GetTypeName2) that can inherit radius / bend allowance from their parent.
        private static readonly string[] FeatureTypesInheritingBendParameters =
            { "EdgeFlange", "SM3dBend", "SketchBend", "OneBend", "SMMiteredFlange", "Jog", "Bends", "ProcessBends", "FlattenBends" };

        private static readonly string[] FeatureTypesWithBendRelief =
            { "EdgeFlange", "OneBend", "SketchBend" };

        #endregion

        private readonly SldWorks swApp;
        private string CurrentDirectory;
        private string OutputDirectory;
        private List<CustomTreeNode> AllTreeViewNodes = new List<CustomTreeNode>();
        private bool isRunning;

        private static string TempArchiveFolder
        {
            get { return Path.Combine(Path.GetTempPath(), "PowerSM"); }
        }

        /// <summary>Snapshot of everything a run needs, taken on the UI thread before work starts.</summary>
        private sealed class RunSettings
        {
            public string UnitSystem;
            public string BendZoneType;
            public double RadiusMeters;
            public double ThicknessMeters;
            /// <summary>K-factor (unitless) or bend allowance / deduction in meters.</summary>
            public double BendZoneValue;
            public bool Force;
            public bool SetOverrideToFalse;
            public bool ForceDimensionalRespect;
            public bool Archive;
            public string OutputDirectory;

            public bool HasRadius { get { return RadiusMeters > 0; } }
            public bool HasThickness { get { return ThicknessMeters > 0; } }
            public bool HasBendZone { get { return BendZoneValue > 0; } }
            public bool ArchiveToZip { get { return Archive && !string.IsNullOrEmpty(OutputDirectory); } }
        }

        private sealed class PartJob
        {
            public string FullFilePath;
            public string RelativePath;
        }

        public PowerGeometryForm(SldWorks swApp_)
        {
            InitializeComponent();
            swApp = swApp_;

            UnitSystemCombBox.SelectedIndexChanged += (s, e) => UpdateUnitLabels();
            BendZoneCheckedBox.SelectedIndexChanged += (s, e) => UpdateUnitLabels();
            FormClosing += PowerGeometryForm_FormClosing;
        }

        private void Power_SM_Form_Load(object sender, EventArgs e)
        {
            // Tree view icons (index order must match PowerSMEnums.TreeViewIcons)
            var rm = new System.Resources.ResourceManager("PowerSM.Properties.Resources", typeof(PowerGeometryForm).Assembly);
            FilesTreeView.ImageList = new ImageList { ColorDepth = ColorDepth.Depth32Bit };
            FilesTreeView.ImageList.Images.Add((Bitmap)rm.GetObject("part_png"));
            FilesTreeView.ImageList.Images.Add((Bitmap)rm.GetObject("folder_png"));

            UnitSystemCombBox.SelectedIndex = 0;
            BendZoneCheckedBox.SelectedIndex = 0;
            UpdateUnitLabels();
            UpdateOutputLabel();
            LoadRecentDirectory();

            toolTip.SetToolTip(BrowseForFolderButton, "Pick the folder that contains the sheet metal parts.");
            toolTip.SetToolTip(BrowseForOutPutFolder, "Optional. Save modified parts to another folder instead of overwriting them.\nRight-click to clear.");
            toolTip.SetToolTip(OpenFolder, "Open the source folder in Windows Explorer.");
            toolTip.SetToolTip(RadiusTextBox, "New bend radius. Leave empty to keep the current value.");
            toolTip.SetToolTip(ThicknessTextBox, "New thickness. Leave empty to keep the current value.");
            toolTip.SetToolTip(BendZoneTextBox, "New K-factor, bend allowance or bend deduction. Leave empty to keep the current value.");

            BrowseForOutPutFolder.MouseUp += BrowseForOutPutFolder_MouseUp;
        }

        #region UI methods

        private void UpdateUnitLabels()
        {
            var unit = MeasuringUnitsConvertor.Length.UnitLabel(SelectedUnitSystem);
            NewRadiusLabel.Text = string.Format("Radius ({0}):", unit);
            label1.Text = string.Format("Thickness ({0}):", unit);

            switch (SelectedBendZoneType)
            {
                case BendZoneAllowance:
                    BendZoneValue.Text = string.Format("Allowance ({0}):", unit);
                    break;
                case BendZoneDeduction:
                    BendZoneValue.Text = string.Format("Deduction ({0}):", unit);
                    break;
                default:
                    BendZoneValue.Text = "K-Factor:";
                    break;
            }
        }

        private void UpdateOutputLabel()
        {
            OutputStatusLabel.Text = string.IsNullOrEmpty(OutputDirectory)
                ? "Output: save in place"
                : "Output: " + OutputDirectory;
            OutputStatusLabel.ToolTipText = OutputStatusLabel.Text;
        }

        private void UpdateSelectionStatus()
        {
            if (isRunning)
                return;
            var count = AllTreeViewNodes.Count(n => n.Checked && n.IsPart);
            toolStripStatusLabel.Text = count == 0 ? "Ready" : string.Format("Ready - {0} part(s) selected", count);
        }

        private string SelectedUnitSystem
        {
            get { return Convert.ToString(UnitSystemCombBox.SelectedItem) == MeasuringUnitsConvertor.IPS ? MeasuringUnitsConvertor.IPS : MeasuringUnitsConvertor.MMGS; }
        }

        private string SelectedBendZoneType
        {
            get { return Convert.ToString(BendZoneCheckedBox.SelectedItem) ?? BendZoneKFactor; }
        }

        private void SetRunning(bool running)
        {
            isRunning = running;
            StartButton.Enabled = !running;
            StartButton.Text = running ? "Running..." : "Start";
            BrowseForFolderButton.Enabled = !running;
            BrowseForOutPutFolder.Enabled = !running;
            SheetMetalDataGroupBox.Enabled = !running;
            FilesTreeView.Enabled = !running;
            menuStrip1.Enabled = !running;
            UseWaitCursor = running;
        }

        private void AppendLog(IEnumerable<string> lines)
        {
            foreach (var line in lines)
                AppendLog(line);
        }

        private void AppendLog(string line)
        {
            // Normalize bare \n so the WinForms TextBox renders line breaks.
            var text = (line ?? string.Empty).Replace("\r\n", "\n").Replace("\n", System.Environment.NewLine);
            LogTextBox.AppendText(text + System.Environment.NewLine);
        }

        private void PowerGeometryForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (isRunning && e.CloseReason == CloseReason.UserClosing)
            {
                MessageBox.Show(this, "Please wait until the current run finishes.", "Power Geometry Tool",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                e.Cancel = true;
            }
        }

        #region Browse for files

        private void BrowseForFolderButton_Click(object sender, EventArgs e)
        {
            using (var folderBrowser = new FolderBrowserDialog())
            {
                folderBrowser.Description = "Select the folder that contains the sheet metal parts";
                if (!string.IsNullOrEmpty(CurrentDirectory) && Directory.Exists(CurrentDirectory))
                    folderBrowser.SelectedPath = CurrentDirectory;

                if (folderBrowser.ShowDialog(this) == DialogResult.OK && !string.IsNullOrEmpty(folderBrowser.SelectedPath))
                {
                    LoadDirectory(folderBrowser.SelectedPath);

                    PowerSMSettings.RecentDirectory = CurrentDirectory;
                    PowerSMSettings.Save();
                    LoadRecentDirectory();
                }
            }
        }

        private void BrowseForOutPutFolder_Click(object sender, EventArgs e)
        {
            using (var folderBrowser = new FolderBrowserDialog())
            {
                folderBrowser.Description = "Select the folder where modified parts will be saved";
                if (folderBrowser.ShowDialog(this) == DialogResult.OK && !string.IsNullOrEmpty(folderBrowser.SelectedPath))
                {
                    OutputDirectory = folderBrowser.SelectedPath;
                    UpdateOutputLabel();
                }
            }
        }

        private void BrowseForOutPutFolder_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right && !isRunning && !string.IsNullOrEmpty(OutputDirectory))
            {
                OutputDirectory = null;
                UpdateOutputLabel();
            }
        }

        private void LoadDirectory(string path)
        {
            CurrentDirectory = path;

            FilesTreeView.BeginUpdate();
            try
            {
                FilesTreeView.Nodes.Clear();
                var root = CreateDirectoryNode(new DirectoryInfo(path));
                FilesTreeView.Nodes.Add(root);
                root.Expand();
            }
            finally
            {
                FilesTreeView.EndUpdate();
            }

            AllTreeViewNodes = new List<CustomTreeNode>();
            GetAllNodesFromTreeView(FilesTreeView, AllTreeViewNodes);
            SetAllNodesChecked(true);
            FilesTreeGroupBox.Text = "Files Tree - " + path;
        }

        private static CustomTreeNode CreateDirectoryNode(DirectoryInfo directoryInfo)
        {
            var directoryNode = new CustomTreeNode(directoryInfo.Name, (int)PowerSMEnums.TreeViewIcons.directory,
                (int)PowerSMEnums.TreeViewIcons.directory, directoryInfo.FullName, false);

            try
            {
                foreach (var directory in directoryInfo.GetDirectories())
                {
                    // Do not add hidden directories
                    if (!directory.Attributes.HasFlag(FileAttributes.Hidden))
                        directoryNode.Nodes.Add(CreateDirectoryNode(directory));
                }

                foreach (var file in directoryInfo.GetFiles())
                {
                    // Parts only, any casing; skip SOLIDWORKS lock/temp files (~$part.sldprt)
                    if (string.Equals(file.Extension, PartExtension, StringComparison.OrdinalIgnoreCase)
                        && !file.Name.StartsWith("~$", StringComparison.Ordinal))
                    {
                        directoryNode.Nodes.Add(new CustomTreeNode(file.Name, (int)PowerSMEnums.TreeViewIcons.file,
                            (int)PowerSMEnums.TreeViewIcons.file, file.FullName, true));
                    }
                }
            }
            catch (UnauthorizedAccessException)
            {
                directoryNode.Text += " (access denied)";
            }
            catch (IOException)
            {
                directoryNode.Text += " (unreadable)";
            }

            return directoryNode;
        }

        #endregion

        private void OpenFolder_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(CurrentDirectory) && Directory.Exists(CurrentDirectory))
                System.Diagnostics.Process.Start("explorer.exe", "\"" + CurrentDirectory + "\"");
        }

        private void optionsToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            using (var options = new PowerGeometryOptions())
            {
                options.ShowDialog(this);
            }
        }

        #region UI: Check/uncheck children feature

        private void FilesTreeView_AfterCheck(object sender, TreeViewEventArgs e)
        {
            // Only react to user changes, not to the recursive changes made below.
            if (e.Action != TreeViewAction.Unknown)
            {
                if (e.Node.Nodes.Count > 0)
                    CheckAllChildNodes(e.Node, e.Node.Checked);
                UpdateSelectionStatus();
            }
        }

        private static void CheckAllChildNodes(TreeNode treeNode, bool nodeChecked)
        {
            foreach (TreeNode node in treeNode.Nodes)
            {
                node.Checked = nodeChecked;
                if (node.Nodes.Count > 0)
                    CheckAllChildNodes(node, nodeChecked);
            }
        }

        private void SetAllNodesChecked(bool isChecked)
        {
            foreach (var node in AllTreeViewNodes)
                node.Checked = isChecked;
            UpdateSelectionStatus();
        }

        #endregion

        private void SaveLog_Click(object sender, EventArgs e)
        {
            using (var saveFileDialog = new SaveFileDialog())
            {
                saveFileDialog.Filter = "Text files (*.txt)|*.txt|All files (*.*)|*.*";
                saveFileDialog.FileName = string.Format("PowerSM_{0:yyyyMMdd_HHmmss}.txt", DateTime.Now);
                if (saveFileDialog.ShowDialog(this) != DialogResult.OK)
                    return;
                try
                {
                    File.WriteAllText(saveFileDialog.FileName, LogTextBox.Text);
                }
                catch (Exception)
                {
                    ErrorEchoer.Err((int)PowerSMEnums.Errors.Cannot_Write_Log_File);
                }
            }
        }

        private void quitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void aboutThisToolToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (var aboutBox = new AboutBox())
            {
                aboutBox.ShowDialog(this);
            }
        }

        private void selectAllToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SetAllNodesChecked(true);
        }

        private void unSelectAllToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SetAllNodesChecked(false);
        }

        private void helpToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start(HelpUrl);
        }

        private void LoadRecentDirectory()
        {
            var recent = PowerSMSettings.RecentDirectory;
            var exists = !string.IsNullOrEmpty(recent) && Directory.Exists(recent);
            RecentToolStripMenu.Text = exists ? recent : "(none)";
            RecentToolStripMenu.Enabled = exists;
        }

        private void RecentToolStripMenu_Click(object sender, EventArgs e)
        {
            var recent = PowerSMSettings.RecentDirectory;
            if (!string.IsNullOrEmpty(recent) && Directory.Exists(recent))
                LoadDirectory(recent);
        }

        private void PowerGeometryForm_Activated(object sender, EventArgs e)
        {
            LoadRecentDirectory();
        }

        #endregion

        #region GetAllNodes methods

        private static void GetAllNodesFromTreeView(TreeView treeview, List<CustomTreeNode> allTreeViewNodes)
        {
            allTreeViewNodes.Clear();
            foreach (CustomTreeNode treenode in treeview.Nodes)
                GetChildrenNodesFromNode(treenode, allTreeViewNodes);
        }

        private static void GetChildrenNodesFromNode(CustomTreeNode treenode, List<CustomTreeNode> allTreeViewNodes)
        {
            allTreeViewNodes.Add(treenode);
            foreach (CustomTreeNode childtreenode in treenode.Nodes)
                GetChildrenNodesFromNode(childtreenode, allTreeViewNodes);
        }

        #endregion

        #region Run

        /// <summary>Parses a user value; empty means "do not change" (0). Accepts both the current culture and '.' decimals.</summary>
        private static bool TryParseInput(string text, out double value)
        {
            value = 0;
            if (string.IsNullOrWhiteSpace(text))
                return true;
            text = text.Trim();
            return (double.TryParse(text, NumberStyles.Float, CultureInfo.CurrentCulture, out value)
                    || double.TryParse(text, NumberStyles.Float, CultureInfo.InvariantCulture, out value))
                   && value >= 0 && !double.IsInfinity(value) && !double.IsNaN(value);
        }

        private async void StartButton_Click(object sender, EventArgs e)
        {
            if (isRunning)
                return;

            double radius, thickness, bendZoneValue;
            if (!TryParseInput(RadiusTextBox.Text, out radius))
            {
                ErrorEchoer.Err((int)PowerSMEnums.Errors.Cannot_parse_radius_value);
                RadiusTextBox.Focus();
                return;
            }
            if (!TryParseInput(ThicknessTextBox.Text, out thickness))
            {
                ErrorEchoer.Err((int)PowerSMEnums.Errors.cannot_parse_thickness);
                ThicknessTextBox.Focus();
                return;
            }
            // A K-factor must be between 0 and 1; bend allowance / deduction are lengths.
            if (!TryParseInput(BendZoneTextBox.Text, out bendZoneValue)
                || (SelectedBendZoneType == BendZoneKFactor && bendZoneValue > 1))
            {
                ErrorEchoer.Err((int)PowerSMEnums.Errors.cannot_parse_kfacor);
                BendZoneTextBox.Focus();
                return;
            }

            if (FilesTreeView.Nodes.Count == 0)
            {
                ErrorEchoer.Err((int)PowerSMEnums.Errors.Empty_Tree);
                return;
            }

            var jobs = AllTreeViewNodes
                .Where(n => n.Checked && n.IsPart && File.Exists(n.FullFilePath))
                .Select(n => new PartJob { FullFilePath = n.FullFilePath, RelativePath = n.FullPath })
                .ToList();

            if (jobs.Count == 0)
            {
                MessageBox.Show(this, "No part files are selected.", "Power Geometry Tool", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            var unitSystem = SelectedUnitSystem;
            var bendZoneType = SelectedBendZoneType;
            var settings = new RunSettings
            {
                UnitSystem = unitSystem,
                BendZoneType = bendZoneType,
                RadiusMeters = MeasuringUnitsConvertor.Length.ToMeters(radius, unitSystem),
                ThicknessMeters = MeasuringUnitsConvertor.Length.ToMeters(thickness, unitSystem),
                BendZoneValue = bendZoneType == BendZoneKFactor ? bendZoneValue : MeasuringUnitsConvertor.Length.ToMeters(bendZoneValue, unitSystem),
                Force = PowerSMSettings.ForceMode,
                SetOverrideToFalse = !PowerSMSettings.ForceMode && PowerSMSettings.SetOverrideToFalse,
                ForceDimensionalRespect = PowerSMSettings.ForceDimensionalRespect,
                Archive = PowerSMSettings.ArchiveInZipFormat,
                OutputDirectory = OutputDirectory
            };

            SetRunning(true);
            LogTextBox.Clear();
            _ProgressBar.Minimum = 0;
            _ProgressBar.Maximum = jobs.Count;
            _ProgressBar.Step = 1;
            _ProgressBar.Value = 0;

            try
            {
                if (settings.Archive && string.IsNullOrEmpty(settings.OutputDirectory))
                    AppendLog("Note: zip archiving is on but no output folder is selected. Files will be saved in place and not archived.");

                if (settings.ArchiveToZip)
                    await Task.Run(() => PrepareZip(settings.OutputDirectory));

                var processed = 0;
                foreach (var job in jobs)
                {
                    toolStripStatusLabel.Text = string.Format("Processing {0} of {1}: {2}", processed + 1, jobs.Count, Path.GetFileName(job.FullFilePath));
                    var currentJob = job;
                    var result = await Task.Run(() => ChangeSheetMetalFeatures(currentJob, settings));
                    AppendLog(result);
                    processed++;
                    _ProgressBar.PerformStep();
                }

                if (settings.ArchiveToZip)
                {
                    toolStripStatusLabel.Text = "Compressing...";
                    var zipPath = await Task.Run(() => CreateZip(settings.OutputDirectory));
                    AppendLog(string.Format("[{0}] Archive created: {1}", DateTime.Now, zipPath));
                }

                toolStripStatusLabel.Text = string.Format("Done - {0} part(s) processed", processed);
            }
            catch (Exception exception)
            {
                AppendLog(string.Format("[{0}] - ERROR: {1}", DateTime.Now, exception.Message));
                toolStripStatusLabel.Text = "Finished with errors";
            }
            finally
            {
                SetRunning(false);
            }
        }

        private static void PrepareZip(string outputDirectory)
        {
            var tempFolder = TempArchiveFolder;
            if (Directory.Exists(tempFolder))
                Directory.Delete(tempFolder, true);
            Directory.CreateDirectory(tempFolder);

            var zipPath = Path.Combine(outputDirectory, ArchiveFileName);
            if (File.Exists(zipPath))
                File.Delete(zipPath);
        }

        private static string CreateZip(string outputDirectory)
        {
            var zipPath = Path.Combine(outputDirectory, ArchiveFileName);
            if (File.Exists(zipPath))
                File.Delete(zipPath);
            ZipFile.CreateFromDirectory(TempArchiveFolder, zipPath);
            return zipPath;
        }

        #endregion

        #region ChangeSheetMetalFeatures

        // Runs on a worker thread: must not touch any control. Everything it needs is in RunSettings.
        private string[] ChangeSheetMetalFeatures(PartJob job, RunSettings settings)
        {
            var results = new List<string>();
            ModelDoc2 swModelDoc = null;
            var wasAlreadyOpen = false;

            results.Add(string.Format("[{0}] PART FILENAME: {1}", DateTime.Now, job.FullFilePath));

            try
            {
                int error = 0;
                int warning = 0;

                // Don't close a document the user already had open.
                wasAlreadyOpen = swApp.GetOpenDocumentByName(job.FullFilePath) != null;

                swApp.DocumentVisible(false, (int)swDocumentTypes_e.swDocPART);
                swModelDoc = swApp.OpenDoc6(job.FullFilePath, (int)swDocumentTypes_e.swDocPART,
                    (int)swOpenDocOptions_e.swOpenDocOptions_Silent, string.Empty, ref error, ref warning);

                // Any error or warning skips the file.
                if (error != 0)
                    throw new OpenFileException(error, false);
                if (warning != 0)
                    throw new OpenFileException(warning, true);
                if (swModelDoc == null)
                    throw new InvalidOperationException("SOLIDWORKS could not open the file.");

                if (settings.Force)
                {
                    // Force mode: write values on every feature, breaking inheritance.
                    if (settings.HasThickness)
                        results.AddRange(ForceChangeThickness(swModelDoc, settings));
                    if (settings.HasBendZone)
                        results.AddRange(ForceChangeBendZone(swModelDoc, settings));
                    if (settings.HasRadius)
                        results.AddRange(ForceChangeRadius(swModelDoc, settings));
                }
                else
                {
                    // Normal mode: change the sheet metal folder and let features inherit.
                    if (settings.SetOverrideToFalse)
                        results.AddRange(RemoveOverridingParameters(swModelDoc));
                    if (settings.HasThickness)
                        results.AddRange(ChangeThickness(swModelDoc, settings));
                    if (settings.HasBendZone)
                        results.AddRange(ChangeBendZone(swModelDoc, settings));
                    if (settings.HasRadius)
                        results.AddRange(ChangeRadius(swModelDoc, settings));
                }

                results.AddRange(SaveDocument(swModelDoc, job, settings));
            }
            catch (Exception exception)
            {
                results.Add(string.Format("[{0}] - ERROR: {1}", DateTime.Now, exception.Message));
            }
            finally
            {
                if (swModelDoc != null && !wasAlreadyOpen)
                    CloseDocument(swModelDoc);
                swApp.DocumentVisible(true, (int)swDocumentTypes_e.swDocPART);
            }

            results.Add(string.Empty);
            return results.ToArray();
        }

        private IEnumerable<string> SaveDocument(ModelDoc2 swModelDoc, PartJob job, RunSettings settings)
        {
            int error = 0;
            int warning = 0;

            if (!string.IsNullOrEmpty(settings.OutputDirectory))
            {
                var targetRoot = settings.ArchiveToZip ? TempArchiveFolder : settings.OutputDirectory;
                var newFileName = Path.Combine(targetRoot, job.RelativePath);
                Directory.CreateDirectory(Path.GetDirectoryName(newFileName));

                var saved = swModelDoc.Extension.SaveAs(newFileName, (int)swSaveAsVersion_e.swSaveAsCurrentVersion,
                    (int)swSaveAsOptions_e.swSaveAsOptions_Silent, null, ref error, ref warning);
                return new[]
                {
                    string.Format("\t * {0} {1} : {2} Error: {3} Warning: {4}",
                        settings.ArchiveToZip ? "Saving for archive" : "Saving to output folder",
                        newFileName, saved ? "SUCCESS" : "FAILURE", error, warning)
                };
            }

            var savedInPlace = swModelDoc.Save3((int)swSaveAsOptions_e.swSaveAsOptions_Silent, ref error, ref warning);
            return new[]
            {
                string.Format("\t * Saving file : {0} Error: {1} Warning: {2}", savedInPlace ? "SUCCESS" : "FAILURE", error, warning)
            };
        }

        private void CloseDocument(ModelDoc2 swModelDoc)
        {
            try
            {
                var name = swModelDoc.GetTitle();
                if (string.IsNullOrEmpty(name))
                    name = swModelDoc.GetPathName();
                swApp.CloseDoc(name);
            }
            catch (Exception)
            {
                // The document may already be gone; nothing else to do.
            }
        }

        #region Normal mode: changes go through the sheet metal folder

        private IEnumerable<string> RemoveOverridingParameters(ModelDoc2 swModelDoc)
        {
            var results = new List<string>();
            try
            {
                FeatureManager swFeatureManager = swModelDoc.FeatureManager;
                var swSheetMetalFolder = (SheetMetalFolder)swFeatureManager.GetSheetMetalFolder();

                // Sheet metal features inherit from the sheet metal folder
                var sheetMetals = swSheetMetalFolder == null ? null : swSheetMetalFolder.GetSheetMetals() as object[];
                if (sheetMetals != null)
                {
                    foreach (var featObj in sheetMetals)
                    {
                        var swSheetMetalFeature = (Feature)featObj;
                        var swSheetMetalFeatureData = (SheetMetalFeatureData)swSheetMetalFeature.GetDefinition();
                        swSheetMetalFeatureData.SetOverrideDefaultParameter(false);
                        var ok = swSheetMetalFeature.ModifyDefinition(swSheetMetalFeatureData, swModelDoc, null);
                        results.Add(string.Format("\t * Making sheet metal {0} inherit from the sheet metal folder : {1}", swSheetMetalFeature.Name, Status(ok)));
                    }
                }

                var features = swFeatureManager.GetFeatures(false) as object[];
                if (features == null)
                    return results;

                foreach (var featObj in features)
                {
                    var swFeature = (Feature)featObj;
                    var typeName = swFeature.GetTypeName2();

                    // Bend features: use default bend radius and bend allowance
                    if (FeatureTypesInheritingBendParameters.Contains(typeName))
                    {
                        dynamic data = swFeature.GetDefinition();
                        if (data != null)
                        {
                            data.UseDefaultBendRadius = true;
                            data.UseDefaultBendAllowance = true;
                            bool ok = swFeature.ModifyDefinition((object)data, swModelDoc, null);
                            results.Add(string.Format("\t * Making {0} inherit parameters from its parent : {1}", swFeature.Name, Status(ok)));
                        }
                    }

                    // Base flange driven by a gauge table: remove thickness / radius / k-factor overrides
                    if (typeName == "SMBaseFlange")
                    {
                        dynamic data = swFeature.GetDefinition();
                        if (data != null && data.UseGaugeTable == true)
                        {
                            data.OverrideThickness = false;
                            data.OverrideRadius = false;
                            data.OverrideKFactor = false;
                            bool ok = swFeature.ModifyDefinition((object)data, swModelDoc, null);
                            results.Add(string.Format("\t * Removing overrides of base flange {0} : {1}", swFeature.Name, Status(ok)));
                        }
                    }

                    if (typeName == "Hem")
                    {
                        dynamic data = swFeature.GetDefinition();
                        if (data != null)
                        {
                            data.UseDefaultBendAllowance = true;
                            bool ok = swFeature.ModifyDefinition((object)data, swModelDoc, null);
                            results.Add(string.Format("\t * Making hem {0} use the default bend allowance : {1}", swFeature.Name, Status(ok)));
                        }
                    }

                    if (FeatureTypesWithBendRelief.Contains(typeName))
                    {
                        dynamic data = swFeature.GetDefinition();
                        if (data != null)
                        {
                            data.UseDefaultBendRelief = true;
                            bool ok = swFeature.ModifyDefinition((object)data, swModelDoc, null);
                            results.Add(string.Format("\t * Making {0} use the default bend relief : {1}", swFeature.Name, Status(ok)));
                        }
                    }
                }
            }
            catch (Exception exception)
            {
                results.Add(FeatureError("removing overriding parameters", exception));
            }
            return results;
        }

        private IEnumerable<string> ChangeThickness(ModelDoc2 swModelDoc, RunSettings settings)
        {
            var results = new List<string>();
            try
            {
                var swSheetMetalFolder = (SheetMetalFolder)swModelDoc.FeatureManager.GetSheetMetalFolder();
                if (swSheetMetalFolder == null)
                    return new[] { NoSheetMetalFolder("thickness") };

                var ok = new SheetMetalFolder2(swSheetMetalFolder).Thickness(settings.ThicknessMeters);
                if (ok)
                    swModelDoc.ForceRebuild3(true);

                results.Add(string.Format("\t * NORMAL MODE: changing {0}'s thickness to {1} : {2}",
                    swSheetMetalFolder.GetFeature().Name, FormatLength(settings.ThicknessMeters, settings), Status(ok)));
            }
            catch (Exception exception)
            {
                results.Add(FeatureError("changing thickness", exception));
            }
            return results;
        }

        private IEnumerable<string> ChangeBendZone(ModelDoc2 swModelDoc, RunSettings settings)
        {
            var results = new List<string>();
            try
            {
                var swSheetMetalFolder = (SheetMetalFolder)swModelDoc.FeatureManager.GetSheetMetalFolder();
                if (swSheetMetalFolder == null)
                    return new[] { NoSheetMetalFolder("bend zone") };

                results.Add(ChangeSheetMetalFolderBendZone(swModelDoc, swSheetMetalFolder, settings, "NORMAL MODE"));
            }
            catch (Exception exception)
            {
                results.Add(FeatureError("changing bend zone", exception));
            }
            return results;
        }

        private IEnumerable<string> ChangeRadius(ModelDoc2 swModelDoc, RunSettings settings)
        {
            var results = new List<string>();
            try
            {
                var swSheetMetalFolder = (SheetMetalFolder)swModelDoc.FeatureManager.GetSheetMetalFolder();
                if (swSheetMetalFolder == null)
                    return new[] { NoSheetMetalFolder("radius") };

                var ok = new SheetMetalFolder2(swSheetMetalFolder).BendRadius(settings.RadiusMeters);
                if (ok)
                    swModelDoc.ForceRebuild3(true);

                results.Add(string.Format("\t * NORMAL MODE: changing {0}'s radius to {1} : {2}",
                    swSheetMetalFolder.GetFeature().Name, FormatLength(settings.RadiusMeters, settings), Status(ok)));
            }
            catch (Exception exception)
            {
                results.Add(FeatureError("changing radius", exception));
            }
            return results;
        }

        #endregion

        #region Force mode: changes are written on every feature

        private IEnumerable<string> ForceChangeThickness(ModelDoc2 swModelDoc, RunSettings settings)
        {
            var results = new List<string>();
            try
            {
                foreach (var swFeature in GetFeatures(swModelDoc, FeatureTypesWithThickness))
                {
                    dynamic data = swFeature.GetDefinition();
                    if (data == null)
                        continue;
                    data.Thickness = settings.ThicknessMeters;
                    bool ok = swFeature.ModifyDefinition((object)data, swModelDoc, null);
                    results.Add(string.Format("\t * FORCE MODE: changing {0}'s thickness to {1} : {2}",
                        swFeature.Name, FormatLength(settings.ThicknessMeters, settings), Status(ok)));
                }
            }
            catch (Exception exception)
            {
                results.Add(FeatureError("changing thickness", exception));
            }
            return results;
        }

        private IEnumerable<string> ForceChangeBendZone(ModelDoc2 swModelDoc, RunSettings settings)
        {
            var results = new List<string>();
            try
            {
                // Sheet metal folder first
                var swSheetMetalFolder = (SheetMetalFolder)swModelDoc.FeatureManager.GetSheetMetalFolder();
                if (swSheetMetalFolder != null)
                    results.Add(ChangeSheetMetalFolderBendZone(swModelDoc, swSheetMetalFolder, settings, "FORCE MODE"));

                // Then every feature that carries its own bend allowance
                foreach (var swFeature in GetFeatures(swModelDoc, FeatureTypesWithKFactor))
                {
                    dynamic data = swFeature.GetDefinition();
                    if (data == null)
                        continue;
                    CustomBendAllowance swCustomBendAllowance = data.GetCustomBendAllowance();
                    ApplyBendZone(swCustomBendAllowance, settings);
                    data.SetCustomBendAllowance(swCustomBendAllowance);
                    bool ok = swFeature.ModifyDefinition((object)data, swModelDoc, null);
                    results.Add(string.Format("\t * FORCE MODE: changing {0}'s {1} to {2} : {3}",
                        swFeature.Name, BendZoneDescription(settings), FormatBendZone(settings), Status(ok)));
                }
            }
            catch (Exception exception)
            {
                results.Add(FeatureError("changing bend zone", exception));
            }
            return results;
        }

        private IEnumerable<string> ForceChangeRadius(ModelDoc2 swModelDoc, RunSettings settings)
        {
            var results = new List<string>();
            double swSheetMetalThickness = 0;
            var radius = settings.RadiusMeters;

            try
            {
                foreach (var swFeature in GetFeatures(swModelDoc, FeatureTypesWithRadius))
                {
                    dynamic data = swFeature.GetDefinition();
                    if (data == null)
                        continue;

                    if (data is SheetMetalFeatureData)
                    {
                        data.SetOverrideDefaultParameter(true);
                        swSheetMetalThickness = data.Thickness;
                    }

                    if (data is OneBendFeatureData)
                        data.UseDefaultBendRadius = false;

                    if (data is BaseFlangeFeatureData)
                        data.OverrideRadius = true;

                    if (data is EdgeFlangeFeatureData)
                    {
                        data.UseDefaultBendRadius = false;

                        // Edge flange positioned "bend outside": offset the flange so the outside
                        // dimensions of the part are kept when the radius changes.
                        if (settings.ForceDimensionalRespect
                            && data.PositionType == (int)swFlangePositionTypes_e.swFlangePositionTypeBendOutside)
                        {
                            data.UsePositionOffset = true;
                            data.PositionOffsetType = (int)swFlangeOffsetTypes_e.swFlangeOffsetBlind;
                            double difference = radius - (double)data.BendRadius; // new radius - old radius
                            if (difference >= 0)
                                data.ReversePositionOffset = true;
                            data.PositionOffsetDistance = difference + 0.5 * swSheetMetalThickness;
                        }
                    }

                    data.BendRadius = radius;
                    bool ok = swFeature.ModifyDefinition((object)data, swModelDoc, null);
                    results.Add(string.Format("\t * FORCE MODE: changing {0}'s radius to {1} : {2}",
                        swFeature.Name, FormatLength(radius, settings), Status(ok)));
                }
            }
            catch (Exception exception)
            {
                results.Add(FeatureError("changing radius", exception));
            }
            return results;
        }

        #endregion

        #region Helpers

        private static IEnumerable<Feature> GetFeatures(ModelDoc2 swModelDoc, string[] typeNames)
        {
            var features = swModelDoc.FeatureManager.GetFeatures(false) as object[];
            if (features == null)
                return Enumerable.Empty<Feature>();
            return features.Cast<Feature>().Where(f => typeNames.Contains((string)f.GetTypeName())).ToList();
        }

        private static string ChangeSheetMetalFolderBendZone(ModelDoc2 swModelDoc, SheetMetalFolder swSheetMetalFolder, RunSettings settings, string mode)
        {
            Feature swSheetMetalFolderFeature = swSheetMetalFolder.GetFeature();
            var data = (SheetMetalFeatureData)swSheetMetalFolderFeature.GetDefinition();
            CustomBendAllowance swCustomBendAllowance = data.GetCustomBendAllowance();
            ApplyBendZone(swCustomBendAllowance, settings);
            data.SetCustomBendAllowance(swCustomBendAllowance);
            var ok = swSheetMetalFolderFeature.ModifyDefinition(data, swModelDoc, null);
            return string.Format("\t * {0}: changing {1}'s {2} to {3} : {4}",
                mode, swSheetMetalFolderFeature.Name, BendZoneDescription(settings), FormatBendZone(settings), Status(ok));
        }

        private static void ApplyBendZone(CustomBendAllowance swCustomBendAllowance, RunSettings settings)
        {
            switch (settings.BendZoneType)
            {
                case BendZoneKFactor:
                    swCustomBendAllowance.Type = (int)swBendAllowanceTypes_e.swBendAllowanceKFactor;
                    swCustomBendAllowance.KFactor = settings.BendZoneValue;
                    break;
                case BendZoneAllowance:
                    swCustomBendAllowance.Type = (int)swBendAllowanceTypes_e.swBendAllowanceDirect;
                    swCustomBendAllowance.BendAllowance = settings.BendZoneValue;
                    break;
                case BendZoneDeduction:
                    swCustomBendAllowance.Type = (int)swBendAllowanceTypes_e.swBendAllowanceDeduction;
                    swCustomBendAllowance.BendDeduction = settings.BendZoneValue;
                    break;
            }
        }

        private static string BendZoneDescription(RunSettings settings)
        {
            switch (settings.BendZoneType)
            {
                case BendZoneAllowance: return "bend allowance";
                case BendZoneDeduction: return "bend deduction";
                default: return "k-factor";
            }
        }

        private static string FormatBendZone(RunSettings settings)
        {
            return settings.BendZoneType == BendZoneKFactor
                ? settings.BendZoneValue.ToString("0.####", CultureInfo.InvariantCulture)
                : FormatLength(settings.BendZoneValue, settings);
        }

        private static string FormatLength(double meters, RunSettings settings)
        {
            return string.Format(CultureInfo.InvariantCulture, "{0:0.####} {1}",
                MeasuringUnitsConvertor.Length.FromMeters(meters, settings.UnitSystem),
                MeasuringUnitsConvertor.Length.UnitLabel(settings.UnitSystem));
        }

        private static string Status(bool ok)
        {
            return ok ? "SUCCESS" : "FAILURE";
        }

        private static string NoSheetMetalFolder(string what)
        {
            return string.Format("\t * Skipping {0}: no sheet metal folder found (not a sheet metal part?)", what);
        }

        private static string FeatureError(string action, Exception exception)
        {
            return string.Format("\t * ERROR while {0}: {1}", action, exception.Message);
        }

        #endregion

        #endregion
    }

    public class CustomTreeNode : TreeNode
    {
        public string FullFilePath { get; set; }

        /// <summary>True for part files, false for folders.</summary>
        public bool IsPart { get; private set; }

        public CustomTreeNode(string text, int imageindex, int selectedimageIndex, string fullFilePath, bool isPart)
        {
            Text = text;
            Name = text;
            SelectedImageIndex = selectedimageIndex;
            ImageIndex = imageindex;
            FullFilePath = fullFilePath;
            IsPart = isPart;
        }
    }

    public class OpenFileException : Exception
    {
        private readonly bool warning;
        private readonly int code;

        public OpenFileException(int errorOrWarningCode, bool isWarning)
        {
            code = errorOrWarningCode;
            warning = isWarning;
        }

        public override string Message
        {
            get
            {
                var text = warning
                    ? ((swFileLoadWarning_e)code).ToString()
                    : ((swFileLoadError_e)code).ToString();
                return text.Replace('_', ' ').Replace("sw", "SOLIDWORKS ");
            }
        }
    }
}

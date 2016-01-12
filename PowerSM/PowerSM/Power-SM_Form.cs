using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using SolidWorks.Interop.sldworks;
using SolidWorks.Interop.swconst;
using System.IO;
using System.IO.Compression;
namespace PowerSM
{
    public partial class Power_SM_Form : Form
    {

        string CurrentDirectory;
        string OutputDirectory;
        SldWorks swApp;
        List<CustomTreeNode> AllTreeViewNodes;
        List<string> swSheetMetalFeatureTypeWithRadiusProperty;
        List<string> swSheetMetalFeatureTypeWithThicknessProperty;
        delegate bool del(SldWorks swApp, string filename, double radius);
        public Power_SM_Form(SldWorks swApp_)
        {
            InitializeComponent();
            swApp = swApp_;
        }

        private void Power_SM_Form_Load(object sender, EventArgs e)
        {
            // Load icons for tree view 
            var rm = new System.Resources.ResourceManager("PowerSM.Properties.Resources", System.Reflection.Assembly.GetExecutingAssembly());
            Bitmap part = (Bitmap)rm.GetObject("part_png");
            Bitmap folder = (Bitmap)rm.GetObject("folder_png");

            FilesTreeView.ImageList = new ImageList();

            FilesTreeView.ImageList.Images.Add(part);
            FilesTreeView.ImageList.Images.Add(folder);
          
            swSheetMetalFeatureTypeWithRadiusProperty = new List<string>();
            swSheetMetalFeatureTypeWithRadiusProperty.AddRange(new[] {"SMBaseFlange",
                                                    "EdgeFlange",
                                                    "SMBaseFlange",
                                                    "SketchedBend",
                                                    "SheetMetal",
                                                    "OneBend",
                                                    "SMMiteredFlange",
                                                    "Jog",
                                                    "Bends"});
        

        swSheetMetalFeatureTypeWithThicknessProperty = new List<string>();
        swSheetMetalFeatureTypeWithThicknessProperty.AddRange(new[] {"SMBaseFlange",
                                                    "SMBaseFlange",
                                                    "LoftedBends",
                                                    "SheetMetal",
                                                    "OneBend",
                                                    });
        }
    


    #region UI methods

    #region Browse for files
    private void BrowseForFolderButton_Click(object sender, EventArgs e)
        {
            InitiateOpenFolder();

        }

        private void InitiateOpenFolder()
        {
            FolderBrowserDialog FolderBrowser = new FolderBrowserDialog();
            FolderBrowser.ShowDialog();

            if (!string.IsNullOrEmpty(FolderBrowser.SelectedPath))
            {
                // List all directories
                CurrentDirectory = FolderBrowser.SelectedPath;
                ListDirectory(FilesTreeView, CurrentDirectory);

                //Get all tree nodes from treeview
                AllTreeViewNodes = new List<CustomTreeNode>();
                GetAllNodesFromTreeView(FilesTreeView, AllTreeViewNodes);

                // Select all nodes
                selectAllToolStripMenuItem.PerformClick();
            }
        }
        private void OutputFolder()
        {
            FolderBrowserDialog FolderBrowser = new FolderBrowserDialog();
            FolderBrowser.ShowDialog();

            if (!string.IsNullOrEmpty(FolderBrowser.SelectedPath))
            {
                // List all directories
                 OutputDirectory = FolderBrowser.SelectedPath;
                
            }
        }
        private void ListDirectory(TreeView treeView, string path)
        {
            treeView.Nodes.Clear();
            var rootDirectoryInfo = new DirectoryInfo(path);
            treeView.Nodes.Add(CreateDirectoryNode(rootDirectoryInfo));
        }

        private TreeNode CreateDirectoryNode(DirectoryInfo directoryInfo)
        {
            List<string> FileExtensions = new List<string>();
            FileExtensions.AddRange(new[] { "SLDPRT", "sldprt" });

            var directoryNode = new CustomTreeNode(directoryInfo.Name, (int)PowerSMEnums.TreeViewIcons.directory, (int)PowerSMEnums.TreeViewIcons.directory, directoryInfo.FullName);

            foreach (var directory in directoryInfo.GetDirectories())
                // Do not add hiddens directories
                if (!directory.Attributes.HasFlag(FileAttributes.Hidden))
                    directoryNode.Nodes.Add(CreateDirectoryNode(directory));

            foreach (var file in directoryInfo.GetFiles())
            {
                if (FileExtensions.Any(x => file.Extension.Contains(x)))
                {
                    directoryNode.Nodes.Add(new CustomTreeNode(file.Name, (int)PowerSMEnums.TreeViewIcons.file, (int)PowerSMEnums.TreeViewIcons.file, file.FullName));

                }
            }
            return directoryNode;
        }
        #endregion

        private void OpenFolder_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(CurrentDirectory))
                System.Diagnostics.Process.Start("explorer.exe", CurrentDirectory);
        }
        private void BrowseForOutPutFolder_Click(object sender, EventArgs e)
        {
            OutputFolder();
        }
        private void optionsToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            var powersmoptions = new PowerSMOptions();
            powersmoptions.ShowDialog();
        }


        #region UI: Check/uncheck children feature
        private void FilesTreeView_AfterCheck(object sender, TreeViewEventArgs e)
        {   // MSDN:
            // The code only executes if the user caused the checked state to change.
            if (e.Action != TreeViewAction.Unknown)
            {
                if (e.Node.Nodes.Count > 0)
                {   // MSDN:
                    /* Calls the CheckAllChildNodes method, passing in the current 
                    Checked value of the TreeNode whose checked state changed. */
                    this.CheckAllChildNodes(e.Node, e.Node.Checked);
                }
            }
        }
        private void CheckAllChildNodes(TreeNode treeNode, bool nodeChecked)
        {
            foreach (TreeNode node in treeNode.Nodes)
            {
                node.Checked = nodeChecked;
                if (node.Nodes.Count > 0)
                {
                    // If the current node has child nodes, call the CheckAllChildsNodes method recursively.
                    this.CheckAllChildNodes(node, nodeChecked);
                }
            }
        }


        #endregion

        private void SaveLog_Click(object sender, EventArgs e)
        {
            SaveFileDialog _SaveFileDialog = new SaveFileDialog();
            _SaveFileDialog.Filter = "Text files (*.txt)|*.txt|All files (*.*)|*.*";
            _SaveFileDialog.FileOk += SaveFileDialog_FileOK;
            _SaveFileDialog.ShowDialog();
        }

        private void SaveFileDialog_FileOK(object sender, EventArgs e)
        {
            var _SaveFileDialog = (SaveFileDialog)sender;
            try
            {
                System.IO.File.WriteAllText(_SaveFileDialog.FileName, LogTextBox.Text);
            }
            catch (Exception)
            {
                ErrorEchoer.Err((int)PowerSMEnums.Errors.Cannot_Write_Log_File);
            }
        }

        private void quitToolStripMenuItem_Click(object sender, EventArgs e)
        {

            this.Close();
        }

        private void aboutThisToolToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AboutBox aboutbox = new AboutBox();
            aboutbox.ShowDialog();
        }

        private void selectAllToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (AllTreeViewNodes != null)
            {
                foreach (TreeNode treenode in AllTreeViewNodes)
                {
                    treenode.Checked = true;
                }
            }
        }
        private void unSelectAllToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (AllTreeViewNodes != null)
            {
                foreach (TreeNode treenode in AllTreeViewNodes)
                {
                    treenode.Checked = false;
                }
            }
        }
        #endregion

        #region GetAllNodes methods
        private static void GetAllNodesFromTreeView(TreeView treeview, List<CustomTreeNode> AllTreeViewNodes)
        {
            AllTreeViewNodes.RemoveAll(x => x != null);
            foreach (CustomTreeNode treenode in treeview.Nodes)
            {
                GetChildrenNodesFromNode(treenode, AllTreeViewNodes);
            }

        }
        private static void GetChildrenNodesFromNode(CustomTreeNode treenode, List<CustomTreeNode> AllTreeViewNodes)
        {
            AllTreeViewNodes.Add(treenode);
            foreach (CustomTreeNode childtreenode in treenode.Nodes)
                GetChildrenNodesFromNode(childtreenode, AllTreeViewNodes);
        }
        #endregion


        private async void StartButton_Click(object sender, EventArgs e)
        {
            // Radius must be a double
            double Radius;
            if (!double.TryParse(RadiusTextBox.Text, out Radius))
            {
                ErrorEchoer.Err((int)PowerSMEnums.Errors.Cannot_parse_radius_value);
                return;
            }

            double thickness;
            if (!double.TryParse(ThicknessTextBox.Text, out thickness))
            {
                ErrorEchoer.Err((int)PowerSMEnums.Errors.Cannot_parse_radius_value);
                return;
            }

            decimal kfactor;
            if (!decimal.TryParse(KFactorTextBox.Text, out kfactor))
            {
                ErrorEchoer.Err((int)PowerSMEnums.Errors.Cannot_parse_radius_value);
                return;
            }


            // create selectednodes list 

            List<CustomTreeNode> SelectedTreeNodes = AllTreeViewNodes.FindAll(x => x.Checked == true).FindAll(x => File.Exists(x.FullFilePath));

            // return if no tree nodes are selected 
            if (SelectedTreeNodes.Count == 0)
                return;

            // housekeeping: Progressbar, LogTextBox
            _ProgressBar.Value = 0;
            _ProgressBar.Minimum = 0;
            var ProgressMaximum = SelectedTreeNodes.Count;
            _ProgressBar.Step = (int)(ProgressMaximum / SelectedTreeNodes.Count);
            _ProgressBar.Maximum = ProgressMaximum;
            LogTextBox.Text = string.Empty;

            // sleeves roll-up:


            foreach (CustomTreeNode element in SelectedTreeNodes)
            {

                Task<string[]> ChangeSheetMetalFeaturesAsyncTask = ChangeSheetMetalFeaturesAsync(element, Radius,thickness,kfactor);
                string[] result = await ChangeSheetMetalFeaturesAsyncTask;
                _ProgressBar.PerformStep();
                toolStripStatusLabel.Text = ((_ProgressBar.Value * 1.0) / (_ProgressBar.Maximum * 1.0)).ToString("P", System.Globalization.CultureInfo.InvariantCulture);
                LogTextBox.Text += string.Join(System.Environment.NewLine, result);

            }

            // Compress

            if (!(string.IsNullOrEmpty(OutputDirectory) && true) )
            {
                toolStripStatusLabel.Text = "Compressing...";

                Task task = Task.Run(() =>
               System.IO.Compression.ZipFile.CreateFromDirectory(@"C\Temp\PowerSM", OutputDirectory)
               );

                task.Start();
                task.Wait();
            }

            toolStripStatusLabel.Text = "Ready";

        }
        
        private async Task<string[]> ChangeSheetMetalFeaturesAsync(CustomTreeNode customtreenode, double radius,double thickness,decimal kfactor)
        {

            return await Task.Run(() => ChangeSheetMetalFeatures(customtreenode, radius,thickness, kfactor));

        }


        #region ChangeSheetMetalFeatures

        private string[] ChangeSheetMetalFeatures(CustomTreeNode customtreenode, double radius, double thickness, decimal kfactor)
        {
            List<string> Results = new List<string>();
            ModelDoc2 swModelDoc;

           try
            {
                int Error = 0;
                int Warning = 0;
                Results.Add(string.Format("[{0}] PART FILENAME: {1}", DateTime.Now.ToString(), customtreenode.FullFilePath)); 

                swApp.DocumentVisible(false, (int)swDocumentTypes_e.swDocPART);
                swModelDoc = swApp.OpenDoc6(customtreenode.FullFilePath, (int)swDocumentTypes_e.swDocPART, (int)swOpenDocOptions_e.swOpenDocOptions_Silent, string.Empty, ref Error, ref Warning);

                // All errors and warnings would throw a an error
                if (Warning != 0)
                {
                    throw new OpenFileException(Warning, true);
                }
                else if (Error != 0)
                {
                    throw new OpenFileException(Error, false);
                }

            
                // change thickness
                if (thickness > 0)
                {
                    Results.AddRange(ChangeThickness(swModelDoc, thickness));

                }
                // change k-factor
                if (kfactor > 0)
                {
                    Results.AddRange(ChangeKFactor(swModelDoc, kfactor));

                }

                // change radius

                if (radius > 0)
                {
                    Results.AddRange(ChangeRadius(swModelDoc, radius));

                }
                // save 

                if (Convert.ToBoolean(System.Configuration.ConfigurationSettings.AppSettings["SaveAsZip"]) == true)
                {
                   
                    // Delete old temp folder 
                    string TempFolder = @"C:\Temp\PowerSM";
                    System.IO.Directory.Delete(TempFolder);
                    // Create new temp folder
                    System.IO.Directory.CreateDirectory(TempFolder);
                    string newfilename = string.Format(@"{0}\{1}", TempFolder, customtreenode.FullPath);
                    Results.Add(System.Environment.NewLine);
                    swModelDoc.SaveAs(newfilename);
                    swApp.CloseDoc(string.Empty);
                    Results.Add(System.Environment.NewLine);
                    swApp.DocumentVisible(true, (int)swDocumentTypes_e.swDocPART);
                }
                else if (!String.IsNullOrEmpty(OutputDirectory))
                {   string newfilename = string.Format(@"{0}\{1}",OutputDirectory,customtreenode.FullPath);
                    Results.Add(System.Environment.NewLine);
                    swModelDoc.SaveAs(newfilename);
                    swApp.CloseDoc(string.Empty);
                    swApp.DocumentVisible(true, (int)swDocumentTypes_e.swDocPART);
                }
                else
                {
                    // Save part
                    swModelDoc.Save();
                    swApp.CloseDoc(string.Empty);
                    Results.Add(System.Environment.NewLine);
                    swApp.DocumentVisible(true, (int)swDocumentTypes_e.swDocPART);
                }
                return Results.ToArray<string>();

            }
            catch(Exception exception)
            {
                var ErrorString = string.Format("[{0}] - ERROR: {1}", DateTime.Now.ToString(), exception.Message);
                Results.Add(ErrorString);
                Results.Add(System.Environment.NewLine);
                swApp.DocumentVisible(true, (int)swDocumentTypes_e.swDocPART);
                swApp.CloseDoc(string.Empty);
                return Results.ToArray<string>();
            }
        

            
        }

        private string[] ChangeRadius(ModelDoc2 swModelDoc, double radius)
        {

            double swSheetMetalThickness = 0;
           
            List<string> Results = new List<string>();
     

            try
            {
                

                // Check unit system and convert radius to corresponding value
                int LengthUnit = swModelDoc.LengthUnit;
                switch (LengthUnit)
                {
                    case (int)swLengthUnit_e.swMIL:
                        {
                            if (UnitSystemCombBox.Text == "MMGS")
                                break;
                            else
                                radius = 0.0393701 * radius;
                            break;
                        }
                    case (int)swLengthUnit_e.swINCHES:
                        if (UnitSystemCombBox.Text == "IPS")
                            break;
                        else
                            radius = radius / 0.0393701;
                        break;
                    default:
                        break;
                }

                FeatureManager swFeatureManager = swModelDoc.FeatureManager;
                var swFeatures = swFeatureManager.GetFeatures(false);
                foreach (Feature swFeature in swFeatures)
                {
                    dynamic swSheetMetalDataFeature = swSheetMetalFeatureTypeWithRadiusProperty.Exists(x => x == swFeature.GetTypeName()) ? swFeature.GetDefinition() : null;
                    if (swSheetMetalDataFeature == null)
                        continue;

                    if (swSheetMetalDataFeature is SheetMetalFeatureData)
                    {
                       swSheetMetalDataFeature.SetOverrideDefaultParameter(true);
                       swSheetMetalThickness  = swSheetMetalDataFeature.Thickness; 
                    }
                    if (swSheetMetalDataFeature is OneBendFeatureData)
                        swSheetMetalDataFeature.UseDefaultBendRadius = false;

                    if (swSheetMetalDataFeature is BaseFlangeFeatureData)
                        swSheetMetalDataFeature.OverrideRadius = true;

                    if (swSheetMetalDataFeature is EdgeFlangeFeatureData)
                        swSheetMetalDataFeature.UseDefaultBendRadius = false;
                    {
                        // If Edge Flange's bend is outside of the base flange, compensation is needed in order to respect outside dimensions
                        if (Convert.ToBoolean(System.Configuration.ConfigurationSettings.AppSettings["ForceDimensionalRespect"]))
                        {
                            if (swSheetMetalDataFeature.PositionType == (int)swFlangePositionTypes_e.swFlangePositionTypeBendOutside)
                            {
                                var dif = radius * 1000.0 - swSheetMetalDataFeature.BendRadius; // difference between old radius and new radius
                                if (dif >= 0)
                                {
                                    swSheetMetalDataFeature.OffsetDistance = dif + 0.5 * swSheetMetalThickness;
                                }
                                else
                                {
                                    swSheetMetalDataFeature.ReversePositionOffset = true;
                                    swSheetMetalDataFeature.OffsetDistance = dif + 0.5 * swSheetMetalThickness;

                                }
                            }
                        }        
                    }

                    swSheetMetalDataFeature.BendRadius = (radius * 1.0) / 1000.0;
                    bool FeatureResult = swFeature.ModifyDefinition((object)swSheetMetalDataFeature, swModelDoc, null);
                    Results.Add(string.Format("\t * changing {0}'s radius to {1} mm: {2}.", swFeature.Name, radius.ToString(), FeatureResult ? "SUCCESS" : "FAILURE"));


                }

              
                return Results.ToArray<string>();

            }
            catch (Exception exception)
            {

                return Results.ToArray<string>();

            }
        }
        private string[] ChangeKFactor(ModelDoc2 swModelDoc, decimal KFactor)
        {
            List<string> Results = new List<string>();

            var swCustomAllowance = new CustomBendAllowance();
            swCustomAllowance.KFactor = (double)KFactor;

            try
            {

                FeatureManager swFeatureManager = swModelDoc.FeatureManager;
                var swFeatures = swFeatureManager.GetFeatures(false);
                foreach (Feature swFeature in swFeatures)
                {
                    dynamic swSheetMetalDataFeature = swSheetMetalFeatureTypeWithThicknessProperty.Exists(x => x == swFeature.GetTypeName()) ? swFeature.GetDefinition() : null;
                    if (swSheetMetalDataFeature == null)
                        continue;
                    swSheetMetalDataFeature.SetCustomBendAllowance(swCustomAllowance);
                    bool FeatureResult = swFeature.ModifyDefinition((object)swSheetMetalDataFeature, swModelDoc, null);
                    Results.Add(string.Format("\t * changing {0}'s kfactor to {1} mm: {2}.", swFeature.Name, KFactor.ToString(), FeatureResult ? "SUCCESS" : "FAILURE"));


                }


                return Results.ToArray<string>();

            }
            catch (Exception exception)
            {

                return Results.ToArray<string>();

            }
        }
       private string[] ChangeThickness(ModelDoc2 swModelDoc, double Thickness)
        {
           

            List<string> Results = new List<string>();


            try
            {


                // Check unit system and convert radius to corresponding value
                int LengthUnit = swModelDoc.LengthUnit;
                switch (LengthUnit)
                {
                    case (int)swLengthUnit_e.swMIL:
                        {
                            if (UnitSystemCombBox.Text == "MMGS")
                                break;
                            else
                                Thickness = 0.0393701 * Thickness;
                            break;
                        }
                    case (int)swLengthUnit_e.swINCHES:
                        if (UnitSystemCombBox.Text == "IPS")
                            break;
                        else
                            Thickness = Thickness / 0.0393701;
                        break;
                    default:
                        break;
                }

                FeatureManager swFeatureManager = swModelDoc.FeatureManager;
                var swFeatures = swFeatureManager.GetFeatures(false);
                foreach (Feature swFeature in swFeatures)
                {
                    dynamic swSheetMetalDataFeature = swSheetMetalFeatureTypeWithThicknessProperty.Exists(x => x == swFeature.GetTypeName()) ? swFeature.GetDefinition() : null;
                    if (swSheetMetalDataFeature == null)
                        continue;
                    swSheetMetalDataFeature.Thickness = Thickness * 1000.0;
                    bool FeatureResult = swFeature.ModifyDefinition((object)swSheetMetalDataFeature, swModelDoc, null);
                    Results.Add(string.Format("\t * changing {0}'s thickness to {1} mm: {2}.", swFeature.Name, Thickness.ToString(), FeatureResult ? "SUCCESS" : "FAILURE"));


                }


                return Results.ToArray<string>();

            }
            catch (Exception exception)
            {

                return Results.ToArray<string>();

            }
        }




        #endregion

      
    }
    public class CustomTreeNode : TreeNode
    {
        public string FullFilePath { get; set; }


        public CustomTreeNode(string text, int imageindex, int selectedimageIndex, string _FullFilePath)
        {
            base.Text = text;
            base.Name = text;
            base.SelectedImageIndex = selectedimageIndex;
            base.ImageIndex = imageindex;
            FullFilePath = _FullFilePath;
        }
    }

    public class OpenFileException : Exception
    {
        bool warning;
        int exception_warning_error;
        public OpenFileException(int _exception_warning_error, bool _warning)
        {

            exception_warning_error = _exception_warning_error;
            warning = _warning;
        }
        public override string Message
        {
            get
            {
                if (warning)
                {
                    swFileLoadWarning_e error = (swFileLoadWarning_e)exception_warning_error;
                    return error.ToString().Replace('_', ' ').Replace("sw", "SolidWorks ");
                }
                else
                {
                    swFileLoadError_e error = (swFileLoadError_e)exception_warning_error;
                    return error.ToString().Replace('_', ' ').Replace("sw", "SolidWorks ");
                }

            }
        }

    }



}
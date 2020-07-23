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
using System.Globalization;
namespace PowerSM
{
    public partial class PowerGeometryForm : Form
    {

        string CurrentDirectory;
        string OutputDirectory;
        SldWorks swApp;
        List<CustomTreeNode> AllTreeViewNodes;
        List<string> swSheetMetalFeatureTypeWithRadiusProperty;
        List<string> swSheetMetalFeatureTypeWithThicknessProperty;
        List<string> swSheetMetalFeatureTypeWithKFactorProperty;
        delegate bool del(SldWorks swApp, string filename, double radius);

        public PowerGeometryForm(SldWorks swApp_)
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
                                                    "SketchBend",
                                                    "SheetMetal",
                                                    "OneBend",
                                                    "SMMiteredFlange",
                                                    "Jog",
                                                    "ProcessBends"});
            swSheetMetalFeatureTypeWithKFactorProperty = new List<string>();
            swSheetMetalFeatureTypeWithKFactorProperty.AddRange(new[] {
                                                    "EdgeFlange",
                                                    "SheetMetal",
                                                    "OneBend",
                                                    "Hem",
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

                // Add curren directory to recent directory menu strip 
                System.Configuration.ConfigurationSettings.AppSettings["RecentDirectory"] = CurrentDirectory;

                RecentToolStripMenu.Text = CurrentDirectory;
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
            var powersmoptions = new PowerGeometryOptions();
            powersmoptions.Owner = this;
            powersmoptions.StartPosition = FormStartPosition.CenterParent;
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
            if (!double.TryParse(String.IsNullOrWhiteSpace(RadiusTextBox.Text) ? "0" : RadiusTextBox.Text, out Radius))
            {
                ErrorEchoer.Err((int)PowerSMEnums.Errors.Cannot_parse_radius_value);
                return;
            }

            double thickness;
            if (!double.TryParse(String.IsNullOrWhiteSpace(ThicknessTextBox.Text) ? "0" : ThicknessTextBox.Text, out thickness))
            {
                ErrorEchoer.Err((int)PowerSMEnums.Errors.cannot_parse_thickness);
                return;
            }

            double BendZoneValue;
             
            if (!double.TryParse(String.IsNullOrWhiteSpace(BendZoneTextBox.Text) ? "0" : BendZoneTextBox.Text, out BendZoneValue))
            {
               ErrorEchoer.Err((int)PowerSMEnums.Errors.cannot_parse_kfacor);
               return; 
            }

            if (BendZoneValue > 1)
            {
                ErrorEchoer.Err((int)PowerSMEnums.Errors.cannot_parse_kfacor);
                return;
            }

            // check if list has nodes

            if (FilesTreeView.Nodes.Count == 0)
            {
                ErrorEchoer.Err((int)PowerSMEnums.Errors.Empty_Tree);
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

            // if ArchiveZipFormat is true, prepare temp directory etc..
            if (Convert.ToBoolean(System.Configuration.ConfigurationSettings.AppSettings["ArchiveInZipFormat"]))
            PrepareZip();
            // sleeves roll-up:

            Boolean force = Convert.ToBoolean(System.Configuration.ConfigurationSettings.AppSettings["Force"]);
            foreach (CustomTreeNode element in SelectedTreeNodes)
            {

                Task<string[]> ChangeSheetMetalFeaturesAsyncTask = ChangeSheetMetalFeaturesAsync(element, Radius,thickness,BendZoneValue,force);
                string[] result = await ChangeSheetMetalFeaturesAsyncTask;
                _ProgressBar.PerformStep();
                toolStripStatusLabel.Text = ((_ProgressBar.Value * 1.0) / (_ProgressBar.Maximum * 1.0)).ToString("P", System.Globalization.CultureInfo.InvariantCulture);
                LogTextBox.Text += string.Join(System.Environment.NewLine, result);

            }

            // Compress

            if ((!(string.IsNullOrEmpty(OutputDirectory)) && Convert.ToBoolean(System.Configuration.ConfigurationSettings.AppSettings["ArchiveInZipFormat"])))
            {
            

                toolStripStatusLabel.Text = "Compressing...";

                Task task = Task.Run(() =>
               {
                  
                   System.IO.Compression.ZipFile.CreateFromDirectory(@"C:\Temp\PowerSM", String.Format(@"{0}\powersm.zip",OutputDirectory));

               }

               );
                task.Wait();
            }

            toolStripStatusLabel.Text = "Ready";

        }
        
        private async Task<string[]> ChangeSheetMetalFeaturesAsync(CustomTreeNode customtreenode, double radius,double thickness, double BendZoneValue, Boolean force)
        {

            return await Task.Run(() => ChangeSheetMetalFeatures(customtreenode, radius,thickness, BendZoneValue, force));

        }


        #region ChangeSheetMetalFeatures

        private void PrepareZip()
        {
            string TempFolder = @"C:\Temp\PowerSM";
            // Create new temp folder
            System.IO.Directory.CreateDirectory(TempFolder);

            // Delete old temp folder
            System.IO.DirectoryInfo tempfolderdirectory = new DirectoryInfo(TempFolder);
            foreach (FileInfo file in tempfolderdirectory.GetFiles())
            {
                file.Delete();
            }
            foreach (DirectoryInfo dir in tempfolderdirectory.GetDirectories())
            {
                dir.Delete(true);
            }
            // Create new temp folder
            System.IO.Directory.CreateDirectory(TempFolder);
            // try to delete old archivage
            try 
            {
                System.IO.File.Delete(String.Format(@"{0}\powersm.zip", OutputDirectory));
            }
            catch (Exception) 
            {
            }
        }
        private string[] ChangeSheetMetalFeatures(CustomTreeNode customtreenode, double radius, double thickness, double BendZoneValue, Boolean force)
        {
            

            List<string> Results = new List<string>();
            ModelDoc2 swModelDoc;

           try
            {
                StartButton.Enabled = false;
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


              if (force) { 
                // change thickness
                if (thickness > 0)
                {
                    Results.AddRange(ForceChangeThickness(swModelDoc, thickness));

                }
                // change BendZone value
                if (BendZoneValue > 0)
                {
                    Results.AddRange(ForceChangeBendZone(swModelDoc, BendZoneValue));

                }

                // change radius

                if (radius > 0)
                {

                    Results.AddRange(ForceChangeRadius(swModelDoc, radius));
                }

                   
                }
                 // force is false. It will change the sheet metal features and will the changes propegate through the sub-features
                
              else
                {
                    if (Convert.ToBoolean(System.Configuration.ConfigurationSettings.AppSettings["SetOverrideToFalse"]) == true) { 
                    #region "Remove all overriding parameters"

                    FeatureManager swFeatureManager = swModelDoc.FeatureManager;
                    var swFeatures = swFeatureManager.GetFeatures(false);
                    var swSheetMetalFolder = (SheetMetalFolder)swFeatureManager.GetSheetMetalFolder();
                    // remove any overridden parameters from sheet metal 
                    foreach (Object FeatObj in swSheetMetalFolder.GetSheetMetals())
                    {
                        var swSheetMetalFeature = (Feature)FeatObj;
                        SheetMetalFeatureData swSheetMetalFeatureData = swSheetMetalFeature.GetDefinition();
                        swSheetMetalFeatureData.SetOverrideDefaultParameter(false);
                       bool r = swSheetMetalFeature.ModifyDefinition((object)swSheetMetalFeatureData, swModelDoc, null);
                       Results.Add(string.Format("\n\t * Trying to make sheet named {1} inherit from parent SM folder: {0}...\n", r.ToString(), swSheetMetalFeature.Name));
                        }

                    // make sure sheet metal features use default parameters 
                    foreach (Object FeatObj in swFeatures)
                    {
                        var swSheetMetalFeature = (Feature)FeatObj;
                         
                        // set default radius and bend allowance to true
                        dynamic swSheetMetalFeatureData = (new List<string> {
                                                    "EdgeFlange",
                                                    "SM3dBend",
                                                    "SketchBend",
                                                    "OneBend", 
                                                    "SMMiteredFlange",
                                                    "Jog",
                                                    "Bends",
                                                    "ProcessBends",
                                                    "FlattenBends",
                        }).Exists((x) => x == swSheetMetalFeature.GetTypeName2()) ? swSheetMetalFeature.GetDefinition() : null;
                        if (swSheetMetalFeatureData != null)
                        {
                                swSheetMetalFeatureData.UseDefaultBendRadius = true;
                            swSheetMetalFeatureData.UseDefaultBendAllowance = true;
                           bool r =  swSheetMetalFeature.ModifyDefinition((object)swSheetMetalFeatureData, swModelDoc, null);
                           Results.Add(string.Format("\n\t * Attempting to make feature named {1} inherit parameters from parent feature/sheet: {0}...\n", r.ToString(), swSheetMetalFeature.Name));
                            }
                        // set default radius and thickness for base flange
                        dynamic swBaseFlangeFeatureData = "SMBaseFlange" == swSheetMetalFeature.GetTypeName2() ? swSheetMetalFeature.GetDefinition() : null;
                        if (swBaseFlangeFeatureData != null)
                        {
                                if (swBaseFlangeFeatureData.UseGaugeTable == true)
                                {
                                    swBaseFlangeFeatureData.OverrideThickness = false;
                                    swBaseFlangeFeatureData.OverrideRadius = false;
                                    swBaseFlangeFeatureData.OverrideKFactor = false;
                                    bool r = swSheetMetalFeature.ModifyDefinition((object)swBaseFlangeFeatureData, swModelDoc, null);
                                    Results.Add(string.Format("\n\t * Attempting to set override of base flange feature named {1} to false: {0}...\n", r.ToString(), swSheetMetalFeature.Name));

                                }
                            }


                            dynamic swHemSheetMetalFeatureData = "Hem" == swSheetMetalFeature.GetTypeName2() ? swSheetMetalFeature.GetDefinition() : null;
                            if (swHemSheetMetalFeatureData != null)
                            {
                                swHemSheetMetalFeatureData.UseDefaultBendAllowance = false;
                                bool r = swSheetMetalFeature.ModifyDefinition((object)swHemSheetMetalFeatureData, swModelDoc, null);
                                Results.Add(string.Format("\n\t * Attempting to make hem feature named {1} uses default bend allowance: {0}...\n", r.ToString(), swSheetMetalFeature.Name));
                            }


                            dynamic swBendReliefSheetMetalFeatureData= (new List<string> {"EdgeFlange","OneBend","SketchBend"}).Exists( (x) => x ==  swSheetMetalFeature.GetTypeName2()) ? swSheetMetalFeature.GetDefinition() : null;
                            if (swBendReliefSheetMetalFeatureData != null)
                            {
                                swBendReliefSheetMetalFeatureData.UseDefaultBendRelief = false;
                                bool r = swSheetMetalFeature.ModifyDefinition((object)swBendReliefSheetMetalFeatureData, swModelDoc, null);
                                Results.Add(string.Format("\n\t * Attempting to make feature named {1} uses default bend relief: {0}...\n", r.ToString(), swSheetMetalFeature.Name));
                            }
                    


                    }
                        #endregion
                    }


                        // change thickness
                        if (thickness > 0)
                    {
                        
                        Results.AddRange(ChangeThickness(swModelDoc, thickness));

                    }
                    // change k-factor
                    if (BendZoneValue > 0)
                    {
                      
                        Results.AddRange(ChangeBendZone(swModelDoc, BendZoneValue));

                    }

                    // change radius

                    if (radius > 0)
                    {

                        Results.AddRange(ChangeRadius(swModelDoc, radius));
                    }

                }

                // save 
                if (Convert.ToBoolean(System.Configuration.ConfigurationSettings.AppSettings["ArchiveInZipFormat"]) && !String.IsNullOrEmpty(OutputDirectory))
                {
                    string TempFolder = @"C:\Temp\PowerSM";
                    string newfilename = string.Format(@"{0}\{1}", TempFolder, customtreenode.FullPath);
                    // create full directory for new file
                    System.IO.Directory.CreateDirectory(newfilename);
                    System.IO.Directory.Delete(newfilename);
                    // save
                    var saveresult = swModelDoc.Extension.SaveAs(newfilename, (int)swSaveAsVersion_e.swSaveAsCurrentVersion, (int)swSaveAsOptions_e.swSaveAsOptions_Silent, null, ref Error, ref Warning);
                    Results.Add(string.Format("\n\t * Saving and compressing... : {0} Error: {1} Warning: {2} ", saveresult.ToString(), Error, Warning));
                    swApp.CloseDoc(string.Empty);
                    Results.Add(System.Environment.NewLine);
                    swApp.DocumentVisible(true, (int)swDocumentTypes_e.swDocPART);
                }

               else if (!String.IsNullOrEmpty(OutputDirectory))
               {
                   string newfilename = string.Format(@"{0}\{1}", OutputDirectory, customtreenode.FullPath);
                   // create full directory for new file
                   System.IO.Directory.CreateDirectory(newfilename);
                   System.IO.Directory.Delete(newfilename);
                   // save
                   var saveresult = swModelDoc.Extension.SaveAs(newfilename, (int)swSaveAsVersion_e.swSaveAsCurrentVersion, (int)swSaveAsOptions_e.swSaveAsOptions_Silent, null, ref Error, ref Warning);
                   Results.Add(string.Format("\n\t * Saving to output directory... : {0} Error: {1} Warning: {2} \n", saveresult.ToString(), Error, Warning));
                   Results.Add(System.Environment.NewLine);
                   swApp.CloseDoc(string.Empty);
                   swApp.DocumentVisible(true, (int)swDocumentTypes_e.swDocPART);
               }
               
                else
                {
                    // Save part
                    Results.Add(string.Format("\n\t * Saving file...\n"));
                    Results.Add(System.Environment.NewLine);
                    swModelDoc.Save();
                    swApp.CloseDoc(string.Empty);
                    Results.Add(System.Environment.NewLine);
                    swApp.DocumentVisible(true, (int)swDocumentTypes_e.swDocPART);
                    
                }
                StartButton.Enabled = true;
                return Results.ToArray<string>();

            }
            catch(Exception exception)
            {
                var ErrorString = string.Format("[{0}] - ERROR: {1}", DateTime.Now.ToString(), exception.Message);
                Results.Add(ErrorString);
                Results.Add(System.Environment.NewLine);
                swApp.DocumentVisible(true, (int)swDocumentTypes_e.swDocPART);
                swApp.CloseDoc(string.Empty);
                StartButton.Enabled = true;
                return Results.ToArray<string>();
            }
        

            
        }


     
      
        private string[] ForceChangeRadius(ModelDoc2 swModelDoc, double radius)
        {

            double swSheetMetalThickness = 0;
           
            List<string> Results = new List<string>();
     

            try
            {
                

                // Check unit system and convert radius to corresponding value
                int LengthUnit = swModelDoc.LengthUnit;
                switch (LengthUnit)
                {
                    case (int)swLengthUnit_e.swMM:
                        {
                            if (UnitSystemCombBox.Text == "MMGS")
                            {
                                //radius = radius / 1000.0;

                            }

                            else
                            {
                                radius = MeasuringUnitsConvertor.Length.FromInchToMillimeters(radius);
                            }
                            break;
                        }
                    case (int)swLengthUnit_e.swINCHES:
                        if (UnitSystemCombBox.Text == "IPS")
                        {

                            //radius = radius / 1000.0;

                        }
                        else
                        {

                            radius = MeasuringUnitsConvertor.Length.FromMillimetersToInchs(radius);


                        }
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
                    {
                        swSheetMetalDataFeature.UseDefaultBendRadius = false;
                        // If Edge Flange's bend is outside of the base flange, compensation is needed in order to respect outside dimensions
                        if (Convert.ToBoolean(System.Configuration.ConfigurationSettings.AppSettings["ForceDimensionalRespect"]))
                        {
                           
                            if (swSheetMetalDataFeature.PositionType == (int)swFlangePositionTypes_e.swFlangePositionTypeBendOutside)
                            {
                                swSheetMetalDataFeature.UsePositionOffset = true;
                                var dif = radius - swSheetMetalDataFeature.BendRadius; // difference between old radius and new radius
                                if (dif >= 0)
                                {
                                    swSheetMetalDataFeature.PositionOffsetType = (int)swFlangeOffsetTypes_e.swFlangeOffsetBlind;
                                    swSheetMetalDataFeature.ReversePositionOffset = true;
                                    swSheetMetalDataFeature.PositionOffsetDistance = dif + 0.5 * swSheetMetalThickness;
                                }
                                else
                                {
                                    swSheetMetalDataFeature.PositionOffsetType = (int)swFlangeOffsetTypes_e.swFlangeOffsetBlind;
                                    swSheetMetalDataFeature.PositionOffsetDistance = dif + 0.5 * swSheetMetalThickness;

                                }
                            }
                        }        
                    }

                    swSheetMetalDataFeature.BendRadius = (radius * 1.0);
                    bool FeatureResult = swFeature.ModifyDefinition((object)swSheetMetalDataFeature, swModelDoc, null);
                    
                    Results.Add(string.Format("\t * FORCE MODE: changing {0}'s radius to {1} {3} : {2}.", swFeature.Name, radius.ToString("F2"), FeatureResult ? "SUCCESS" : "FAILURE", BendZoneCheckedBox.Text == "KFactor" ? String.Empty : swModelDoc.LengthUnit == 0 ? "mm" : "inch"));


                }

              
                return Results.ToArray<string>();

            }
            catch (Exception exception)
            {

                return Results.ToArray<string>();

            }
        }   
        private string[] ForceChangeBendZone(ModelDoc2 swModelDoc, double BendZoneValue)
        {
            List<string> Results = new List<string>();

            try
            {
                #region sheetmetalfolder 
                // change sheet metal folder data
                FeatureManager swFeatureManager = swModelDoc.FeatureManager;
                var swFeatures = swFeatureManager.GetFeatures(false);
                var swSheetMetalFolder = (SheetMetalFolder)swFeatureManager.GetSheetMetalFolder();
                if (!(swSheetMetalFolder == null))
                {
                    Feature swSheetMetalFolderFeature = swSheetMetalFolder.GetFeature();         
                    SheetMetalFeatureData swSheetMetalFeatureDataFromFolder = swSheetMetalFolderFeature.GetDefinition();
                    CustomBendAllowance swCustomBendAllowance = swSheetMetalFeatureDataFromFolder.GetCustomBendAllowance();

                    switch (BendZoneCheckedBox.Text)
                    {
                       
                        default:
                            break;
                        case "KFactor":
                            {
                                swCustomBendAllowance.Type = (int)swBendAllowanceTypes_e.swBendAllowanceKFactor;
                                swCustomBendAllowance.KFactor = BendZoneValue;
                                break;
                            }
                        case "Bend Allowance":
                            {
                                swCustomBendAllowance.Type = (int)swBendAllowanceTypes_e.swBendAllowanceDirect;
                                // Unit checking

                                if (UnitSystemCombBox.Text == "IPS")
                                    BendZoneValue = MeasuringUnitsConvertor.Length.FromInchToMillimeters(BendZoneValue) / 1000.0;
                                else
                                    BendZoneValue = BendZoneValue / 1000.0;

                                swCustomBendAllowance.BendAllowance = BendZoneValue;
                                BendZoneValue = BendZoneValue * 1000.0;
                                break;

                            }

                        case "Bend Deduction":
                            {
                                swCustomBendAllowance.Type = (int)swBendAllowanceTypes_e.swBendAllowanceDeduction;
                                // Unit checking
                                if (UnitSystemCombBox.Text == "IPS")
                                    BendZoneValue = MeasuringUnitsConvertor.Length.FromInchToMillimeters(BendZoneValue) / 1000.0;
                                else
                                    BendZoneValue = BendZoneValue / 1000.0;


                                swCustomBendAllowance.BendDeduction = BendZoneValue  ;
                                BendZoneValue = BendZoneValue * 1000.0;
                                break;
                            }
                   }
                         

                            swSheetMetalFeatureDataFromFolder.SetCustomBendAllowance(swCustomBendAllowance);
                    bool FeatureResult = swSheetMetalFolderFeature.ModifyDefinition((object)swSheetMetalFeatureDataFromFolder, swModelDoc, null);
                    Results.Add(string.Format("\t * FORCE MODE: changing {0}'s bend zone value to {1} {3} : {2}.", swSheetMetalFolderFeature.Name, BendZoneCheckedBox.Text == "KFactor" ? BendZoneValue.ToString("F2") : (swModelDoc.LengthUnit == (int)swLengthUnit_e.swINCHES ? MeasuringUnitsConvertor.Length.FromMillimetersToInchs(BendZoneValue) : BendZoneValue).ToString("F2"), FeatureResult ? "SUCCESS" : "FAILURE", BendZoneCheckedBox.Text == "KFactor" ? String.Empty : swModelDoc.LengthUnit == 0 ? "mm" : "inch"));

                }

                #endregion
                #region sheet metal features 
                // Change kfactor of features from feature manager

                foreach (Feature swFeature in swFeatures)
                {
                    dynamic swSheetMetalDataFeature = swSheetMetalFeatureTypeWithKFactorProperty.Exists(x => x == swFeature.GetTypeName()) ? swFeature.GetDefinition() : null;
                    if (swSheetMetalDataFeature == null)
                        continue;
                    // missing overrride for bend tables  
                    CustomBendAllowance swCustomBendAllowance = swSheetMetalDataFeature.GetCustomBendAllowance();
                    switch (BendZoneCheckedBox.Text)
                    {

                        default:
                            break;
                        case "KFactor":
                            {
                                swCustomBendAllowance.Type = (int)swBendAllowanceTypes_e.swBendAllowanceKFactor;
                                swCustomBendAllowance.KFactor = BendZoneValue;
                                break;
                            }
                        case "Bend Allowance":
                            {
                                swCustomBendAllowance.Type = (int)swBendAllowanceTypes_e.swBendAllowanceDirect;
                                // Unit checking
                                if (UnitSystemCombBox.Text == "IPS")
                                    BendZoneValue = MeasuringUnitsConvertor.Length.FromInchToMillimeters(BendZoneValue) / 1000.0;
                                else
                                    BendZoneValue = BendZoneValue / 1000.0;

                                swCustomBendAllowance.BendAllowance = BendZoneValue ;
                                BendZoneValue = BendZoneValue * 1000.0;
                                break;

                            }

                        case "Bend Deduction":
                            {
                                swCustomBendAllowance.Type = (int)swBendAllowanceTypes_e.swBendAllowanceDeduction;
                                // Unit checking
                                if (UnitSystemCombBox.Text == "IPS")
                                    BendZoneValue = MeasuringUnitsConvertor.Length.FromInchToMillimeters(BendZoneValue) / 1000.0;
                                else
                                    BendZoneValue = BendZoneValue / 1000.0;

                                swCustomBendAllowance.BendDeduction = BendZoneValue;
                                BendZoneValue = BendZoneValue * 1000.0;
                                break;
                            }
                    }
                    swSheetMetalDataFeature.SetCustomBendAllowance(swCustomBendAllowance);
                    bool FeatureResult = swFeature.ModifyDefinition((object)swSheetMetalDataFeature, swModelDoc, null);
                    Results.Add(string.Format("\t * FORCE MODE: changing {0}'s bend zone to {1} {3} : {2}.", swFeature.Name, BendZoneCheckedBox.Text == "KFactor" ? BendZoneValue.ToString("F2") : (swModelDoc.LengthUnit == (int)swLengthUnit_e.swINCHES ? MeasuringUnitsConvertor.Length.FromMillimetersToInchs(BendZoneValue) : BendZoneValue).ToString("F2"), FeatureResult ? "SUCCESS" : "FAILURE", BendZoneCheckedBox.Text == "KFactor" ? String.Empty : swModelDoc.LengthUnit == 0 ? "mm" : "inch"));


                    #endregion
            }



                return Results.ToArray<string>();

            }
            catch (Exception exception)
            {
                Results.Add(exception.ToString());
                return Results.ToArray<string>();

            }

        }
        private string[] ForceChangeThickness(ModelDoc2 swModelDoc, double Thickness)

        {
           

            List<string> Results = new List<string>();


            try
            {


                // Check unit system and convert radius to corresponding value
                int LengthUnit = swModelDoc.LengthUnit;
                switch (LengthUnit)
                {
                    case (int)swLengthUnit_e.swMM:
                        {
                            if (UnitSystemCombBox.Text == "MMGS")
                            {
                                

                            }

                            else
                            {
                                Thickness = MeasuringUnitsConvertor.Length.FromInchToMillimeters(Thickness);
                            }


                            break;
                        }
                    case (int)swLengthUnit_e.swINCHES:
                        if (UnitSystemCombBox.Text == "IPS")
                        {

                          

                        }
                        else
                        {

                            Thickness = MeasuringUnitsConvertor.Length.FromMillimetersToInchs(Thickness);


                        }
                        break;
                    default:
                        break;
                }
                 // missing overrride for bend tables 
                FeatureManager swFeatureManager = swModelDoc.FeatureManager;
                var swFeatures = swFeatureManager.GetFeatures(false);
                foreach (Feature swFeature in swFeatures)
                {
                    dynamic swSheetMetalDataFeature = swSheetMetalFeatureTypeWithThicknessProperty.Exists(x => x == swFeature.GetTypeName()) ? swFeature.GetDefinition() : null;
                    if (swSheetMetalDataFeature == null)
                        continue;
                    swSheetMetalDataFeature.Thickness = Thickness;
                    bool FeatureResult = swFeature.ModifyDefinition((object)swSheetMetalDataFeature, swModelDoc, null);
                    Results.Add(string.Format("\t * FORCE MODE: changing {0}'s thickness to {1} {3} : {2} .", swFeature.Name, Thickness.ToString("F2"), FeatureResult ? "SUCCESS" : "FAILURE", BendZoneCheckedBox.Text == "KFactor" ? String.Empty : swModelDoc.LengthUnit == 0 ? "mm" : "inch"));


                }


                return Results.ToArray<string>();

            }
            catch (Exception exception)
            {

                return Results.ToArray<string>();

            }
        }
        
        // normal mode: all changes must be done from the sheet metal folder and not the features 
         

        private string[] ChangeRadius(ModelDoc2 swModelDoc, double radius)
        {

            

            List<string> Results = new List<string>();


            try
            {

                #region units
                // Check unit system and convert radius to corresponding value
                int LengthUnit = (int)swModelDoc.LengthUnit;
               

                switch (LengthUnit)
                {
                    case (int)swLengthUnit_e.swMM:
                        {
                            if (UnitSystemCombBox.Text == "MMGS")
                            {
                                //radius = radius / 1000.0;
                               
                            }

                            else
                            {
                                radius = MeasuringUnitsConvertor.Length.FromInchToMillimeters(radius)  ;
                            }


                            break;
                        }
                    case (int)swLengthUnit_e.swINCHES:
                        if (UnitSystemCombBox.Text == "IPS")
                        {

                            //radius = radius / 1000.0;

                        }
                        else
                        {

                            radius = MeasuringUnitsConvertor.Length.FromMillimetersToInchs(radius);


                        }
                        break;
                    default:
                        break;
                }
                #endregion

                     

                FeatureManager swFeatureManager = swModelDoc.FeatureManager;
                var swFeatures = swFeatureManager.GetFeatures(false);
                var swSheetMetalFolder = (SheetMetalFolder)swFeatureManager.GetSheetMetalFolder();
            
                // change sheet metal folder data
                var swSheetMetalFolder2 = new SheetMetalFolder2(swSheetMetalFolder);

                bool FeatureResult = swSheetMetalFolder2.BendRadius(radius);
            
                if (FeatureResult)
                    swModelDoc.ForceRebuild3(true);

                Results.Add(string.Format("\t *NORMAL MODE: changing {0}'s radius to {1} {3} : {2} .", swSheetMetalFolder.GetFeature().Name, radius.ToString("F2"), FeatureResult ? "SUCCESS" : "FAILURE", BendZoneCheckedBox.Text == "KFactor" ? String.Empty : swModelDoc.LengthUnit == 0 ? "mm" : "inch"));


                 


                return Results.ToArray<string>();

            }
            catch (Exception exception)
            {

                return Results.ToArray<string>();

            }
        }
       
        private string[] ChangeBendZone(ModelDoc2 swModelDoc, double BendZoneValue)
        {
            List<string> Results = new List<string>();

            try
            {

                FeatureManager swFeatureManager = swModelDoc.FeatureManager;
                var swFeatures = swFeatureManager.GetFeatures(false);
                var swSheetMetalFolder = (SheetMetalFolder)swFeatureManager.GetSheetMetalFolder();


                if (!(swSheetMetalFolder == null))
                {
                    Feature swSheetMetalFolderFeature = swSheetMetalFolder.GetFeature();
                    SheetMetalFeatureData swSheetMetalFeatureDataFromFolder = swSheetMetalFolderFeature.GetDefinition();
                    CustomBendAllowance swCustomBendAllowance = swSheetMetalFeatureDataFromFolder.GetCustomBendAllowance();

                    switch (BendZoneCheckedBox.Text)
                    {
                        #region units
                     
                        case "KFactor":
                            {
                                swCustomBendAllowance.Type = (int)swBendAllowanceTypes_e.swBendAllowanceKFactor;
                                swCustomBendAllowance.KFactor = BendZoneValue;
                                break;
                            }
                        case "Bend Allowance":
                            {
                                swCustomBendAllowance.Type = (int)swBendAllowanceTypes_e.swBendAllowanceDirect;
                                // Unit checking

                                if (UnitSystemCombBox.Text == "IPS")
                                    BendZoneValue = MeasuringUnitsConvertor.Length.FromInchToMillimeters(BendZoneValue) / 1000.0;
                                else
                                    BendZoneValue = BendZoneValue / 1000.0;

                                swCustomBendAllowance.BendAllowance = BendZoneValue;
                                BendZoneValue = BendZoneValue * 1000.0;
                                break;

                            }

                        case "Bend Deduction":
                            {
                                swCustomBendAllowance.Type = (int)swBendAllowanceTypes_e.swBendAllowanceDeduction;
                                // Unit checking

                                if (UnitSystemCombBox.Text == "IPS")
                                    BendZoneValue = MeasuringUnitsConvertor.Length.FromInchToMillimeters(BendZoneValue) / 1000.0;
                                else
                                    BendZoneValue = BendZoneValue / 1000.0;


                                swCustomBendAllowance.BendDeduction = BendZoneValue;
                                BendZoneValue = BendZoneValue * 1000.0;
                                break;
                            }

                        #endregion
                        default:
                            break;
                    }
                    swSheetMetalFeatureDataFromFolder.SetCustomBendAllowance(swCustomBendAllowance);
                    bool FeatureResult = swSheetMetalFolderFeature.ModifyDefinition((object)swSheetMetalFeatureDataFromFolder, swModelDoc, null);
                    Results.Add(string.Format("\t * NORMAL MODE: Changing {0}'s bend zone to {1} {3} : {2}.", swSheetMetalFolderFeature.Name, BendZoneCheckedBox.Text == "KFactor" ? BendZoneValue.ToString("F2") : (swModelDoc.LengthUnit == (int)swLengthUnit_e.swINCHES ? MeasuringUnitsConvertor.Length.FromMillimetersToInchs(BendZoneValue):BendZoneValue).ToString("F2"), FeatureResult ? "SUCCESS" : "FAILURE", BendZoneCheckedBox.Text == "KFactor" ? String.Empty : swModelDoc.LengthUnit == 0 ? "mm" : "inch"));

                }

                return Results.ToArray<string>();

            }
            catch (Exception exception)
            {
                Results.Add(exception.ToString());
                return Results.ToArray<string>();

            }
        }
      
        private string[] ChangeThickness(ModelDoc2 swModelDoc, double Thickness)
        {
 
            List<string> Results = new List<string>();
 
            try
            {
 
                #region units
                // Check unit system and convert radius to corresponding value
                int LengthUnit = swModelDoc.LengthUnit;
                switch (LengthUnit)
                {
                    case (int)swLengthUnit_e.swMM:
                        {
                            if (UnitSystemCombBox.Text == "MMGS")
                            {


                            }

                            else
                            {
                                Thickness = MeasuringUnitsConvertor.Length.FromInchToMillimeters(Thickness);
                            }

                            break;
                        }
                    case (int)swLengthUnit_e.swINCHES:
                        if (UnitSystemCombBox.Text == "IPS")
                        {



                        }
                        else
                        {

                            Thickness = MeasuringUnitsConvertor.Length.FromMillimetersToInchs(Thickness);


                        }
                        break;
                    default:
                        break;
                }
                #endregion

                

                FeatureManager swFeatureManager = swModelDoc.FeatureManager;
               
                var swSheetMetalFolder = (SheetMetalFolder)swFeatureManager.GetSheetMetalFolder();
                var swSheetMetalFolder2 = new SheetMetalFolder2(swSheetMetalFolder);

                bool FeatureResult = swSheetMetalFolder2.Thickness(Thickness);
                if (FeatureResult)
                    swModelDoc.ForceRebuild3(true);

                Results.Add(string.Format("\t * NORMAL MODE: changing {0}'s thickness to {1} {3} : {2} ", swSheetMetalFolder.GetFeature().Name, Thickness.ToString("F2"), FeatureResult ? "SUCCESS" : "FAILURE",swModelDoc.LengthUnit == 0 ? "mm": "inch"));
                return Results.ToArray<string>();
            }

            catch (Exception exception)
            {
                return Results.ToArray<string>();
            }
        }



        #endregion
 

        private void helpToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("https://github.com/jliliamen/power-sm/wiki/PowerGeometry");
        }

        private void FilesTreeView_AfterSelect(object sender, TreeViewEventArgs e)
        {

        }

        private void RecentToolStripMenu_Click(object sender, EventArgs e)
        {
            var RecentDirectory = RecentToolStripMenu.Text;
            if (System.IO.Directory.Exists(RecentDirectory))
            {
                CurrentDirectory = RecentDirectory;
                ListDirectory(FilesTreeView, CurrentDirectory);

                //Get all tree nodes from treeview
                AllTreeViewNodes = new List<CustomTreeNode>();
                GetAllNodesFromTreeView(FilesTreeView, AllTreeViewNodes);

                // Select all nodes
                selectAllToolStripMenuItem.PerformClick();

            }
        }

        private void PowerGeometryForm_Activated(object sender, EventArgs e)
        {
            // Load recent directory
            RecentToolStripMenu.Text = Convert.ToString(System.Configuration.ConfigurationSettings.AppSettings["RecentDirectory"]);
        }
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
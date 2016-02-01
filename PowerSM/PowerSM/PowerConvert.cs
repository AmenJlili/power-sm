using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using SolidWorks.Interop.sldworks;
using SolidWorks.Interop.swconst;
using System.IO;
using FeatureWorks;
namespace PowerSM
{
    public partial class PowerConvert : Form
    {
        FeatureWorks.FeatureWorksApp swFeatureWorks;
        string CurrentDirectory;
        string OutputDirectory;
        SldWorks swApp;
        List<CustomTreeNode> AllTreeViewNodes;

        public PowerConvert(SldWorks swApp_)
        {
            InitializeComponent();
            swApp = swApp_;
        }



        private async void StartButton_Click(object sender, EventArgs e)
        {
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


            // load featureworks 
            string strExecutablePath = swApp.GetExecutablePath();
            string strDllFileName = strExecutablePath + @"\fworks\fworks.dll";
            long lStatus = swApp.LoadAddIn(strDllFileName);
            if (lStatus != 0)
            {
                switch (lStatus)

                {
                    case -1:
                        MessageBox.Show(strDllFileName + " not loaded; unknown error.");
                        return;
                    case 1:
                        MessageBox.Show(strDllFileName + "not loaded; not an error.");
                        return;
                    case 2:
                        MessageBox.Show(strDllFileName + "not loaded; already loaded." +
                        "Unload the add -in in SOLIDWORKS and then rerun the add-in.");
                        return;
                    default:
                        break;
                }

                return;
            }
            else {

                // startfeatureworks 
              
                swFeatureWorks = (FeatureWorks.FeatureWorksApp)swApp.GetAddInObject("FeatureWorks.FeatureWorksApp");
                if (swFeatureWorks == null)
                {
                    MessageBox.Show("Unable to start feature recognition.");
                    return;
                }
            }

            // sleeves roll-up:


            foreach (CustomTreeNode element in SelectedTreeNodes)
            {

                Task<string[]> ConvertToSheetMetalPartAsyncTask = ConvertToSheetMetalPartAsync(element);
                string[] result = await ConvertToSheetMetalPartAsyncTask;
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

                    System.IO.Compression.ZipFile.CreateFromDirectory(@"C:\Temp\PowerSM", String.Format(@"{0}\powersm.zip", OutputDirectory));

                }

               );
                task.Wait();
            }

            toolStripStatusLabel.Text = "Ready";

        }


        private void BrowseForOutPutFolder_Click(object sender, EventArgs e)
        {
            OutputFolder();
        }

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

        private async Task<string[]> ConvertToSheetMetalPartAsync(CustomTreeNode customtreenode)
        {

            return await Task.Run(() => ConvertToSheetMetalPart(customtreenode));

        }

        private string[] ConvertToSheetMetalPart(CustomTreeNode customtreenode)
        {
            List<string> Results = new List<string>();
            ModelDoc2 swModelDoc;

            try
            {
                int Error = 0;
                int Warning = 0;
                Results.Add(string.Format("[{0}] PART FILENAME: {1}", DateTime.Now.ToString(), customtreenode.FullFilePath));

                swApp.DocumentVisible(false, (int)swDocumentTypes_e.swDocPART);
                bool Result = swApp.LoadFile(customtreenode.FullFilePath);
                swModelDoc = swApp.ActiveDoc;
                // All errors and warnings would throw a an error
              


               
                
                // Select base flange for fixed faces

                PartDoc swPart;
                swPart = (PartDoc)swApp.ActiveDoc;
                var swSheetMetalBodiesObj = (object[])swPart.GetBodies2((int)swBodyType_e.swSolidBody, true);
                swModelDoc.ClearSelection2(true);
                SelectionMgr swSelectionManager = swModelDoc.SelectionManager;
                foreach (object swBodyObj in swSheetMetalBodiesObj)
                {
                    Body2 swBody = (Body2)swBodyObj;
                    dynamic[] swFacesObj = (object[])swBody.GetFaces();
                    List<Face2> swFaces = new List<Face2>();
                    foreach (object swFaceObj in swFacesObj)
                    {
                        Face2 swFace = (Face2)swFaceObj;
                        swFaces.Add(swFace);
                    };

                    Face2 swLargestFace = swFaces.OrderByDescending<Face2, double>(x => x.GetArea()).First<Face2>();

                    swSelectionManager.AddSelectionListObject(swLargestFace, null);
                }
                // start featureworks
                int Options =
                      (int)FeatureWorks.fwAutomaticRecognitionOptions_e.fwAutoEdgeFlange
                    + (int)FeatureWorks.fwAutomaticRecognitionOptions_e.fwBaseFlange
                    + (int)FeatureWorks.fwAutomaticRecognitionOptions_e.fwSketchedBend
                    + (int)FeatureWorks.fwAutomaticRecognitionOptions_e.fwAutoHemFlange
                    + (int)FeatureWorks.fwAutomaticRecognitionOptions_e.fwVolume;
                
                
                // Recongize feature
                swFeatureWorks.RecognizeFeatureAutomatic(Options);
                bool result = swFeatureWorks.CreateFeatures((int)FeatureWorks.fwFeatureCreationOptions_e.fwAllowFailFeatureCreation);
                Results.Add(string.Format("\n\t * recognizing features: {0}", result.ToString()));
                // save


                if (Convert.ToBoolean(System.Configuration.ConfigurationSettings.AppSettings["ArchiveInZipFormat"]) && !String.IsNullOrEmpty(OutputDirectory))
                {
                    string TempFolder = @"C:\Temp\PowerSM";
                    string newfilename = string.Format(@"{0}\{1}", TempFolder, customtreenode.FullPath);
                    // change extension
                    newfilename = System.IO.Path.ChangeExtension(newfilename, "sldprt");
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
                    // change extension
                    newfilename = System.IO.Path.ChangeExtension(newfilename, "sldprt");
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

               
                return Results.ToArray<string>();

            }
            catch (Exception exception)
            {
                var ErrorString = string.Format("[{0}] - ERROR: {1}", DateTime.Now.ToString(), exception.ToString());
                Results.Add(ErrorString);
                Results.Add(System.Environment.NewLine);
                swApp.DocumentVisible(true, (int)swDocumentTypes_e.swDocPART);
                swApp.CloseDoc(string.Empty);
                return Results.ToArray<string>();
            }



        }

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
            FileExtensions.AddRange(new[] { "STEP", "Step", "IGES","iges","STP", "stp" });

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


        private void OpenFolder_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(CurrentDirectory))
                System.Diagnostics.Process.Start("explorer.exe", CurrentDirectory);
        }
        
        private void optionsToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            var powersmoptions = new PowerGeometryOptions();
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

        private void PowerConvert_Load(object sender, EventArgs e)
        {
            // Load icons for tree view 
            var rm = new System.Resources.ResourceManager("PowerSM.Properties.Resources", System.Reflection.Assembly.GetExecutingAssembly());
            Bitmap part = (Bitmap)rm.GetObject("part_png");
            Bitmap folder = (Bitmap)rm.GetObject("folder_png");

            FilesTreeView.ImageList = new ImageList();

            FilesTreeView.ImageList.Images.Add(part);
            FilesTreeView.ImageList.Images.Add(folder);
        }

        private void optionsToolStripMenuItem1_Click_1(object sender, EventArgs e)
        {
            var powersmoptions = new PowerGeometryOptions();
            powersmoptions.ShowDialog();
        }

        private void aboutThisToolToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            AboutBox aboutbox = new AboutBox();
            aboutbox.ShowDialog();
        }

        private void FilesTreeGroupBox_Enter(object sender, EventArgs e)
        {

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

     
    }

    

}

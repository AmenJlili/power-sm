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
using SolidWorks.Interop.swcommands;
using SolidWorks.Interop.swconst;
using SolidWorks.Interop.swpublished;
using System.IO;
namespace PowerSM
{
    public partial class Power_SM_Form : Form
    {
        string CurrentDirectory;
        SldWorks swApp;
        List<FileNodeElement> TreeViewFiles;
        List<string> swSheetMetalFeatureTypes;
      
        public Power_SM_Form(SldWorks swApp_)
        {
            InitializeComponent();
            swApp = swApp_;
        }
        enum TreeViewIcons
        {
            directory = 1,
            file = 0
        }

        #region UI methods

        private void Power_SM_Form_Load(object sender, EventArgs e)
        {
            // Load icons for tree view 
            var rm = new System.Resources.ResourceManager("PowerSM.Properties.Resources",System.Reflection.Assembly.GetExecutingAssembly());
            Bitmap part = (Bitmap)rm.GetObject("part_png");
            Bitmap folder = (Bitmap)rm.GetObject("folder_png");

            FilesTreeView.ImageList = new ImageList();

            FilesTreeView.ImageList.Images.Add(part);
            FilesTreeView.ImageList.Images.Add(folder);
            // Add SheetMetalFeature types
            swSheetMetalFeatureTypes = new List<string>();
            swSheetMetalFeatureTypes.AddRange(new[] {"SMBaseFlange",
                                                    "EdgeFlange",
                                                    "SMBaseFlange",
                                                    "SketchedBend",
                                                    "SheetMetal",
                                                    "OneBend",
                                                    "SMMiteredFlange",
                                                    "Jog",
                                                    "Bends"});
        }

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
                CurrentDirectory = FolderBrowser.SelectedPath;
                TreeViewFiles = new List<FileNodeElement>();
                ListDirectory(FilesTreeView, CurrentDirectory);
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

            var directoryNode = new TreeNode(directoryInfo.Name,(int)TreeViewIcons.directory, (int)TreeViewIcons.directory);

            foreach (var directory in directoryInfo.GetDirectories())

                if (!directory.Attributes.HasFlag(FileAttributes.Hidden))
                    directoryNode.Nodes.Add(CreateDirectoryNode(directory));

            foreach (var file in directoryInfo.GetFiles())
            {
                if (FileExtensions.Any(x => file.Extension.Contains(x)))
                {
                    directoryNode.Nodes.Add(new TreeNode(file.Name, (int)TreeViewIcons.file, (int)TreeViewIcons.file));

                    TreeViewFiles.Add(new FileNodeElement()
                    {
                      FullName = file.FullName,
                      Name = file.Name
                    });
                }
            }
            return directoryNode;
        }
        #endregion
           
        private void StartButton_Click(object sender, EventArgs e)
        {
            
            
        }

        #endregion

        #region Change radius methods
        // Untested
        private bool ChangeRadius(string filename, double radius)
        {
            int Errors = 0;
            ModelDoc2 swModelDoc;
            try
            {
                swModelDoc = swApp.OpenDocSilent(filename, (int)swDocumentTypes_e.swDocPART, ref Errors);

                FeatureManager swFeatureManager = swModelDoc.FeatureManager;
                var swFeatures = swFeatureManager.GetFeatures(false);
                foreach (Feature swFeature in swFeatures)
                {
                    var swSheetMetalDataFeature = swSheetMetalFeatureTypes.Exists(x => x == swFeature.GetTypeName()) ? swFeature : null;
                    if (swSheetMetalDataFeature == null)
                        continue;
                    if (swSheetMetalDataFeature is SheetMetalFeatureData)
                    {
                        SheetMetalFeatureData _swSheetMetalDataFeature = (SheetMetalFeatureData)swSheetMetalDataFeature;
                        _swSheetMetalDataFeature.SetOverrideDefaultParameter(true);
                    }

                    if (swSheetMetalDataFeature is BaseFlangeFeatureData)
                    {
                        BaseFlangeFeatureData _swBaseFlangeDataFeature = (BaseFlangeFeatureData)swSheetMetalDataFeature;
                        _swBaseFlangeDataFeature.OverrideRadius = true;
                    }

                    Type swSheetMetalDataFeatureType = swSheetMetalDataFeature.GetType();
                    var swSheetMetalDataFeatureRadiusProperty = swSheetMetalDataFeatureType.GetProperty("BendRadius");
                    swSheetMetalDataFeatureRadiusProperty.SetValue(swSheetMetalDataFeature, radius);
                   
                }
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
        #endregion


        public static class TestArea
    {
        public static void Method()
        {

        }
    }
    public class FileNodeElement
    {
        public string FullName { get; set; }
        public string Name { get; set; }
    } 
}

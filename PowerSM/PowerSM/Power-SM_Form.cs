﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using SolidWorks.Interop.sldworks;
using SolidWorks.Interop.swconst;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
namespace PowerSM
{
    public partial class Power_SM_Form : Form
    {
        string CurrentDirectory;
        SldWorks swApp;
        List<FileNodeElement> TreeViewFiles;
        List<string> swSheetMetalFeatureTypes;
        delegate bool del(SldWorks swApp, string filename, double radius);
        public Power_SM_Form(SldWorks swApp_)
        {
            InitializeComponent();
            swApp = swApp_;
        }
       

        #region UI methods

        private void Power_SM_Form_Load(object sender, EventArgs e)
        {
            // Load icons for tree view 
            var rm = new System.Resources.ResourceManager("PowerSM.Properties.Resources", System.Reflection.Assembly.GetExecutingAssembly());
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

            var directoryNode = new TreeNode(directoryInfo.Name, (int)PowerSMEnums.TreeViewIcons.directory, (int)PowerSMEnums.TreeViewIcons.directory);

            foreach (var directory in directoryInfo.GetDirectories())

                if (!directory.Attributes.HasFlag(FileAttributes.Hidden))
                    directoryNode.Nodes.Add(CreateDirectoryNode(directory));

            foreach (var file in directoryInfo.GetFiles())
            {
                if (FileExtensions.Any(x => file.Extension.Contains(x)))
                {
                    directoryNode.Nodes.Add(new TreeNode(file.Name, (int)PowerSMEnums.TreeViewIcons.file, (int)PowerSMEnums.TreeViewIcons.file));

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

        private void HouseKeeping()
        {
          
        }
        private void StartButton_Click(object sender, EventArgs e)
        {  
            // Radius must be a double
            double Radius;
            if (!double.TryParse(RadiusTextBox.Text, out Radius))
            {
                ErrorEchoer.Err((int)PowerSMEnums.Errors.Cannot_parse_radius_value);
                return;
            }
            // Progressbar housekeeping
            _ProgressBar.Value = 0;
            _ProgressBar.Minimum = 0;
            var ProgressMaximum = TreeViewFiles.Count;
            _ProgressBar.Step = (int)(ProgressMaximum / TreeViewFiles.Count);
            _ProgressBar.Maximum = ProgressMaximum;

            foreach (FileNodeElement element in TreeViewFiles)
            {
          
                string filename = element.FullName;
                string[] result = ChangeRadiusAsync(filename, Radius).Result;
                _ProgressBar.PerformStep();
                LogTextBox.Text += string.Join(System.Environment.NewLine,result);
                
            }
        }

        private async Task<string[]> ChangeRadiusAsync(string filename, double radius)
        {

            return await Task.Run(() => ChangeRadius(filename, radius));

        }
        #endregion

        #region Change radius methods
        

        private string[] ChangeRadius(string filename, double radius)
        {
            int Errors = 0;
            ModelDoc2 swModelDoc;
            List<string> Results = new List<string>();
           
            try
            {
                swModelDoc = swApp.OpenDocSilent(filename, (int)swDocumentTypes_e.swDocPART, ref Errors);
                
                FeatureManager swFeatureManager = swModelDoc.FeatureManager;
                var swFeatures = swFeatureManager.GetFeatures(false);
                foreach (Feature swFeature in swFeatures)
                {
                    dynamic swSheetMetalDataFeature = swSheetMetalFeatureTypes.Exists(x => x == swFeature.GetTypeName()) ? swFeature.GetDefinition() : null;
                    if (swSheetMetalDataFeature == null)
                        continue;

                    if (swSheetMetalDataFeature is SheetMetalFeatureData)
                        swSheetMetalDataFeature.SetOverrideDefaultParameter(true);

                    if (swSheetMetalDataFeature is BaseFlangeFeatureData)
                        swSheetMetalDataFeature.OverrideRadius = true;

                    if (swSheetMetalDataFeature is EdgeFlangeFeatureData)
                        swSheetMetalDataFeature.UseDefaultBendRadius = false;


                    swSheetMetalDataFeature.BendRadius = (radius * 1.0) / 1000.0;
                    bool FeatureResult = swFeature.ModifyDefinition((object)swSheetMetalDataFeature, swModelDoc, null);
                    Results.Add(string.Format("Operation on {1} feature: {0}", FeatureResult?"Success":"Failure", swFeature.Name));


                }

                swModelDoc.Save();
                swApp.CloseDoc(string.Empty);
                Results.Add("Complete with no errors.");
                return Results.ToArray<string>();
            
            }
            catch (Exception exception)
            {
                var ErrorString = string.Format("{0}:{1}",DateTime.Today.TimeOfDay.ToString(), exception.Message);
                System.IO.File.AppendAllText(string.Format(@"{0}\{1}",System.Reflection.Assembly.GetExecutingAssembly().CodeBase, "ErrorLog.txt"),ErrorString);
                Results.Add(ErrorString);
                swApp.CloseDoc(string.Empty);
                return Results.ToArray<string>();
               
            }
        }
        #endregion


       
        public class FileNodeElement
        {
            public string FullName { get; set; }
            public string Name { get; set; }
        }

        public static class TestArea
        {

            public static string[] ChangeRadius(SldWorks swApp, string filename, double radius, List<string> swSheetMetalFeatureTypes)
            {

                int Errors = 0;
                ModelDoc2 swModelDoc;
                List<string> Results = new List<string>();

                swModelDoc = swApp.OpenDocSilent(filename, (int)swDocumentTypes_e.swDocPART, ref Errors);

                FeatureManager swFeatureManager = swModelDoc.FeatureManager;
                var swFeatures = swFeatureManager.GetFeatures(false);
                foreach (Feature swFeature in swFeatures)
                {
                    dynamic swSheetMetalDataFeature = swSheetMetalFeatureTypes.Exists(x => x == swFeature.GetTypeName()) ? swFeature.GetDefinition() : null;
                    if (swSheetMetalDataFeature == null)
                        continue;

                    if (swSheetMetalDataFeature is SheetMetalFeatureData)
                        swSheetMetalDataFeature.SetOverrideDefaultParameter(true);

                    if (swSheetMetalDataFeature is BaseFlangeFeatureData)
                        swSheetMetalDataFeature.OverrideRadius = true;

                    if (swSheetMetalDataFeature is EdgeFlangeFeatureData)
                        swSheetMetalDataFeature.UseDefaultBendRadius = false;


                    swSheetMetalDataFeature.BendRadius = radius / 1000.0;
                    bool FeatureResult = swFeature.ModifyDefinition((object)swSheetMetalDataFeature, swModelDoc, null);
                    Results.Add(string.Format("{0}@{1}", FeatureResult, swFeature.Name));


                }

                swModelDoc.Save();
                swModelDoc.Close();
                return Results.ToArray();
            }



        }
    }
}

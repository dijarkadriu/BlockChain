﻿using BlockChainCore;
using BlockChainCore.Helpers;
using BlockChainCore.Models.BlockChain;
using BlockChainCore.Models.File;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;


namespace BlockChainUI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            
            InitializeComponent();
            
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            using (var fbd = new FolderBrowserDialog())
            {
                DialogResult result = fbd.ShowDialog();

                if (!string.IsNullOrWhiteSpace(fbd.SelectedPath))
                {
                    selectedFolderText.Text = fbd.SelectedPath;
                    GlobalVariables.FolderToWatch = fbd.SelectedPath;
                    List<FileModel> fileModels = new List<FileModel>();
                    fileModels = Functions.PopulateFilesList();
                    Blockchain chain = Functions.PopulateBlockchain();
                    fileList.ItemsSource = chain.Chain;
                    Functions.Watch(Directory.GetLastWriteTime(GlobalVariables.FolderToWatch),fileModels,chain);
                }
            }
        }
    }
}

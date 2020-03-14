using BlockChainCore.Helpers;
using BlockChainCore.Models.BlockChain;
using System;
using System.Diagnostics;
using System.Drawing;
using System.Threading;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Media;

namespace BlockChainUI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Thread thread;
        Blockchain chain = null;



        private Block selectedBlock;
        public MainWindow()
        {
            InitializeComponent();
        }
        private async void Bw_DoWork()
        {
            try
            {
                Functions functions = new Functions();
                await functions.Watch(chain);

            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK);
            }

        }
        private void Track_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                using (var fbd = new FolderBrowserDialog())
                {
                    DialogResult result = fbd.ShowDialog();

                    if (!string.IsNullOrWhiteSpace(fbd.SelectedPath))
                    {
                        selectedFolderText.Text = fbd.SelectedPath;
                        GlobalVariables.FolderToWatch = fbd.SelectedPath;
                        selectedFolderText.Foreground = new SolidColorBrush(Colors.Green);
                    }
                }
            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK);
            }
        }

        private void Copy_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                using (var fbd = new FolderBrowserDialog())
                {
                    DialogResult result = fbd.ShowDialog();

                    if (!string.IsNullOrWhiteSpace(fbd.SelectedPath))
                    {
                        copiedFolder.Text = fbd.SelectedPath;
                        GlobalVariables.CopiedFilePath = @fbd.SelectedPath + "\\";
                        copiedFolder.Foreground = new SolidColorBrush(Colors.Green);
                    }
                }
            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK);
            }

        }

        private void StartTrack_Click(object sender, RoutedEventArgs e)
        {
            try
            {

                if (GlobalVariables.CopiedFilePath != "" && GlobalVariables.FolderToWatch != "")
                {
                    btnTrack.Visibility = Visibility.Hidden;
                    chain = Blockchain.PopulateBlockchain();
                    chain.Chain.RemoveAt(0);
                    fileList.ItemsSource = chain.Chain;
                    thread = new Thread(new ThreadStart(Bw_DoWork));
                    thread.Start();
                }
                else
                {
                    System.Windows.Forms.MessageBox.Show("Please Select The Recommended Folders", "Error", MessageBoxButtons.OK);
                }

            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK);
            }


        }

        private void fileList_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            var d = sender as ListView;
            selectedBlock = e.AddedItems[0] as Block;
        }

        private void Open_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var p = new Process();
                p.StartInfo = new ProcessStartInfo(selectedBlock.FullPath)
                {
                    UseShellExecute = true
                };
                p.Start();
            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK);
            }
        }
    }
}

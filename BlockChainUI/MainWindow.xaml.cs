using BlockChainCore.Helpers;
using BlockChainCore.Models.BlockChain;
using System.Threading;
using System.Windows;
using System.Windows.Forms;


namespace BlockChainUI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Blockchain chain = null;
        public MainWindow()
        {

            InitializeComponent();

        }
        private async void bw_DoWork()
        {
            await Functions.Watch(chain);
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

                    chain = Blockchain.PopulateBlockchain();
                    fileList.ItemsSource = chain.Chain;

                    new Thread(bw_DoWork).Start();


                }
            }
        }
    }
}

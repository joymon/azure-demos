using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApplication1
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            
            DownloadInParallel(1);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            
            DownloadInParallel(2);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            DownloadInParallel(Convert.ToInt32(NumberOfDownloadsNumericUpDown.Value));
        }

        private void DownloadInParallel(int numberOfThreads)
        {
            Parallel.For(0, numberOfThreads, (index) =>
            {
                Context.AzCopyPath = AzCopyPathTextBox.Text;
                Task.Run(() =>
                {
                    DownloadBlob(index);
                });
            });
        }

        private static void DownloadBlob(int destinationFolder)
        {
            AzCopyDownloaderFomBlob acopy = new AzCopyDownloaderFomBlob(Context.StorageAccountFullName);
            acopy.DoDownload($"https://{Context.StorageAccountFullName}.blob.core.windows.net/{Context.FolderToDownloadFrom}",
                $@"c:\temp\{destinationFolder}", "*",
                Context.AccountKey);
        }

        private void GenerateAzCopyButton_Click(object sender, EventArgs e)
        {
            AzCopyCommandTextBox.Text = GetAzureAzCopyCommand(AnalysisIdTextBox.Text);
        }
        private const string azureStorageConnectionstring = "DefaultEndpointsProtocol=https;AccountName=$AccountName$;AccountKey=$AccountKey$";

        public string GetAzureAzCopyCommand(string ContainerName)
        {
            var accountDetails = new { StorageAccountFullName = Context.StorageAccountFullName, AccessKey = Context.AccountKey };
            if (accountDetails != null)
            {
                BlobService _blobService = new BlobService();
                SASToken _sasToken = new SASToken();
                AzCopyCommand _azCopyCommand = new AzCopyCommand();
                var connectionstring = azureStorageConnectionstring.Replace("$AccountName$", accountDetails.StorageAccountFullName).Replace("$AccountKey$", accountDetails.AccessKey);
                ContainerName = ContainerName.Replace("_", "-").ToLower();
                var checkContainer = _blobService.CheckContainerExists(connectionstring, ContainerName);
                if (!checkContainer)
                    _blobService.CreateContainer(connectionstring, ContainerName);

                var createSASToken = _sasToken.GetContainerSasUri(connectionstring, ContainerName, null);
                var createCommand = _azCopyCommand.Generate(SourceFolderTextBox.Text, createSASToken.Url, createSASToken.SASToken);
                return createCommand;
            }
            return "Account not found";
        }

        private void BrowseFolderButton_Click(object sender, EventArgs e)
        {
            SourceFolderBrowserDialog.ShowDialog();
            SourceFolderTextBox.Text = SourceFolderBrowserDialog.SelectedPath;
        }
    }
}

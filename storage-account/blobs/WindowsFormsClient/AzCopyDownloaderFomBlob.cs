using System;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Windows.Forms;

namespace WindowsFormsApplication1
{
    public class AzCopyDownloaderFomBlob
    {
        private string _storageAccount;
        private string _azcopyCommand = "AzCopy.exe";
        private string aZCopy = " /Source:$Source$ /Dest:$Destination$ /SourceKey:$SourceKey$ /s /y /v:$LogPath$ /Z:$Journalfolder$";
        public AzCopyDownloaderFomBlob(string storageAccount)
        {
            _storageAccount = storageAccount;
        }

        private string GenerateAzCopyCommand(string source, string destination, string filePattern, string sourceKey)
        {
            StringBuilder azcopy = new StringBuilder(aZCopy);
            azcopy.Replace("$Source$", source);
            azcopy.Replace("$Destination$", destination);
            azcopy.Replace("$SourceKey$", sourceKey);
            azcopy.Replace("$Pattern$", filePattern);
            azcopy.Replace("$LogPath$", Path.Combine(destination,"log.txt"));
            azcopy.Replace("$Journalfolder$", Path.Combine(destination, "azcopy.jnl"));
            return azcopy.ToString();
        }

        public void DoDownload(string source, string destination, string filePattern, string sourceKey)
        {
            string command = GenerateAzCopyCommand(source, destination, filePattern, sourceKey);
            Console.WriteLine(command);
            ProcessStartInfo startInfo = new ProcessStartInfo(Path.Combine(Context.AzCopyPath, _azcopyCommand), command);
            startInfo.WorkingDirectory = @"C:\Program Files (x86)\Microsoft SDKs\Azure\AzCopy";
            startInfo.WindowStyle = ProcessWindowStyle.Hidden;
            using (Process azCopy = new Process())
            {
                startInfo.CreateNoWindow = true;
                startInfo.RedirectStandardOutput = true;
                startInfo.UseShellExecute = false;
                azCopy.StartInfo = startInfo;
                azCopy.OutputDataReceived += (sender, e) =>
                {
                    System.IO.File.AppendAllText(System.IO.Path.Combine(destination, "tempdata.txt"), $"From Event {e.Data}");
                }

                ;
                azCopy.Start();
                azCopy.WaitForExit();
                System.IO.File.AppendAllText(System.IO.Path.Combine(destination, "tempdata.txt"), "Completed");
                System.IO.File.AppendAllText(System.IO.Path.Combine(destination, "tempdata.txt"), azCopy.StandardOutput.ReadToEnd());
            }
        }

        private void AzCopy_Exited(object sender, EventArgs e)
        {
            // System.IO.File.AppendAllText(@"d:\tempdata.txt", "Completed");
            MessageBox.Show("Completed");
        }
    }
}
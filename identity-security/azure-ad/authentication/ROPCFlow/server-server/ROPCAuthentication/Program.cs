using EasyConsole;
using Microsoft.Graph;
using PnP.Framework;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ROPCAuthentication
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var easyConsoleMenu = new Menu()
                .Add("GraphAPI - Display Root Site (just to check authentication)", async (token) => await DisplayRootSiteInformationUsingGraphAPI())
                
                .Add("GraphAPI - Display Root site x2 (confirm JWT token caching)", async (token) => await DisplayTootSite2Times())
                .Add("GraphAPI - Download file", async (token) => await DownloadFileUsingGraphAPI())
                .Add("PnPFramework - List Root Site (just to check authentication)", async (token) => await DisplayRootSiteUsingPnPFramework())
                .Add("PnPFramework - Download file", async (token) => await DownloadFileUsingPnPFramework())

                .Add("Exit", async (token) => await Task.Delay(0));

            await easyConsoleMenu.Display(CancellationToken.None);

            Input.ReadString("Completed. Press any key to exit...");
        }

        #region PnP Library
        private async static Task DisplayRootSiteUsingPnPFramework()
        {
            await SharePointManagerFactory.Get(SharePointInteractionType.PnPFramework).ListRootSite();
        }
        async private static Task DownloadFileUsingPnPFramework()
        {
            SPOFile spoObj = new SPOFile
            {
                SiteId = Configurations.SiteId,
                LibraryId = Configurations.LibraryId,
                FileId = Configurations.FileId
            };

            await SharePointManagerFactory.Get(SharePointInteractionType.PnPFramework).DownloadFile(spoObj, Configurations.BasePathToDownloadFile);
        }
        #endregion

        #region Graph API
        private static async Task DisplayRootSiteInformationUsingGraphAPI()
        {
            await SharePointManagerFactory.Get(SharePointInteractionType.GraphAPI).ListRootSite();
        }
        async private static Task DisplayTootSite2Times()
        {
            Output.WriteLine($"Attempt 1 - Start");
            await SharePointManagerFactory.Get(SharePointInteractionType.GraphAPI).ListRootSite();
            Output.WriteLine($"Attempt 1 - End");

            Output.WriteLine($"Attempt 2 - Start");
            await SharePointManagerFactory.Get(SharePointInteractionType.GraphAPI).ListRootSite();
            Output.WriteLine($"Attempt 2 ");
        }

        private static async Task DownloadFileUsingGraphAPI()
        {
            Output.WriteLine($"{nameof(DownloadFileUsingGraphAPI)} - Start");
            try
            {
                SPOFile spoObj = new SPOFile
                {
                    SiteId = Configurations.SiteId,
                    LibraryId = Configurations.LibraryId,
                    FileId = Configurations.FileId
                };
                var file = await SharePointManagerFactory.Get(SharePointInteractionType.GraphAPI).GetFileAsync(spoObj);
                if (file == null)
                {
                    Output.WriteLine(ConsoleColor.Red, $"File with id {spoObj.FileId} does not exists.");
                }
                else
                {
                    Output.WriteLine($"Found FileId {file.Id}");
                    Output.WriteLine($"Found File Name {file.Name}");
                    Output.WriteLine($"Found File Size {file.Size}");
                    Output.WriteLine($"Found File DownloadUrl {file.AdditionalData?["@microsoft.graph.downloadUrl"]?.ToString()}");

                    
                    await SharePointManagerFactory.Get(SharePointInteractionType.GraphAPI).DownloadFile(spoObj,Configurations.BasePathToDownloadFile);
                    Output.WriteLine("Download completed");
                   }
            }
            catch (Exception ex)
            {
                Output.WriteLine(ConsoleColor.Red, ex.Message);
            }
        }
        #endregion
    }
}

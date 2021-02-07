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
                .Add("GraphAPI - List Root Site (just to check authentication)", async (token) => await ListRootSiteUsingGraphAPI())
                .Add("GraphAPI - List sites x2 (confirm JWT token caching)", async (token) => await ListSites2Times())
                .Add("GraphAPI - Download file", async (token) => await DownloadFileUsingGraphAPI())
                .Add("PnPFramework - List Root Site (just to check authentication)", async (token) => await ListRootSiteUsingPnPFramework())
                .Add("PnPFramework - Download file", async (token) => await DownloadFileUsingPnPFramework())

                .Add("Exit", async (token) => await Task.Delay(0));

            await easyConsoleMenu.Display(CancellationToken.None);

            Input.ReadString("Completed...");
        }

        #region PnP Library
        private async static Task ListRootSiteUsingPnPFramework()
        {
            await SharePointManagerFactory.Get(SharePointInteractionType.PnPFramework).ListRootSite();
        }
        async private static Task DownloadFileUsingPnPFramework()
        {
            Spo spoObj = new Spo
            {
                siteId = Configurations.SiteId,
                LibraryId = Configurations.LibraryId,
                FileId = Configurations.FileId
            };
            await SharePointManagerFactory.Get(SharePointInteractionType.PnPFramework).DownloadFile(spoObj);
        }
        #endregion

        #region Graph API
        private static async Task ListRootSiteUsingGraphAPI()
        {
            await SharePointManagerFactory.Get(SharePointInteractionType.GraphAPI).ListRootSite();
        }
        async private static Task ListSites2Times()
        {
            Console.WriteLine($"Attemp 1 - Start");
            await SharePointManagerFactory.Get(SharePointInteractionType.GraphAPI).ListRootSite();
            Console.WriteLine($"Attemp 1 - End");

            Console.WriteLine($"Attemp 2 - Start");
            await SharePointManagerFactory.Get(SharePointInteractionType.GraphAPI).ListRootSite();
            Console.WriteLine($"Attemp 2 ");
        }

        private static async Task DownloadFileUsingGraphAPI()
        {
            Console.WriteLine("Acquiring file Details ...");
            try
            {
                Spo spoObj = new Spo
                {
                    siteId = Configurations.SiteId,
                    LibraryId = Configurations.LibraryId,
                    FileId = Configurations.FileId
                };
                var file = await SharePointManagerFactory.Get(SharePointInteractionType.GraphAPI).GetFileAsync(spoObj);
                if (file == null)
                {
                    return;
                }
                else
                {
                    await SharePointManagerFactory.Get(SharePointInteractionType.GraphAPI).DownloadFile(spoObj);
                }
                Console.WriteLine($"Found FileId {file.Id}");
                Console.WriteLine($"Found File Name {file.Name}");
                Console.WriteLine($"Found File Size {file.Size}");
                Console.WriteLine($"Found File DownloadUrl {file.AdditionalData?["@microsoft.graph.downloadUrl"]?.ToString()}");
            }
            catch (Exception ex)
            {
                Output.WriteLine(ConsoleColor.Red, ex.Message);
            }
        }
        #endregion
    }
}

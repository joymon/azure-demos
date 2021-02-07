using EasyConsole;
using Microsoft.Graph;
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
                .Add("List Root Site (just to check authentication)", async (token) => await ListRootSite())
                .Add("List sites x2 (confirm JWT token caching)", async (token) => await ListSites2Times())
                .Add("Download file", async (token) => await GetFileForDownload())
                .Add("Exit", async (token) => Console.WriteLine("Exiting"));
            
            await easyConsoleMenu.Display(CancellationToken.None);
            
            Input.ReadString("Completed...");
        }
        private static async Task ListRootSite()
        {
            string accessToken = await GetAccessToken();
            GraphServiceClient graphClient = GetGraphServiceClient(accessToken);

            var site = await GraphHelper.GetRootSite(graphClient);
            Output.WriteLine(ConsoleColor.Green, $@"Root site information: 
                                    Id:{site.Id},
                                    Display Name:{site.DisplayName},
                                    WebUrl:{site.WebUrl}");
        }

        async private static Task ListSites2Times()
        {
            string accessToken = await GetAccessToken();
            GraphServiceClient graphClient = GetGraphServiceClient(accessToken);

            var site = await GraphHelper.GetRootSite(graphClient);
            Console.WriteLine($"Attemp 1 = Root site information: Id:{site.Id},WebUrl:{site.WebUrl}");

            string accessToken1 = await GetAccessToken();
            GraphServiceClient graphClient1 = GetGraphServiceClient(accessToken1);

            var site1 = await GraphHelper.GetRootSite(graphClient1);
            Console.WriteLine($"Attemp 2 = Root site information: Id:{site1.Id},WebUrl:{site1.WebUrl}");
        }
        private static async Task<string> GetAccessToken()
        {
            string KeyVaultURI = string.Empty;
            IAuthenticationManager authenticationManager = AuthenticationManagerFactory.Get();
            return await authenticationManager.GetAccessTokenAsync(new Uri("https://graph.microsoft.com/"), KeyVaultURI);
        }

        private static async Task GetFileForDownload()
        {
            string accessToken = await GetAccessToken();
            GraphServiceClient graphClient = GetGraphServiceClient(accessToken);

            await GetFileforDownload(graphClient);
        }

        private static GraphServiceClient GetGraphServiceClient(string accessToken)
        {
            return new GraphServiceClient(new DelegateAuthenticationProvider((requestMessage) =>
            {
                requestMessage.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", accessToken);
                return Task.FromResult(0);
            }));
        }

        private static async Task GetFileforDownload(GraphServiceClient graphClient)
        {
            Console.WriteLine("Acquiring file Details ...");
            try
            {
                Spo spoObj = new Spo
                {
                    siteId = ConfigurationManager.AppSettings["SiteId"],
                    LibraryId = ConfigurationManager.AppSettings["LibraryId"],
                    FileId = ConfigurationManager.AppSettings["FileId"]
                };
                var file = await GraphHelper.GetFileAsync(spoObj, graphClient);
                if (file == null)
                {
                    return;
                }
                else
                {
                    await GraphHelper.DownloadFile(file);
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
    }
}

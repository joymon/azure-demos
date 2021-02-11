using EasyConsole;
using Microsoft.Graph;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace ROPCAuthentication
{
    public class GraphAPIBasedSharePointManager : ISharePointManager
    {
        #region ISharePointManager implementation
        async Task ISharePointManager.ListRootSite()
        {
            GraphServiceClient graphClient = await GetGraphServiceClient();

            var request = graphClient.Sites.Root.Request();
            var site = await request.GetAsync();
            Output.WriteLine(ConsoleColor.Green, $@"Root site information: 
                                    Id:{site.Id},
                                    Display Name:{site.DisplayName},
                                    WebUrl:{site.WebUrl}");

        }
        async Task<DriveItem> ISharePointManager.GetFileAsync(Spo spo)
        {
            Output.WriteLine($"{nameof(GraphAPIBasedSharePointManager)}.{nameof(ISharePointManager.GetFileAsync)} - Start");
            DriveItem file = await GetFileFromSpo(spo);
            if (file == null)
            {
                Output.WriteLine($"File does not exists");
            }
            return file;
        }
       
        /// <summary>
        /// Download file
        /// </summary>
        /// <param name="spo"></param>
        /// <returns></returns>
        async Task ISharePointManager.DownloadFile(Spo spo)
        {
            await DownloadUsingNativeGraphWay(spo);
            //await DownloadUsingHttpRequest(spo);
        }

        async private Task DownloadUsingNativeGraphWay(Spo spo)
        {
            GraphServiceClient graphClient = await GetGraphServiceClient();
            var file = await GetFileFromSpo(spo);
            var inputFileStream = await graphClient.Sites[spo.siteId].Drives[spo.LibraryId].Items[spo.FileId].Content.Request().GetAsync();
            using (FileStream fileStream = System.IO.File.Create(Path.Combine(ConfigurationManager.AppSettings["DownloadBasePath"], file.Name)))
            {
                inputFileStream.CopyTo(fileStream);
            }
        }
        private static async Task DownloadUsingHttpRequest(Spo spo)
        {
            DriveItem file = await GetFileFromSpo(spo);
            const long DefaultChunkSize = 2000 * 1024; // 50 KB, TODO: change chunk size to make it realistic for a large file.
            long ChunkSize = DefaultChunkSize;
            long offset = 0;         // cursor location for updating the Range header.
            byte[] bytesInStream;    // bytes in range returned by chunk download.

            // Let's download the first file we get in the response.
            if (file.File != null)
            {
                // We'll use the file metadata to determine size and the name of the downloaded file
                // and to get the download URL.

                // Get the download URL. This URL is preauthenticated and has a short TTL.
                object downloadUrl;

                file.AdditionalData.TryGetValue("@microsoft.graph.downloadUrl", out downloadUrl);

                // Get the number of bytes to download. calculate the number of chunks and determine
                // the last chunk size.
                long size = (long)file.Size;
                int numberOfChunks = Convert.ToInt32(size / DefaultChunkSize);
                // We are incrementing the offset cursor after writing the response stream to a file after each chunk. 
                // Subtracting one since the size is 1 based, and the range is 0 base. 
                int lastChunkSize = Convert.ToInt32(size % DefaultChunkSize) - numberOfChunks - 1;
                if (lastChunkSize > 0) { numberOfChunks++; }

                // Create a file stream to contain the downloaded file.
                using (FileStream fileStream = System.IO.File.Create(Path.Combine(ConfigurationManager.AppSettings["DownloadBasePath"], file.Name)))
                {
                    for (int i = 0; i < numberOfChunks; i++)
                    {
                        // Setup the last chunk to request. This will be called at the end of this loop.
                        if (i == numberOfChunks - 1)
                        {
                            ChunkSize = lastChunkSize;
                        }

                        // Create the request message with the download URL and Range header.
                        HttpRequestMessage req = new HttpRequestMessage(HttpMethod.Get, (string)downloadUrl);
                        req.Headers.Range = new System.Net.Http.Headers.RangeHeaderValue(offset, ChunkSize + offset);

                        var client = new HttpClient();
                        HttpResponseMessage response = await client.SendAsync(req);

                        using (Stream responseStream = await response.Content.ReadAsStreamAsync())
                        {
                            bytesInStream = new byte[ChunkSize];
                            int read;
                            do
                            {
                                read = responseStream.Read(bytesInStream, 0, bytesInStream.Length);
                                if (read > 0)
                                    fileStream.Write(bytesInStream, 0, read);
                            }
                            while (read > 0);
                        }
                        offset += ChunkSize + 1; // Move the offset cursor to the next chunk.
                    }
                }
            }
        }
        #endregion

        private static async Task<DriveItem> GetFileFromSpo(Spo spo)
        {
            GraphServiceClient graphClient = await GetGraphServiceClient();
            //This will be getting from the Response Message
            //This will be getting from the Response Message
            var site = await graphClient.Sites[spo.siteId].Request().GetAsync();
            var file = await graphClient.Sites[spo.siteId].Drives[spo.LibraryId].Items[spo.FileId].Request().GetAsync();
            return file;
        }

        private static async Task<string> GetAccessToken()
        {
            string KeyVaultURI = string.Empty;
            IAuthenticationManager authenticationManager = AuthenticationManagerFactory.Get();
            return await authenticationManager.GetAccessTokenAsync(new Uri("https://graph.microsoft.com/"), KeyVaultURI);
        }
        async private static Task<GraphServiceClient> GetGraphServiceClient()
        {
            string accessToken = await GetAccessToken();
            return new GraphServiceClient(new DelegateAuthenticationProvider((requestMessage) =>
            {
                requestMessage.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", accessToken);
                return Task.FromResult(0);
            }));
        }

    }
}
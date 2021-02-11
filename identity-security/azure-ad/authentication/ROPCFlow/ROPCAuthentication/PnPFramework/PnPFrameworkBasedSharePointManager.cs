using EasyConsole;
using Microsoft.Graph;
using Microsoft.SharePoint.Client;
using PnP.Framework;
using System;
using System.Configuration;
using System.IO;
using System.Threading.Tasks;

namespace ROPCAuthentication
{
    internal class PnPFrameworkBasedSharePointManager : ISharePointManager
    {
        #region ISharePointManager implementation
        /// <summary>
        /// 
        /// </summary>
        /// <param name="spo">Unfortunately this is not required in this implementation. A human readable url is enough.</param>
        /// <retu</returns>
        async Task ISharePointManager.DownloadFile(Spo spo)
        {

            var authManager = new AuthenticationManager(Configurations.AADAppregistrationId,
                                             username: Configurations.ServiceAccountName,
                                             password: Configurations.ServiceAccountSecurePassword);

            using (var context = await authManager.GetContextAsync(Configurations.SubSiteURL))
            {
                var file = context.Web.GetFileByUrl(Configurations.HumanReadableAbosolutePathToFile);
                context.Load(file);
                context.ExecuteQuery();
                ClientResult<Stream> streamFromSPS = file.OpenBinaryStream();
                context.ExecuteQueryRetry();

                var fileOut = Path.Combine(ConfigurationManager.AppSettings["DownloadBasePath"], file.Name);
                if (System.IO.File.Exists(fileOut))
                {
                    Output.WriteLine($"Skipped downloading as file already exists at {fileOut}");
                }
                else
                {
                    using (Stream fileStream = new FileStream(fileOut, FileMode.Create))
                    {
                        using (streamFromSPS.Value)
                        {
                            streamFromSPS.Value.CopyTo(fileStream);
                        }
                    }
                    Output.WriteLine($"Downloaded to {fileOut}");
                }
            }
        }

        Task<DriveItem> ISharePointManager.GetFileAsync(Spo spo)
        {
            throw new System.NotImplementedException();
        }
        async Task ISharePointManager.ListRootSite()
        {
            var authManager = new AuthenticationManager(Configurations.AADAppregistrationId,
                                              username: Configurations.ServiceAccountName,
                                              password: Configurations.ServiceAccountSecurePassword);

            using (var context = await authManager.GetContextAsync(Configurations.RootSiteUrl))
            {
                context.Load<Web>(context.Web, web => web.Title);
                context.ExecuteQuery();
                Output.WriteLine(ConsoleColor.Green, $"Site information: {context.Web.Title}");
            }

        }
        #endregion
    }
}
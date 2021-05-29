using Microsoft.Graph;
using System.Threading.Tasks;

namespace ROPCAuthentication
{
    public interface ISharePointManager
    {
        Task DownloadFile(SPOFile spo, string filePathToDownload);
        Task<DriveItem> GetFileAsync(SPOFile spo);
        Task ListRootSite();
    }
}
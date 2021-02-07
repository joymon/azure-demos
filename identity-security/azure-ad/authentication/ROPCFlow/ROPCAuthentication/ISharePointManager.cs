using Microsoft.Graph;
using System.Threading.Tasks;

namespace ROPCAuthentication
{
    public interface ISharePointManager
    {
        Task DownloadFile(Spo spo);
        Task<DriveItem> GetFileAsync(Spo spo);
        Task ListRootSite();
    }
}
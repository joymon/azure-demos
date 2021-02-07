using System;
using System.Threading.Tasks;

namespace ROPCAuthentication
{
    public interface IAuthenticationManager
    {
        Task<string> GetAccessTokenAsync(Uri resourceUri, string KeyVaultURI);
    }
}
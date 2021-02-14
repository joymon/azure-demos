using EasyConsole;
using Microsoft.Identity.Client;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Security;
using System.Threading.Tasks;

namespace ROPCAuthentication
{
    public class MSALBasedAuthenticationManager : IAuthenticationManager
    {
        static Lazy<IPublicClientApplication> lazyApp = new Lazy<IPublicClientApplication>(() =>
        PublicClientApplicationBuilder
                .Create(Configurations.AADAppregistrationId)
                .WithTenantId(Configurations.AADTenantId)
                .Build());

        /// <summary>
        /// Get token by user name and password
        /// </summary>
        /// <returns>AccessToken</returns>
        /// <remarks>The caching seems working only for the AcquireTokenSilent()
        /// The AcquireTokensilent() will also renew token using refresh token silently as per below link.
        /// https://docs.microsoft.com/en-us/azure/active-directory/develop/msal-error-handling-dotnet#msaluirequiredexception
        /// </remarks>
        async Task<string> IAuthenticationManager.GetAccessTokenAsync(Uri resourceUri, string KeyVaultURI)
        {
            AuthenticationResult result = null;
            try
            {
                result = await lazyApp.Value.AcquireTokenSilent(getScopes(), Configurations.ServiceAccountName).ExecuteAsync();
                Console.WriteLine($"{nameof(MSALBasedAuthenticationManager)} - Obtained token from cache");
            }
            catch (MsalUiRequiredException)
            {
                //As per MSFT attept to read silently from cache. On exception try the actual method
                //https://docs.microsoft.com/en-us/azure/active-directory/develop/msal-net-acquire-token-silently

                result = await lazyApp.Value.AcquireTokenByUsernamePassword(getScopes(),
                    Configurations.ServiceAccountName,
                    Configurations.ServiceAccountSecurePassword).ExecuteAsync();
                Output.WriteLine(ConsoleColor.Yellow, $"{nameof(MSALBasedAuthenticationManager)} - Token not in cache. Obtained new.");
            }
            var token = new System.IdentityModel.Tokens.Jwt.JwtSecurityToken(result.AccessToken);
            Console.WriteLine($@"{nameof(MSALBasedAuthenticationManager)} - JWT ValidTo {token.ValidTo}");
            return result.AccessToken;
        }

        private static List<string> getScopes()
        {
            string ResourceId = "https://graph.microsoft.com/";
            return new List<string>() { ResourceId + "/.default" };
        }
    }
}
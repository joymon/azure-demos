using Microsoft.IdentityModel.Clients.ActiveDirectory;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ROPCAuthentication
{
    public class AuthenticationManager : IAuthenticationManager
    {
        // Token cache handling
        private static readonly SemaphoreSlim semaphoreSlimTokens = new SemaphoreSlim(1);
        private AutoResetEvent tokenResetEvent = null;
        private readonly ConcurrentDictionary<string, string> tokenCache = new ConcurrentDictionary<string, string>();
        internal class TokenWaitInfo
        {
            public RegisteredWaitHandle Handle = null;
        }

        private static string TokenFromCache(Uri web, ConcurrentDictionary<string, string> tokenCache)
        {
            if (tokenCache.TryGetValue(web.DnsSafeHost, out string accessToken))
            {
                return accessToken;
            }

            return null;
        }

        private static void AddTokenToCache(Uri web, ConcurrentDictionary<string, string> tokenCache, string newAccessToken)
        {
            if (tokenCache.TryGetValue(web.DnsSafeHost, out string currentAccessToken))
            {
                tokenCache.TryUpdate(web.DnsSafeHost, newAccessToken, currentAccessToken);
            }
            else
            {
                tokenCache.TryAdd(web.DnsSafeHost, newAccessToken);
            }
        }

        private static void RemoveTokenFromCache(Uri web, ConcurrentDictionary<string, string> tokenCache)
        {
            tokenCache.TryRemove(web.DnsSafeHost, out string currentAccessToken);
        }

        private static TimeSpan CalculateThreadSleep(string accessToken)
        {
            var token = new System.IdentityModel.Tokens.Jwt.JwtSecurityToken(accessToken);
            var lease = GetAccessTokenLease(token.ValidTo);
            lease = TimeSpan.FromSeconds(lease.TotalSeconds - TimeSpan.FromMinutes(5).TotalSeconds > 0 ? lease.TotalSeconds - TimeSpan.FromMinutes(5).TotalSeconds : lease.TotalSeconds);
            return lease;
        }

        private static TimeSpan GetAccessTokenLease(DateTime expiresOn)
        {
            DateTime now = DateTime.UtcNow;
            DateTime expires = expiresOn.Kind == DateTimeKind.Utc ? expiresOn : TimeZoneInfo.ConvertTimeToUtc(expiresOn);
            TimeSpan lease = expires - now;
            return lease;
        }

        //public void Dispose()
        //{
        //    // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
        //    Dispose(disposing: true);
        //    GC.SuppressFinalize(this);
        //}


        //protected virtual void Dispose(bool disposing)
        //{
        //    if (!disposedValue)
        //    {
        //        if (disposing)
        //        {
        //            if (tokenResetEvent != null)
        //            {
        //                tokenResetEvent.Set();
        //                tokenResetEvent.Dispose();
        //            }
        //        }

        //        disposedValue = true;
        //    }
        //}

        public async Task<string> GetAccessTokenAsync(Uri resourceUri, string KeyVaultURI)
        {
            string serviceAccountName = ConfigurationManager.AppSettings["serviceAccountName"];
            string serviceAccountPasswordClearText = ConfigurationManager.AppSettings["serviceAccountPasswordClearText"];
            string AADTenantId = ConfigurationManager.AppSettings["AADTenantId"];
            string AADAppregistrationId = ConfigurationManager.AppSettings["AADAppregistrationId"];
            SecureString tenantAdminPassword = CommonUtils.GetSecureString(serviceAccountPasswordClearText);

            string accessTokenFromCache = TokenFromCache(resourceUri, tokenCache);
            if (accessTokenFromCache == null)
            {
                await semaphoreSlimTokens.WaitAsync().ConfigureAwait(false);
                try
                {

                    Task<AuthenticationResult> taskTokenGenerate = GetAccessTokenForFederatedAccount(resourceUri.ToString(), serviceAccountName, tenantAdminPassword, AADTenantId, AADAppregistrationId);
                    taskTokenGenerate.Wait();
                    var result = taskTokenGenerate.Result;
                    string accessToken = result.AccessToken;
                    AddTokenToCache(resourceUri, tokenCache, accessToken);

                    // Register a thread to invalidate the access token once's it's expired
                    tokenResetEvent = new AutoResetEvent(false);
                    TokenWaitInfo wi = new TokenWaitInfo();
                    wi.Handle = ThreadPool.RegisterWaitForSingleObject(
                        tokenResetEvent,
                        async (state, timedOut) =>
                        {
                            if (!timedOut)
                            {
                                wi = (TokenWaitInfo)state;
                                if (wi.Handle != null)
                                {
                                    wi.Handle.Unregister(null);
                                }
                            }
                            else
                            {
                                try
                                {
                                    // Take a lock to ensure no other threads are updating the SharePoint Access token at this time
                                    await semaphoreSlimTokens.WaitAsync().ConfigureAwait(false);
                                    RemoveTokenFromCache(resourceUri, tokenCache);
                                    Console.WriteLine($"Cached token for resource {resourceUri.DnsSafeHost} and user {serviceAccountName} expired");
                                }
                                catch (Exception ex)
                                {
                                    Console.WriteLine($"Something went wrong during cache token invalidation: {ex.Message}");
                                    RemoveTokenFromCache(resourceUri, tokenCache);
                                }
                                finally
                                {
                                    semaphoreSlimTokens.Release();
                                }
                            }
                        },
                        wi,
                        (uint)CalculateThreadSleep(accessToken).TotalMilliseconds,
                        true
                    );

                    return accessToken;
                }
                finally
                {
                    semaphoreSlimTokens.Release();
                }
            }
            else
            {
                return accessTokenFromCache;
            }
        }

        private async Task<AuthenticationResult> GetAccessTokenForFederatedAccount(string ResourceAppIdUri, string username, SecureString userPassword, string AADTenantId, string AADAppregistrationId)
        {
            string authority = "https://login.microsoftonline.com/" + AADTenantId;
            var authContext = new Microsoft.IdentityModel.Clients.ActiveDirectory.AuthenticationContext(authority);
            UserPasswordCredential cred = new UserPasswordCredential(username, userPassword);
            var authResult = await authContext.AcquireTokenAsync(ResourceAppIdUri, AADAppregistrationId, cred);
            return authResult;
        }

        

    }
}

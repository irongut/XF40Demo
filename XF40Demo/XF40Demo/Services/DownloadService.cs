using XF40Demo.Helpers;
using MonkeyCache.FileStore;
using System;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;
using Xamarin.Essentials;

namespace XF40Demo.Services
{
    public sealed class DownloadService
    {
        private static readonly DownloadService instance = new DownloadService();

        private readonly SettingsService settings = SettingsService.Instance();

        private HttpClient client;

        private DownloadService()
        {
            client = new HttpClient();
            client.DefaultRequestHeaders.Add("X-Requested-With", "XF40Demo");
            client.DefaultRequestHeaders.AcceptEncoding.Add(new StringWithQualityHeaderValue("gzip"));
            client.DefaultRequestHeaders.AcceptEncoding.Add(new StringWithQualityHeaderValue("deflate"));
            client.Timeout = TimeSpan.FromSeconds(40);
        }

        public static DownloadService Instance()
        {
            return instance;
        }

        public async Task<(string data, DateTime updated)> GetData(string url, string dataKey, string lastUpdatedKey, TimeSpan expiry, bool ignoreCache = false)
        {
            string data = String.Empty;
            DateTime lastUpdated;

            if (Connectivity.NetworkAccess != NetworkAccess.Internet || (settings.WifiOnly && !Connectivity.ConnectionProfiles.Contains(ConnectionProfile.WiFi)))
            {
                // no valid connectivity
                if (Barrel.Current.Exists(dataKey))
                {
                    // use cached data
                    data = Barrel.Current.Get<string>(dataKey);
                    lastUpdated = DateTime.Parse(Barrel.Current.Get<string>(lastUpdatedKey));
                }
                else
                {
                    string msg = "No Internet available and no data cached.";
                    if (settings.WifiOnly && !Connectivity.ConnectionProfiles.Contains(ConnectionProfile.WiFi))
                    {
                        msg += " Turn off the WiFi only setting to use mobile data.";
                    }
                    throw new NoNetworkNoCacheException(msg);
                }
            }
            else if (!ignoreCache && Barrel.Current.Exists(dataKey) && !Barrel.Current.IsExpired(dataKey))
            {
                // use cached data
                data = Barrel.Current.Get<string>(dataKey);
                lastUpdated = DateTime.Parse(Barrel.Current.Get<string>(lastUpdatedKey));
            }
            else
            {
                // download data
                var uri = new Uri(url);
                HttpResponseMessage response = await client.GetAsync(uri).ConfigureAwait(false);
                if (!response.IsSuccessStatusCode)
                {
                    throw new APIException(String.Format("{0} - {1}", response.StatusCode, response.ReasonPhrase), (int)response.StatusCode);
                }
                else
                {
                    data = await HttpHelper.ReadContentAsync(response).ConfigureAwait(false);
                    lastUpdated = DateTime.Now;
                    // cache data
                    Barrel.Current.Add(dataKey, data, expiry);
                    Barrel.Current.Add(lastUpdatedKey, lastUpdated.ToString(), expiry);
                }
            }
            return (data, lastUpdated);
        }

        public async Task<(string data, DateTime updated)> GetData(string url, string dataKey, string lastUpdatedKey, TimeSpan expiry, CancellationTokenSource cancelToken, bool ignoreCache = false)
        {
            string data = String.Empty;
            DateTime lastUpdated;

            if (Connectivity.NetworkAccess != NetworkAccess.Internet || (settings.WifiOnly && !Connectivity.ConnectionProfiles.Contains(ConnectionProfile.WiFi)))
            {
                // no valid connectivity
                if (Barrel.Current.Exists(dataKey))
                {
                    // use cached data
                    data = Barrel.Current.Get<string>(dataKey);
                    lastUpdated = DateTime.Parse(Barrel.Current.Get<string>(lastUpdatedKey));
                }
                else
                {
                    string msg = "No Internet available and no data cached.";
                    if (settings.WifiOnly && !Connectivity.ConnectionProfiles.Contains(ConnectionProfile.WiFi))
                    {
                        msg += " Turn off the WiFi only setting to use mobile data.";
                    }
                    throw new NoNetworkNoCacheException(msg);
                }
            }
            else if (!ignoreCache && Barrel.Current.Exists(dataKey) && !Barrel.Current.IsExpired(dataKey))
            {
                // use cached data
                data = Barrel.Current.Get<string>(dataKey);
                lastUpdated = DateTime.Parse(Barrel.Current.Get<string>(lastUpdatedKey));
            }
            else
            {
                // download data
                var uri = new Uri(url);
                HttpResponseMessage response = await client.GetAsync(uri, cancelToken.Token).ConfigureAwait(false);
                if (!response.IsSuccessStatusCode)
                {
                    throw new APIException(String.Format("{0} - {1}", response.StatusCode, response.ReasonPhrase), (int)response.StatusCode);
                }
                else
                {
                    data = await HttpHelper.ReadContentAsync(response).ConfigureAwait(false);
                    lastUpdated = DateTime.Now;
                    // cache data
                    Barrel.Current.Add(dataKey, data, expiry);
                    Barrel.Current.Add(lastUpdatedKey, lastUpdated.ToString(), expiry);
                }
            }
            return (data, lastUpdated);
        }
    }
}

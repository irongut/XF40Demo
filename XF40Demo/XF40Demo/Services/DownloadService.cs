using MonkeyCache.FileStore;
using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Xamarin.Essentials;

namespace XF40Demo.Services
{
    public class DownloadService
    {
        private readonly SettingsService settings = new SettingsService();

        public async Task<(string data, DateTime updated)> GetData(string url, string dataKey, string lastUpdatedKey, TimeSpan expiry, bool ignoreCache = false)
        {
            string data = String.Empty;
            DateTime lastUpdated = DateTime.Now;

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
                    throw new Exception("No valid connection to Internet available and no cached data present.");
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
                var request = (HttpWebRequest)WebRequest.Create(uri);
                using (WebResponse response = await Task<WebResponse>.Factory.FromAsync(request.BeginGetResponse, request.EndGetResponse, request).ConfigureAwait(false))
                {
                    using (var responseStream = response.GetResponseStream())
                    {
                        using (StreamReader reader = new StreamReader(responseStream))
                        {
                            data = await reader.ReadToEndAsync().ConfigureAwait(false);
                        }
                    }
                }
                lastUpdated = DateTime.Now;
                // cache it
                Barrel.Current.Add(dataKey, data, expiry);
                Barrel.Current.Add(lastUpdatedKey, lastUpdated.ToString(), expiry);
            }
            return (data, lastUpdated);
        }
    }
}

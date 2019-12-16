using System;
using System.Threading;
using System.Threading.Tasks;

namespace XF40Demo.Services
{
    public class GalNetService
    {
        private const string GalNetURL = "https://elitedangerous-website-backend-production.elitedangerous.com/api/galnet?_format=json";
        private const string dataKey = "NewsFeed";
        private const string lastUpdatedKey = "NewsLastUpdated";

        private readonly SettingsService settings = SettingsService.Instance();

        public async Task<(string data, DateTime updated)> GetData(CancellationTokenSource cancelToken, bool ignoreCache = false)
        {
            DateTime lastUpdated;
            TimeSpan expiry = TimeSpan.FromHours(settings.NewsCacheTime);

            // download the json feed
            DownloadService downloadService = DownloadService.Instance();
            string json;
            (json, lastUpdated) = await downloadService.GetData(GalNetURL, dataKey, lastUpdatedKey, expiry, cancelToken, ignoreCache).ConfigureAwait(false);
            return (json, lastUpdated);
        }
    }
}

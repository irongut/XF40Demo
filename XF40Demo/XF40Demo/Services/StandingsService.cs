using MonkeyCache.FileStore;
using Newtonsoft.Json;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using XF40Demo.Models;

namespace XF40Demo.Services
{
    public sealed class StandingsService
    {
        private static readonly StandingsService instance = new StandingsService();

        private const string URL = "https://api.taranissoftware.com/elite-dangerous/galactic-standings.json";
        private const string dataKey = "Standings";
        private const string lastUpdatedKey = "StandingsUpdated";

        private GalacticStandings galacticStandings;
        private DateTime lastUpdated;

        private StandingsService()
        {
            lastUpdated = DateTime.MinValue;
        }

        public static StandingsService Instance()
        {
            return instance;
        }

        public async Task<GalacticStandings> GetData(CancellationTokenSource cancelToken, bool ignoreCache = false)
        {
            TimeSpan expiry = TimeSpan.FromMinutes(15);
            if ((galacticStandings == null) || (galacticStandings.Cycle != CycleService.CurrentCycle() && (lastUpdated + expiry < DateTime.Now)))
            {
                // download the standings
                string json;
                DownloadService downloadService = DownloadService.Instance();
                (json, lastUpdated) = await downloadService.GetData(URL, dataKey, lastUpdatedKey, expiry, cancelToken, ignoreCache).ConfigureAwait(false);

                // parse the standings
                galacticStandings = JsonConvert.DeserializeObject<GalacticStandings>(json);

                // cache till next cycle if updated
                if (galacticStandings.Cycle == CycleService.CurrentCycle())
                {
                    expiry = CycleService.TimeTillTick();
                    Barrel.Current.Add(dataKey, json, expiry);
                    Barrel.Current.Add(lastUpdatedKey, lastUpdated.ToString(), expiry);
                }
            }
            return galacticStandings;
        }

        public async Task<PowerStanding> GetPowerAsync(string shortName, CancellationTokenSource cancelToken, bool ignoreCache = false)
        {
            if (galacticStandings.Standings?.Any() == false)
            {
                _ = await GetData(cancelToken, ignoreCache).ConfigureAwait(false);
            }
            return galacticStandings.Standings.Find(x => x.ShortName.Equals(shortName));
        }
    }
}

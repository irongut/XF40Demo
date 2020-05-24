using CsvHelper;
using CsvHelper.Configuration;
using XF40Demo.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Threading;
using System.Globalization;
using MonkeyCache.FileStore;

namespace XF40Demo.Services
{
    public sealed class StandingsService
    {
        private static readonly StandingsService instance = new StandingsService();

        private const string URL = "https://docs.google.com/spreadsheets/d/e/2PACX-1vQNi9sSQ2ziF44YgUJ4La_NK4ARJhrNZCoemGOktbHfWOw-Q2qj7Oot8MM-9qURRUSGZDcru7Is0XvH/pub?&output=csv";
        private const string dataKey = "Standings";
        private const string lastUpdatedKey = "StandingsUpdated";

        private List<PowerStanding> powerStandings;
        private int cycle;
        private DateTime lastUpdated;

        private StandingsService()
        {
            lastUpdated = DateTime.MinValue;
        }

        public static StandingsService Instance()
        {
            return instance;
        }

        #region Properties

        public string DataKey
        {
            get { return dataKey; }
        }

        public string LastUpdatedKey
        {
            get { return lastUpdatedKey; }
        }

        #endregion

        public async Task<(List<PowerStanding> standings, string cycle, DateTime updated)> GetData(CancellationTokenSource cancelToken, bool ignoreCache = false)
        {
            TimeSpan expiry = TimeSpan.FromMinutes(5);
            if ((powerStandings?.Any() == false) || (cycle != CycleService.CurrentCycle() && (lastUpdated + expiry < DateTime.Now)))
            {
                // download the standings
                string csvText;
                DownloadService downloadService = DownloadService.Instance();
                (csvText, lastUpdated) = await downloadService.GetData(URL, dataKey, lastUpdatedKey, expiry, cancelToken, ignoreCache).ConfigureAwait(false);

                // parse the standings
                powerStandings = ParseStandings(csvText);
                if (!string.IsNullOrWhiteSpace(powerStandings[0].Cycle))
                {
                    int p = powerStandings[0].Cycle.IndexOf(" ") + 1;
                    Int32.TryParse(powerStandings[0].Cycle.Substring(p, powerStandings[0].Cycle.Length - p), out cycle);
                }
                else
                {
                    cycle = 0;
                }

                // cache till next cycle if updated
                if (cycle == CycleService.CurrentCycle())
                {
                    expiry = CycleService.TimeTillTick();
                    Barrel.Current.Add(dataKey, csvText, expiry);
                    Barrel.Current.Add(lastUpdatedKey, lastUpdated.ToString(), expiry);
                }
            }
            return (powerStandings, powerStandings[0].Cycle, lastUpdated);
        }

        private List<PowerStanding> ParseStandings(string csvText)
        {
            string cycleNo = string.Empty;
            List<PowerStanding> standingList = new List<PowerStanding>();
            CsvConfiguration csvConfig = new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                Delimiter = ","
            };
            using (CsvReader csv = new CsvReader(new StringReader(csvText), csvConfig))
            {
                int i = 0;
                while (csv.Read())
                {
                    if (i == 12)
                    {
                        cycleNo = csv.GetField<string>(1);
                    }
                    else if (i > 0)
                    {
                        standingList.Add(new PowerStanding(
                                            csv.GetField<int>(0),
                                            csv.GetField<string>(1),
                                            csv.GetField<int>(2),
                                            ConvertChange(csv.GetField<string>(3)),
                                            ConvertTurmoil(csv.GetField<string>(4)),
                                            csv.GetField<string>(5),
                                            csv.GetField<string>(6),
                                            lastUpdated
                                            ));
                    }
                    i++;
                }
            }
            foreach (PowerStanding item in standingList)
            {
                item.Cycle = cycleNo; // has to be set here because cycle is on the last row
            }
            return standingList.OrderBy(x => x.Position).ToList();
        }

        private StandingChange ConvertChange(string change)
        {
            change = change.Trim().ToLower();
            switch (change)
            {
                case "up":
                    return StandingChange.up;
                case "down":
                    return StandingChange.down;
                default:
                    return StandingChange.none;
            }
        }

        private bool ConvertTurmoil(string turmoil)
        {
            turmoil = turmoil.Trim().ToLower();
            return turmoil == "true";
        }
    }
}

﻿using CsvHelper;
using CsvHelper.Configuration;
using XF40Demo.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace XF40Demo.Services
{
    public class StandingsService
    {
        private const string URL = "https://docs.google.com/spreadsheets/d/e/2PACX-1vQNi9sSQ2ziF44YgUJ4La_NK4ARJhrNZCoemGOktbHfWOw-Q2qj7Oot8MM-9qURRUSGZDcru7Is0XvH/pub?&output=csv";
        private const string dataKey = "Standings";
        private const string lastUpdatedKey = "StandingsUpdated";

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

        public async Task<(List<PowerStanding> standings, string cycle, DateTime updated)> GetData(bool ignoreCache = false)
        {
            DateTime lastUpdated = DateTime.Now;
            string csvText = String.Empty;
            string cycle = String.Empty;
            TimeSpan expiry = TimeSpan.FromMinutes(5);

            // download the csv
            DownloadService downloadService = new DownloadService();
            (csvText, lastUpdated) = await downloadService.GetData(URL, dataKey, lastUpdatedKey, expiry, ignoreCache).ConfigureAwait(false);

            // parse the csv
            List<PowerStanding> standingList = new List<PowerStanding>();
            Configuration csvConfig = new Configuration();
            csvConfig.Delimiter = ",";
            using (CsvReader csv = new CsvReader(new StringReader(csvText), csvConfig))
            {
                int i = 0;
                while (csv.Read())
                {
                    if (i == 12)
                    {
                        cycle = csv.GetField<string>(1);
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
                item.Cycle = cycle; // has to be set here because cycle is on the last row
            }
            return (standingList.OrderBy(x => x.Position).ToList(), cycle, lastUpdated);
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

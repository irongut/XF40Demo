using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using XF40Demo.Models;

namespace XF40Demo.Services
{
    public sealed class PowerDetailsService
    {
        private static readonly PowerDetailsService instance = new PowerDetailsService();

        private const string URL = "https://api.taranissoftware.com/elite-dangerous/power-comms.json";
        private const string dataKey = "PowerComms";
        private const string lastUpdatedKey = "PowerCommsUpdated";

        private List<PowerDetails> powerList;
        private List<PowerComms> commsList;

        #region Properties

        public PowerDetails SelectedPower { get; internal set; }

        #endregion

        private PowerDetailsService()
        {
            powerList = new List<PowerDetails>();
            commsList = new List<PowerComms>();
        }

        public static PowerDetailsService Instance()
        {
            return instance;
        }

        public PowerDetails GetPowerDetails(string shortName)
        {
            if (powerList?.Any() == false)
            {
                GetPowerList();
            }
            return powerList.Find(x => x.ShortName.Equals(shortName));
        }

        public void SetSelectedPower(string shortName)
        {
            if (powerList?.Any() == false)
            {
                GetPowerList();
            }
            SelectedPower = powerList.Find(x => x.ShortName.Equals(shortName));
        }

        public async Task<PowerComms> GetPowerCommsAsync(string shortName, int cacheDays)
        {
            if (commsList?.Any() == false)
            {
                await GetPowerCommsListAsync(cacheDays).ConfigureAwait(false);
            }
            return commsList.Find(x => x.ShortName.Equals(shortName));
        }

        private void GetPowerList()
        {
            const string fileName = "XF40Demo.Resources.PowerDetails.json";

            Assembly assembly = GetType().GetTypeInfo().Assembly;
            using (Stream stream = assembly.GetManifestResourceStream(fileName))
            {
                using (StreamReader reader = new StreamReader(stream))
                {
                    JsonSerializer serializer = new JsonSerializer();
                    powerList = (List<PowerDetails>)serializer.Deserialize(reader, typeof(List<PowerDetails>));
                }
            }
        }

        private async Task GetPowerCommsListAsync(int cacheDays)
        {
            string json;
            if (cacheDays < 1)
            {
                cacheDays = 1;
            }
            TimeSpan expiry = TimeSpan.FromDays(cacheDays);
            DownloadService downloadService = DownloadService.Instance();
            (json, _) = await downloadService.GetData(URL, dataKey, lastUpdatedKey, expiry).ConfigureAwait(false);
            commsList = JsonConvert.DeserializeObject<List<PowerComms>>(json);
        }
    }
}
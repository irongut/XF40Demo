using MonkeyCache.FileStore;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using XF40Demo.Helpers;
using XF40Demo.Models;

namespace XF40Demo.Services
{
    public sealed class MarsWeatherService
    {
        private static readonly MarsWeatherService instance = new MarsWeatherService();

        private const string marsWeatherUrl = "https://api.nasa.gov/insight_weather/?api_key=DEMO_KEY&feedtype=json&ver=1.0";
        private const string dataKey = "MarsWeather";
        private const string lastUpdatedKey = "MarsWeatherLastUpdated";

        private readonly List<MartianDay> weather;

        private DateTime lastUpdated;

        private MarsWeatherService()
        {
            weather = new List<MartianDay>();
        }

        public static MarsWeatherService Instance()
        {
            return instance;
        }

        public async Task<(List<MartianDay> weather, DateTime updated)> GetDataAsync(CancellationTokenSource cancelToken, bool ignoreCache = false)
        {
            if (weather.Count < 1 || ignoreCache || Barrel.Current.IsExpired(dataKey))
            {
                TimeSpan expiry = TimeSpan.FromHours(1);
                DownloadService downloadService = DownloadService.Instance();
                string json;
                (json, lastUpdated) = await downloadService.GetData(marsWeatherUrl, dataKey, lastUpdatedKey, expiry, cancelToken, ignoreCache).ConfigureAwait(false);

                Dictionary<string, object> weatherData = JsonConvert.DeserializeObject<Dictionary<string, object>>(json);
                if (weatherData.TryGetValue("sol_keys", out object solKeys))
                {
                    weather.Clear();
                    foreach (JToken sol in (JArray)solKeys)
                    {
                        // get the day
                        string solName = sol.Value<string>();
                        if (weatherData.TryGetValue(solName, out object solJson))
                        {
                            Dictionary<string, object> solData = JsonConvert.DeserializeObject<Dictionary<string, object>>(solJson.ToString());

                            // get first / last UTC + season
                            DateTime firstUTC = solData.TryGetValue("First_UTC", out object fUTC) ? ParseTimestamp(fUTC.ToString()) : DateTime.MinValue;
                            DateTime lastUTC = solData.TryGetValue("Last_UTC", out object lUTC) ? ParseTimestamp(lUTC.ToString()) : DateTime.MinValue;
                            string season = solData.TryGetValue("Season", out object s) ? ParseSeason(s.ToString()) : String.Empty;

                            // get air temp, pressure + wind speed
                            SensorData airTemp = solData.TryGetValue("AT", out object at) ? JsonConvert.DeserializeObject<SensorData>(at.ToString()) : null;
                            SensorData airPressure = solData.TryGetValue("PRE", out object pre) ? JsonConvert.DeserializeObject<SensorData>(pre.ToString()) : null;
                            SensorData windSpeed = solData.TryGetValue("HWS", out object hws) ? JsonConvert.DeserializeObject<SensorData>(hws.ToString()) : null;

                            // get wind direction
                            WindDirectionSensorData windDirection = new WindDirectionSensorData();
                            if (solData.TryGetValue("WD", out object wd))
                            {
                                Dictionary<string, object> windData = JsonConvert.DeserializeObject<Dictionary<string, object>>(wd.ToString());
                                foreach (string pointName in windData.Keys)
                                {
                                    if (windData.TryGetValue(pointName, out object cp))
                                    {
                                        CompassPoint compassPoint = JsonConvert.DeserializeObject<CompassPoint>(cp.ToString());
                                        if (pointName != "most_common")
                                        {
                                            windDirection.CompassPoints.Add(compassPoint);
                                        }
                                        else
                                        {
                                            windDirection.MostCommon = compassPoint;
                                        }
                                    }
                                }
                            }

                            MartianDay martianDay = new MartianDay(
                                uint.Parse(solName),
                                airTemp,
                                windSpeed,
                                airPressure,
                                windDirection,
                                season,
                                firstUTC,
                                lastUTC
                                );
                            weather.Add(martianDay);
                        }
                    }
                    if (weather.Count > 0)
                    {
                        // increase cache expiry to last recorded time + 25h, minimum 1h
                        DateTime lastMeasurement = weather.OrderBy(d => d.Sol).Last<MartianDay>().LastUTC;
                        if (lastMeasurement != DateTime.MinValue)
                        {
                            expiry = lastMeasurement.AddHours(25) - DateTime.UtcNow;
                            if (expiry > TimeSpan.FromHours(1))
                            {
                                Barrel.Current.Add(dataKey, json, expiry);
                                Barrel.Current.Add(lastUpdatedKey, lastUpdated.ToString(), expiry);
                            }
                        }
                    }
                }
            }
            return (weather, lastUpdated);
        }

        private DateTime ParseTimestamp(string timestamp)
        {
            DateTime r;
            return DateTime.TryParse(timestamp, out r) ? r : DateTime.MinValue;
        }

        private string ParseSeason(string season)
        {
            return season.Substring(0, 1).ToUpper() + season.Substring(1);
        }
    }
}

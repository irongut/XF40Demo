using CsvHelper;
using CsvHelper.Configuration;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using XF40Demo.Models;

namespace XF40Demo.Services
{
    public sealed class PowerDetailsService
    {
        private static readonly PowerDetailsService instance = new PowerDetailsService();

        private readonly List<PowerDetails> powerList;

        #region Properties

        public PowerDetails SelectedPower { get; internal set; }

        #endregion

        private PowerDetailsService()
        {
            powerList = new List<PowerDetails>();
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

        private void GetPowerList()
        {
            const string fileName = "XF40Demo.Resources.PowerDetails.csv";

            Assembly assembly = GetType().GetTypeInfo().Assembly;
            using (Stream stream = assembly.GetManifestResourceStream(fileName))
                using (StreamReader reader = new StreamReader(stream))
                {
                    CsvConfiguration csvConfig = new CsvConfiguration(CultureInfo.InvariantCulture)
                    {
                        Delimiter = ","
                    };
                    using (CsvReader csv = new CsvReader(reader, csvConfig))
                    {
                        csv.Read(); // ignore header row
                        while (csv.Read())
                        {
                            powerList.Add(new PowerDetails(
                                csv.GetField<int>(0),
                                csv.GetField<string>(1),
                                csv.GetField<string>(2),
                                csv.GetField<int>(3),
                                csv.GetField<string>(4),
                                csv.GetField<string>(5),
                                csv.GetField<string>(6),
                                csv.GetField<string>(7),
                                csv.GetField<string>(8),
                                csv.GetField<string>(9),
                                csv.GetField<string>(10),
                                csv.GetField<string>(11),
                                csv.GetField<string>(12),
                                csv.GetField<string>(13),
                                csv.GetField<string>(14),
                                Desanitise(csv.GetField<string>(15)),
                                Desanitise(csv.GetField<string>(16)),
                                Desanitise(csv.GetField<string>(17)),
                                Desanitise(csv.GetField<string>(18)),
                                Desanitise(csv.GetField<string>(19)),
                                Desanitise(csv.GetField<string>(20)),
                                Desanitise(csv.GetField<string>(21)),
                                Desanitise(csv.GetField<string>(22)),
                                Desanitise(csv.GetField<string>(23)),
                                Desanitise(csv.GetField<string>(24)),
                                Desanitise(csv.GetField<string>(25))
                                ));
                        }
                    }
                }
        }

        private string Desanitise(string data)
        {
            return data.Replace("\\n", "\n");
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Threading.Tasks;
using System.Globalization;
using Data;


namespace Logic
{
    static public class DataHandler
    {
        static public Dictionary<Date, float> GetIrradianceDict()
        {
            Dictionary<Date, float> dataDict = new Dictionary<Date, float>();
            string irradianceData = DataGetter.GetIrrradianceData();
            string[] dataLines = irradianceData.Split('\n');

            foreach  (var elem in dataLines)
            {
                if(elem != "")
                {
                    string[] data = elem.Split(' ');
                    Date dataDate = new Date();
                    dataDate.year = data[0];
                    dataDate.month = data[1];
                    dataDate.day = data[2];
                    dataDate.hour = data[3];
                    dataDict[dataDate] = float.Parse(data[4], CultureInfo.InvariantCulture.NumberFormat);
                }
            }
            return dataDict;
        }

        static public Dictionary<string, float> GetInstalledPowerDict()
        {
            Dictionary<string, float> dataDict = new Dictionary<string, float>();
            string data = DataGetter.GetInstalledPowerData();
            string[] dataLines = data.Split('\n');
            foreach (var elem in dataLines)
            {
                if(elem != "" && elem != "\r")
                {
                    string[] dataLine = elem.Split(' ');
                    dataDict[dataLine[0]] = float.Parse(dataLine[1], CultureInfo.InvariantCulture.NumberFormat);
                }
                
            }
            return dataDict;
        }

        static public Dictionary<string, Dictionary<Date, double>> CalculateProducedPower()
        {
            Dictionary<string, Dictionary<Date, double>> munDict = 
                new Dictionary<string, Dictionary<Date, double>>();

            Dictionary<string, float> installedPowerDict = GetInstalledPowerDict();
            Dictionary<Date, float> irradianceDict = GetIrradianceDict();

            foreach (var municipality in installedPowerDict) {
                Dictionary<Date, double> powerDict = new Dictionary<Date, double>();
                foreach (var irradiance in irradianceDict)
                {
                    powerDict[irradiance.Key] = irradiance.Value * 0.15 * 0.9 * municipality.Value;
                }
                munDict[municipality.Key] = powerDict;   
            }
            return munDict;
        }
    }
}

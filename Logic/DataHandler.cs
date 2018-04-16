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
            string irradianceData = DataAccesser.GetIrrradianceData();
            string[] dataLines = irradianceData.Split('\n');

            foreach  (var elem in dataLines)
            {
                if(elem != "")
                {
                    string[] data = elem.Split(' ');
                    Date dataDate = new Date();
                    dataDate.date = elem;
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
            string data = DataAccesser.GetInstalledPowerData();
            string[] dataLines = data.Split('\r', '\n');
            foreach (var elem in dataLines)
            {
                if(elem != "" && elem != "\r")
                {
                    string[] dataLine = elem.Split(' ');
                    dataDict[dataLine[0]] = Int32.Parse(dataLine[1]);
                }
                
            }
            return dataDict;
        }

        static public void CalculateProducedPower()
        {
            Dictionary<string, Dictionary<Date, double>> munDict = 
                new Dictionary<string, Dictionary<Date, double>>();

            Dictionary<string, float> installedPowerDict = GetInstalledPowerDict();
            Dictionary<Date, float> irradianceDict = GetIrradianceDict();

            foreach (var municipality in installedPowerDict) {
                Dictionary<Date, double> powerDict = new Dictionary<Date, double>();
                foreach (var irradiance in irradianceDict)
                {
                    var area = municipality.Value / (0.15 * 1000); //Kommunal installerad effekt/(verkningsgrad * Irradians vid STC)
                    powerDict[irradiance.Key] = irradiance.Value * 3600 * 0.15 * 0.9 * area;
                }
                munDict[municipality.Key] = powerDict;   
            }
            DataAccesser.AddPowerRecord(munDict);
        }

        static public List<ProducedPower> GetProducedPowerOfPastWeek(string date, string kommun = null)
        {
            return DataAccesser.GetProducedPowerOfPastWeek(ConvertDate(date), kommun);
        }
        static public List<ProducedPower> GetTotalProducedPower(string kommun = null)
        {
            return DataAccesser.GetTotalProducedPower(kommun);
        }
        static public List<ProducedPower> GetProducedPowerOfDay(string date, string kommun = null)
        {

            return DataAccesser.GetProducedPowerOfDay(ConvertDate(date), kommun);
        }
        static private Date ConvertDate(string date)
        {
            Date formattedDate = new Date();
            var year = date.Split()[0];
            var month = date.Split()[1];
            var day = date.Split()[2];
            var hour = date.Split()[3];
            if (Int32.Parse(month) < 10)
            {
                month = "0" + month;
            }
            if (Int32.Parse(day) < 10)
            {
                day = "0" + day;
            }
            if (Int32.Parse(hour) < 10)
            {
                hour = "0" + hour;
            }
            formattedDate.year = year;
            formattedDate.month = month;
            formattedDate.day = day;
            formattedDate.hour = hour;
            formattedDate.date = String.Format("{0} {1} {2} {3}", year, month, day, hour);
            return formattedDate;
        }
    }
}

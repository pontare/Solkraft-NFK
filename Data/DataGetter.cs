﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Net;
using System.Threading.Tasks;

namespace Data
{

    public struct Date
    {
        public string year;
        public string month;
        public string day;
        public string hour;
    }

    static public class DataGetter
    {

        static public string GetIrrradianceData()
        {
            var date = DateTime.Today;
            var hour = 0;
            var day = date.AddDays(-1).Day;
            var year = date.Year;
            var month = date.Month;
            var nextDay = date.AddDays(1).Day;
            var requestUrl = String.Format("http://strang.smhi.se/extraction/getseries.php?par=117&m1={0}&d1={1}&y1={2}&h1={3}&m2={4}&d2={5}&y2={6}&h2={7}&lat=58.58&lon=16.15&lev=0",
                month, day, year, hour, month, nextDay, year, hour);

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(requestUrl);
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            Stream responseStream = response.GetResponseStream();
            StreamReader streamReader = new StreamReader(responseStream, Encoding.UTF8);
            string responseBody = streamReader.ReadToEnd();
            return responseBody;
        }
        static public string GetInstalledPowerData()
        {
            string data = File.ReadAllText("C:\\Users\\famat\\source\\repos\\MVP Solkalk\\Data\\installerad effekt data.txt");
            return data;
        }
    }
}

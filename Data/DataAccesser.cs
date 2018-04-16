using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Net;
using System.Data.Entity;
using System.Threading.Tasks;

namespace Data
{

    public struct Date
    {
        public string year;
        public string month;
        public string day;
        public string hour;
        public string date;
    }

    static public class DataAccesser
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
            string data = @"Aneby 249000
Tranås 527000
Nässjö 1020000
Eksjö 532000
Vetlanda 1320000
Sävsjö 591000
Värnamo 1788000
Gislaved 1080000
Gnosjö 243000
Vaggeryd 406000
Jönköping 3952000
Habo 176000
Mullsjö 358000";
            return data;
        }

        static public List<ProducedPower> GetProducedPowerOfPastWeek(Date date, string kommun = null)
        {
            using (var db = new SolkalkDbEntities1())
            {
                if(kommun != "" && kommun != null)
                {
                    var day = (Int32.Parse(date.day));
                    var dayto = 0;
                    if (day - 7 <= 0)
                    {
                        dayto = 1;
                    }
                    else
                    {
                        dayto =- 7;
                    }
                    var query = from power in db.ProducedPowers.ToList()
                                let Day = Convert.ToInt32(power.Day)
                                where (Day <= day && Day > dayto) && power.Kommun == kommun
                                orderby Day descending
                                select power;
                    return query.ToList();
                }
                else
                {
                    var day = (Int32.Parse(date.day));
                    var dayto = 0;
                    if (day - 7 <= 0)
                    {
                        dayto = 1;
                    }
                    else
                    {
                        dayto = day - 7;
                    }
                    var query = from power in db.ProducedPowers.ToList()
                                let Day = Int32.Parse(power.Day)
                                where (Day <= day && Day > dayto)
                                orderby Day descending
                                select power;
                    return query.ToList();
                }
            }
        }
        static public List<ProducedPower> GetTotalProducedPower(string kommun = null)
        {
            using (var db = new SolkalkDbEntities1())
            {
                if (kommun != "" && kommun != null)
                {
                    var query = from power in db.ProducedPowers
                                where power.Kommun == kommun
                                select power;
                    return query.ToList();
                }
                else
                {
                    var query = from power in db.ProducedPowers
                                select power;
                    return query.ToList();
                }
            }
        }
        static public List<ProducedPower> GetProducedPowerOfDay(Date date, string kommun)
        {
            using (var db = new SolkalkDbEntities1())
            {
                if (kommun != "" && kommun != null)
                {
                    var query = from power in db.ProducedPowers
                                where power.Kommun == kommun && power.Day == date.day
                                select power;
                    return query.ToList();
                }
                else
                {
                    var query = from power in db.ProducedPowers
                                where power.Day == date.day
                                select power;
                    return query.ToList();
                }
            }
        }
        static public void AddPowerRecord(Dictionary<string, Dictionary<Date, double>> powerDict)
        {
            foreach (var elem in powerDict)
            {
                foreach (var elem2 in elem.Value)
                {
                    var powerRecord = new ProducedPower();
                    powerRecord.Kommun = elem.Key;
                    powerRecord.Year = elem2.Key.year;
                    powerRecord.Month = elem2.Key.month;
                    powerRecord.Day = elem2.Key.day;
                    powerRecord.Hour = elem2.Key.hour;
                    powerRecord.Energi = elem2.Value;
                    using (var db = new SolkalkDbEntities1())
                    {
                        db.ProducedPowers.Add(powerRecord);
                        db.SaveChanges();
                    }
                }
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Data
{
    public class CompanyDataAccesser
    {
        static public List<ProducedMunicipalPower> GetTotalProducedPower(string kommun = null)
        {
            using (var db = new SolkalkDbEntities1())
            {
                if (kommun != "" && kommun != null)
                {
                    var query = from power in db.ProducedMunicipalPowers
                                where power.Kommun == kommun
                                select power;
                    return query.ToList();
                }
                else
                {
                    var query = from power in db.ProducedMunicipalPowers
                                select power;
                    return query.ToList();
                }
            }
        }
        static public List<ProducedMunicipalPower> GetProducedPowerOfDay(Date date, string kommun)
        {
            using (var db = new SolkalkDbEntities1())
            {
                if (kommun != "" && kommun != null)
                {
                    var query = from power in db.ProducedMunicipalPowers
                                where power.Kommun == kommun && power.Day == date.day
                                select power;
                    return query.ToList();
                }
                else
                {
                    var query = from power in db.ProducedMunicipalPowers
                                where power.Day == date.day
                                select power;
                    return query.ToList();
                }
            }
        }
    }
}
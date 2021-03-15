using ConsoleSEBTask.Models;
using System;
using System.Xml;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using System.Xml.Serialization;
using System.IO;
using System.Net;
using System.Linq;

namespace ConsoleSEBTask
{
    class Program
    {
        public static bool ValidateDate(string date)
        {
            DateTime fromDateValue;
            var formats = new[] { "yyyy'-'MM'-'dd" };
            return DateTime.TryParseExact(date, formats, System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None, out fromDateValue);

        }
        public static ExchangeRates GetCurrentDayRates(string url)
        {
            WebClient client = new WebClient();
            string reply = client.DownloadString(url);

            Console.WriteLine(reply);

            XmlSerializer serializer = new XmlSerializer(typeof(ExchangeRates));
            using (TextReader readString = new StringReader(reply))
            {
                ExchangeRates rateDayCurrent = (ExchangeRates)serializer.Deserialize(readString);
                return rateDayCurrent;
            }
            
        }
        public static ExchangeRates GetPreviousDayRates(string url)
        {
            WebClient client = new WebClient();
            string reply = client.DownloadString(url);

            Console.WriteLine(reply);

            XmlSerializer serializer = new XmlSerializer(typeof(ExchangeRates));
            using (TextReader readString = new StringReader(reply))
            {
                ExchangeRates rateDayPrevious = (ExchangeRates)serializer.Deserialize(readString);
                return rateDayPrevious;
            }

        }
        static async Task Main(string[] args)
        {

            Console.WriteLine("Enter year:");
            var yearString = Console.ReadLine().Trim();
            Console.WriteLine("Enter month");
            var monthString = Console.ReadLine().Trim();
            Console.WriteLine("Enter day");
            var dayString = Console.ReadLine().Trim();
            string date = yearString + "-" + monthString + "-" + dayString;
            string previousDay = "";
            if (ValidateDate(date))
            {
                DateTime DateObject = Convert.ToDateTime(date);
                DateTime PrevipusDayObject = DateObject.AddDays(-10);
                previousDay = PrevipusDayObject.ToString("yyyy-MM-dd");
            }
            string apiDay = "http://www.lb.lt//webservices/ExchangeRates/ExchangeRates.asmx/getExchangeRatesByDate?Date=" + date;
            string apiPreviousDay = "http://www.lb.lt//webservices/ExchangeRates/ExchangeRates.asmx/getExchangeRatesByDate?Date=" + previousDay;

            var current = GetCurrentDayRates(apiDay);
            var previous = GetPreviousDayRates(apiPreviousDay);
            List<RateDifference> comparison = new List<RateDifference>();
            
            foreach(var rate in current.Currencies)
            {
              
                 decimal prevRate =  previous.Currencies.Where(p => p.Currency == rate.Currency).Select(p => p.Rate).FirstOrDefault();

                comparison.Add(new RateDifference()
                {
                    Currency = rate.Currency,
                    Difference = rate.Rate - prevRate
                });

            }
            comparison = comparison.OrderByDescending(rate => rate.Difference).ToList();

            foreach(var rate in comparison)
            {
                Console.WriteLine($"{rate.Currency} changed for {rate.Difference}");
            }

        }
    }
}

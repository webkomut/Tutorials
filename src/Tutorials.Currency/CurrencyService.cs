using System;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Tutorials.Currency.Models;
using Tutorials.Currency.Serializer;

namespace Tutorials.Currency
{
    public class CurrencyService : ICurrencyService
    {
        private string urlPatter = "https://www.tcmb.gov.tr/kurlar/{0}.xml";
        private readonly WebClient client;
        private readonly IXmlSerializer serializer;
        public CurrencyService()
        {
            client = new WebClient
            {
                Encoding = Encoding.UTF8
            };
            serializer = new XmlSerializer();
        }
        public Task<CurrencyModel[]> GetToday()
        {
            var url = new Uri(string.Format(urlPatter, "today"));
            var data = client.DownloadString(url);
            var deserialize = serializer.Deserializer<Tarih_Date>(data);
            var result = deserialize.Currency.Select(CurrencyModel.Map).ToArray();
            return Task.FromResult(result);
        }

        public Task<CurrencyModel[]> GetByDate(DateTime date)
        {
            var day = date.Day > 0 && date.Day < 10 ? $"0{date.Day}" : date.Day.ToString();
            var month = date.Month > 0 && date.Month < 10 ? $"0{date.Month}" : date.Month.ToString();
            var url = new Uri(string.Format(urlPatter, $"{date.Year}{month}/{day}{month}{date.Year}"));
            var data = client.DownloadString(url);
            var deserializer = serializer.Deserializer<Tarih_Date>(data);
            var result = deserializer.Currency.Select(CurrencyModel.Map).ToArray();
            return Task.FromResult(result);
        }
    }
}
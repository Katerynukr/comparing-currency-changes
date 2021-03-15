using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace ConsoleSEBTask.Models
{
    [XmlRoot("ExchangeRates")]
    public class ExchangeRates
    {
        [XmlElement("item")]
        public List<Item> Currencies { get; set; }
    }
}

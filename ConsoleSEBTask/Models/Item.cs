using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace ConsoleSEBTask.Models
{
    
    public class Item
    {
        [XmlElement("date")]
        public string Date { get; set; }
        [XmlElement("currency")]
        public string Currency { get; set; }
        [XmlElement("rate")]
        public decimal Rate { get; set; }
        [XmlElement("quantity")]
        public int quantity { get; set; }
        [XmlElement("unit")]
        public string Unit { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml.Serialization;

namespace SIPVS.Models
{
    public class Predmet
    {
        [XmlElement("Nazov")]
        public string nazov { get; set; }
        public int? prvy_rocnik { get; set; }
        public int? druhy_rocnik { get; set; }
        public int? treti_rocnik { get; set; }
        public int? stvrty_rocnik { get; set; }
        public int? piaty_rocnik { get; set; }
        public int? siesty_rocnik { get; set; }
    }
}
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
        public string prvy_rocnik { get; set; }
        public string druhy_rocnik { get; set; }
        public string treti_rocnik { get; set; }
        public string stvrty_rocnik { get; set; }
        public string piaty_rocnik { get; set; }
        public string siesty_rocnik { get; set; }
    }
}
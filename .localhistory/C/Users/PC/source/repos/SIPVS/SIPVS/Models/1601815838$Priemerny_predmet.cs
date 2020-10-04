using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml.Serialization;

namespace SIPVS.Models
{
    public class Priemerny_predmet
    {

        public string nazov { get; set; }
        public double? prvy_rocnik { get; set; }
        public double? druhy_rocnik { get; set; }
        public double? treti_rocnik { get; set; }
        public double? stvrty_rocnik { get; set; }
        public double? piaty_rocnik { get; set; }
        public double? siesty_rocnik { get; set; }
    }
}
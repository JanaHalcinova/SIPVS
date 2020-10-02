using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml.Serialization;

namespace SIPVS.Models
{
    public class Student
    {
        [XmlElement("Rok_od")]
        public int? rok_od { get; set; }
        [XmlElement("Rok_do")]
        public int? rok_do { get; set; }
        public string obcianstvo { get; set; }
        [XmlElement("Meno")]
        public string meno { get; set; }
        public string priezvisko { get; set; }
        public string rodne_priezvisko { get; set; }
        public string titul { get; set; }
        [XmlAttribute("Pohlavie")]
        public string pohlavie { get; set; }
        public DateTime? datum_narodenia { get; set; }
        public string miesto_narodenia { get; set; }
        public string stat { get; set; }
        public string rodne_cislo { get; set; }
        public string telefon { get; set; }
        public string email { get; set; }
        public List<Predmet> predmety { get; set; }

    }
}
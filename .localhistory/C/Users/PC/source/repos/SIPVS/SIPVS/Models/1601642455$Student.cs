using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml.Serialization;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

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
        [RegularExpression(@"^[0-9]{2,6}-?[0 - 9]{2,10}\/[0-9]{4}$", ErrorMessage = "Zadaj valídne rodné číslo.")] 
        public string rodne_cislo { get; set; }
        [RegularExpression(@"^[+]?[() / 0 - 9. -]{9,}$", ErrorMessage = "Zadaj valídne rodné číslo.")]
        public string telefon { get; set; }
        [RegularExpression(@"^[\w-\._\+%]+@(?:[\w-]+\.)+[\w]{2,6}$", ErrorMessage = "Zadaj valídny email.")]
        public string email { get; set; }
        public List<Predmet> predmety { get; set; }

    }
}
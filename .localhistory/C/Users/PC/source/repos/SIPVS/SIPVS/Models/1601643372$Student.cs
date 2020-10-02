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
        [Required(ErrorMessage = "Atribút je povinný.")]
        public int? rok_od { get; set; }
        [XmlElement("Rok_do")]
        [Required(ErrorMessage = "Atribút je povinný.")]
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
        [RegularExpression(@"^\s*(\d\d)(\d\d)(\d\d)[ /]*(\d\d\d)(\d?)\s*$", ErrorMessage = "Zadaj valídne rodné číslo.")] 
        public string rodne_cislo { get; set; }
        [RegularExpression(@"([0-9]{3})?[ .-]?([0-9]{3})[ .-]?([0-9]{4})", ErrorMessage = "Zadaj valídny formát telefónneho čísla.")]
        public string telefon { get; set; }
        [RegularExpression(@"^[\w-\._\+%]+@(?:[\w-]+\.)+[\w]{2,6}$", ErrorMessage = "Zadaj valídny email.")]
        public string email { get; set; }
        public List<Predmet> predmety { get; set; }

    }
}
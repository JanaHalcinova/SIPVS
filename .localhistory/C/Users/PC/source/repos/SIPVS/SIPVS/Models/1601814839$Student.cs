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
        [Required(ErrorMessage = "Atribút je povinný.")]
        public string obcianstvo { get; set; }
        [XmlElement("Meno")]
        [Required(ErrorMessage = "Atribút je povinný.")]
        public string meno { get; set; }
        [Required(ErrorMessage = "Atribút je povinný.")]
        public string priezvisko { get; set; }
        public string rodne_priezvisko { get; set; }
        public string titul { get; set; }
        [XmlAttribute("Pohlavie")]
        public string pohlavie { get; set; }
        [Required(ErrorMessage = "Atribút je povinný.")]
        public DateTime? datum_narodenia { get; set; }
        [Required(ErrorMessage = "Atribút je povinný.")]
        public string miesto_narodenia { get; set; }
        [Required(ErrorMessage = "Atribút je povinný.")]
        public string stat { get; set; }
        [RegularExpression(@"^\s*(\d\d)(\d\d)(\d\d)[ /]*(\d\d\d)(\d?)\s*$", ErrorMessage = "Zadaj valídne rodné číslo.")]
        [Required(ErrorMessage = "Atribút je povinný.")]
        public string rodne_cislo { get; set; }
        [RegularExpression(@"(^\+42[0-9][0-9]{9}$)|(^09[0-9]{8}$)", ErrorMessage = "Zadaj valídny formát telefónneho čísla.")]
        [Required(ErrorMessage = "Atribút je povinný.")]
        public string telefon { get; set; }
        [RegularExpression(@"^[\w-\._\+%]+@(?:[\w-]+\.)+[\w]{2,6}$", ErrorMessage = "Zadaj valídny email.")]
        [Required(ErrorMessage = "Atribút je povinný.")]
        public string email { get; set; }
        public List<Predmet> predmety { get; set; }

    }
}
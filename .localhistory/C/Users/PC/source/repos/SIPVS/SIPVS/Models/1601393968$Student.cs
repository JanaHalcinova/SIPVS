using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SIPVS.Models
{
    public class Student
    {
        public int rok_od { get; set; }
        public int rok_do { get; set; }
        public string obcianstvo { get; set; }
        public string meno { get; set; }
        public string priezvisko { get; set; }
        public string rodne_priezvisko { get; set; }
        public string titul { get; set; }
        public string pohlavie { get; set; }
        public DateTime datum_narodenia { get; set; }
        public string miesto_narodenia { get; set; }
        public string stat { get; set; }
        public string rodne_cislo { get; set; }
        public string telefon { get; set; }
        public string email { get; set; }

    }
}
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Mvc;
using System.Xml;
using SIPVS.Models;

namespace SIPVS.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Index(Student student, string action)
        {
            CultureInfo culture = new CultureInfo("sk-SK");
            Thread.CurrentThread.CurrentCulture = culture;
            Thread.CurrentThread.CurrentUICulture = culture;

            switch (action)
            {
              case "submit":
                    Priemerny_predmet priemer = new Priemerny_predmet();
                    List<int?> prvy = new List<int?>();
                    foreach (var predmet in student.predmety)
                    {
                        if (predmet.prvy_rocnik!= null)
                        {
                            prvy.Add(predmet.prvy_rocnik);
                        }
                    }
                    if (prvy.Count > 0)
                    {
                        priemer.prvy_rocnik = prvy.Average();
                    }

                    student.priemer = priemer;
                    System.Xml.Serialization.XmlSerializer writer =
                        new System.Xml.Serialization.XmlSerializer(typeof(Student));

                    var path = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "//Student.xml";
                    System.IO.FileStream file = System.IO.File.Create(path);

                    writer.Serialize(file, student);
                    file.Close();
                    return View(student);


                case "validate":
                    //otvori sa subor Student.xml
                    //validuje sa
                    //ak validacia je ok
                    ViewBag.message = "XML súbor je valídny.";
                    //ak validacia nie je ok
                    ViewBag.message = "XML súbor nie je valídny.";
                    return View(student);

                    
                case "display":
                    //otvorí xml v prehliadaci na novej karte : automaticky by sa to malo vygenerovat s novym vzhladom
                    ViewBag.message = "PDF bolo vygenerované.";
                    return View(student);

                

            }
            return View(student);
        }

    }
}
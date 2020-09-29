using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
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

            switch (action)
            {
              case "submit":
                    return View(student);


                case "validate":
                    //otvori sa subor
                    //validuje podla druheho suboru
                    //ak validacia je ok
                    ModelState.AddModelError("", "XML súbor je valídny.");
                    //ak validacia nie je ok
                    ModelState.AddModelError("", "XML súbor nie je valídny.");
                    return View(student);

                    
                case "display":
                    //otvorí xml v prehliadaci na novej karte : automaticky by sa to malo vygenerovat s novym vzhladom
                    ModelState.AddModelError("", "PDF bolo vygenerované.");
                    return View(student);

            }
            return View(student);
        }

    }
}
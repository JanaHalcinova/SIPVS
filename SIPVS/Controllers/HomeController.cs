using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Mvc;
using System.Xml;
using System.Xml.Schema;
using SIPVS.Models;
using System.Xml.Linq;
using System.Xml.Xsl;
using System.IO;
using System.Xml.Serialization;

namespace SIPVS.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            Student student = new Student();
            student.predmety = null;
            return View(student);
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
                    List<int?> prvy = new List<int?>();
                    List<int?> druhy = new List<int?>();
                    List<int?> treti = new List<int?>();
                    List<int?> stvrty = new List<int?>();
                    List<int?> piaty = new List<int?>();
                    List<int?> siesty = new List<int?>();

                    if (student.predmety != null)
                    {
                    Priemerny_predmet priemer = new Priemerny_predmet();
                    foreach (var predmet in student.predmety)
                    {
                        if (predmet.prvy_rocnik!= null)
                        {
                            prvy.Add(predmet.prvy_rocnik);
                        }
                        if (predmet.druhy_rocnik != null)
                        {
                            druhy.Add(predmet.druhy_rocnik);
                        }
                        if (predmet.treti_rocnik != null)
                        {
                            treti.Add(predmet.treti_rocnik);
                        }
                        if (predmet.stvrty_rocnik != null)
                        {
                            stvrty.Add(predmet.stvrty_rocnik);
                        }
                        if (predmet.piaty_rocnik != null)
                        {
                            piaty.Add(predmet.piaty_rocnik);
                        }
                        if (predmet.siesty_rocnik != null)
                        {
                            siesty.Add(predmet.siesty_rocnik);
                        }
                    }
                    if (prvy.Count > 0)
                    {
                        priemer.prvy_rocnik = prvy.Average();
                    }
                    if (druhy.Count > 0)
                    {
                        priemer.druhy_rocnik = druhy.Average();
                    }
                    if (treti.Count > 0)
                    {
                        priemer.treti_rocnik = treti.Average();
                    }
                    if (stvrty.Count > 0)
                    {
                        priemer.stvrty_rocnik = stvrty.Average();
                    }
                    if (piaty.Count > 0)
                    {
                        priemer.piaty_rocnik = piaty.Average();
                    }
                    if (siesty.Count > 0)
                    {
                        priemer.siesty_rocnik = siesty.Average();
                    }

                    student.priemer = priemer;
                    }
                    System.Xml.Serialization.XmlSerializer writer =
                        new System.Xml.Serialization.XmlSerializer(typeof(Student));

                    var ns = new XmlSerializerNamespaces();
                    ns.Add(String.Empty, "http://fiit.stu.sk/SIPVS/UniversityApplication");
                    var path = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "//Student.xml";
                    System.IO.FileStream file = System.IO.File.Create(path);
                    writer.Serialize(file, student);
                    file.Close();
                    return View(student);


                case "validate":
                    //otvori sa subor Student.xml
                    XmlSchemaSet schemas = new XmlSchemaSet();
                    schemas.Add("http://fiit.stu.sk/SIPVS/UniversityApplication", AppDomain.CurrentDomain.BaseDirectory + "Content/subory/schema.xsd");

                    XDocument doc = XDocument.Load(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "//Student.xml");

                    bool errors = false;
                    doc.Validate(schemas, (o, e) =>
                    {
                        ViewBag.message = "XML súbor nie je valídny. Chyba: " + e.Message;
                        errors = true;
                    });
                    if (!errors)
                        ViewBag.message = "XML súbor je valídny.";

                    return View(student);

                case "sign":
                    //otvori sa subor Student.xml
                    var inputString = "HAHAHA123"; 
                    var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(inputString);
                    var result = System.Convert.ToBase64String(plainTextBytes);
                    return null;
            }
            return View(student);
        }

        [HttpPost]
        public ActionResult Sign(string ret)
        {
            byte[] data = System.Convert.FromBase64String(ret);
            string xadesXml = System.Text.Encoding.UTF8.GetString(data);

            ViewBag.signString = xadesXml;

            System.IO.File.WriteAllText(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "//xades.xml", xadesXml);

            return RedirectToAction("SignXml", "Home");
        }

        public ActionResult SignXml()
        {
            
            return View();
        }

        public ActionResult Application()
        {
            /*
            string xml_view = AppDomain.CurrentDomain.BaseDirectory + "Content/subory/xml_view.xsl"; //path of xslt file
            string xsltPath = AppDomain.CurrentDomain.BaseDirectory + "Content/subory/view.xsl"; //path of xslt file
            var path_new = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "//Student_new.xml";

            XslCompiledTransform xslt = new XslCompiledTransform();
            xslt.Load(xml_view);
            xslt.Transform(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "//Student.xml", path_new);

            string xml = System.IO.File.ReadAllText(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "//Student_new.xml");
            ViewBag.htmlString = CustomHTMLHelper.RenderXMLData(xml, xsltPath).ToString();
            System.IO.File.WriteAllText(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "//Student.html", ViewBag.htmlString);
            */

            string xml = System.IO.File.ReadAllText(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "//Student.xml");
            string xsltPath = AppDomain.CurrentDomain.BaseDirectory + "Content/subory/view.xsl"; //path of xslt file

            ViewBag.htmlString = CustomHTMLHelper.RenderXMLData(xml, xsltPath).ToString();
            System.IO.File.WriteAllText(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "//Student.html", ViewBag.htmlString);

            return View();
        }
    }
}

public static class CustomHTMLHelper
{
    /// <summary>  
    /// Applies an XSL transformation to an XML document  
    /// </summary>  
    /// <param name="helper"></param>  
    /// <param name="xml"></param>  
    /// <param name="xsltPath"></param>  
    /// <returns></returns>  
    public static HtmlString RenderXMLData(string xml, string xsltPath)
    {
        XsltArgumentList args = new XsltArgumentList();
        // Create XslCompiledTransform object to loads and compile XSLT file.  
        XslCompiledTransform tranformObj = new XslCompiledTransform();
        tranformObj.Load(xsltPath);

        // Create XMLReaderSetting object to assign DtdProcessing, Validation type  
        XmlReaderSettings xmlSettings = new XmlReaderSettings();
        xmlSettings.DtdProcessing = DtdProcessing.Parse;
        xmlSettings.ValidationType = ValidationType.DTD;

        // Create XMLReader object to Transform xml value with XSLT setting   
        using (XmlReader reader = XmlReader.Create(new StringReader(xml), xmlSettings))
        {
            StringWriter writer = new StringWriter();
            tranformObj.Transform(reader, args, writer);

            // Generate HTML string from StringWriter  
            HtmlString htmlString = new HtmlString(writer.ToString());
            return htmlString;
        }
    }
}
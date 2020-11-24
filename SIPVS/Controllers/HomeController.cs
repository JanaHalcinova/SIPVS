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
using System.Net;
using System.Xml.Serialization;
using Org.BouncyCastle.Tsp;

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
                        priemer.prvy_rocnik = prvy.Average().HasValue ? Math.Round(prvy.Average().Value, 2) : 0;
                    }
                    if (druhy.Count > 0)
                    {
                        priemer.druhy_rocnik = druhy.Average().HasValue ? Math.Round(druhy.Average().Value, 2) : 0;
                    }
                    if (treti.Count > 0)
                    {
                        priemer.treti_rocnik = treti.Average().HasValue ? Math.Round(treti.Average().Value, 2) : 0;
                    }
                    if (stvrty.Count > 0)
                    {
                        priemer.stvrty_rocnik = stvrty.Average().HasValue ? Math.Round(stvrty.Average().Value, 2) : 0;
                    }
                    if (piaty.Count > 0)
                    {
                        priemer.piaty_rocnik = piaty.Average().HasValue ? Math.Round(piaty.Average().Value, 2) : 0;
                    }
                    if (siesty.Count > 0)
                    {
                        priemer.siesty_rocnik = siesty.Average().HasValue ? Math.Round(siesty.Average().Value, 2) : 0;
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

                case "timestamp":

                    return View(student);

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

        public ActionResult Timestamp()
        {
            XmlDocument doc = new XmlDocument();
            string xmlData = System.IO.File.ReadAllText(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "//xades.xml");
            doc.Load(new StringReader(xmlData));

            string signatureValue = doc.GetElementsByTagName("ds:SignatureValue")[0].OuterXml.ToString();
            var signatureBytes = System.Text.Encoding.UTF8.GetBytes(signatureValue);
            string dataB64 = Convert.ToBase64String(signatureBytes);
            
            //String dataB64 = "PGRzOlNpZ25hdHVyZVZhbHVlIElkPSJzaWduYXR1cmVJZFNpZ25hdHVyZVZhbHVlIj5pU20zSE1lSlJUbHVkK0p5T0Vpc2cwc0RIK3pmbVFReElVQ3ducGgwUDh0ME1mY0NzdDN0WEw3UHhqdkFpc3dIYlpjc3dHMEtqZmpqDQpHc1NjaW9kMkdQSUdQOHVjR1AwbVc0RFdpQXVDNEZBcFZBUk1NWDRVWW93VCtBeVdrM1UweGN2N2w5UW1kRVNpY1hUbjcyV3dPTVpkDQoya3MydXg3SUttOTE3Y3dRN3kvVC9rZjQrVnBoNCtzU0phWkxyRGc1NkRaaWR0SGRtYVZ1dEIrdkJqa3pNRkJDaXl6ZDVWWlA3Q3lqDQpLSGpqTTZxT1NYNzJuQjJXd2V4aTVFZVJDZVRKSUJRSVBaWGVvb1FUQzJBWkdTeEtOMkFhZ1pqM09BbVNmRWFJK2JsWkNRWlNoVE9SDQoxdVNaMS8xNmZNYVBBaGh3cG52eU5RYS9LblN4dlhsSUYrbVRDdz09PC9kczpTaWduYXR1cmVWYWx1ZT4=";

            XmlDocument soapEnvelopeXml = CreateSoapEnvelope(dataB64);
            HttpWebRequest webRequest = CreateWebRequest();
            InsertSoapEnvelopeIntoWebRequest(soapEnvelopeXml, webRequest);

            // begin async call to web request.
            IAsyncResult asyncResult = webRequest.BeginGetResponse(null, null);

            // suspend this thread until call is complete. You might want to
            // do something usefull here like update your UI.
            asyncResult.AsyncWaitHandle.WaitOne();

            // get the response from the completed web request.

            string soapResult;
            using (WebResponse webResponse = webRequest.EndGetResponse(asyncResult))
            {
                using (StreamReader rd = new StreamReader(webResponse.GetResponseStream()))
                {
                    soapResult = rd.ReadToEnd();
                }
                Console.Write(soapResult);
                ViewBag.soapResult = soapResult;
            }

            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(soapResult);

            string timestampResultEncoded = xmlDoc.GetElementsByTagName("GetTimestampResult")[0].InnerText;

            TimeStampResponse response = new TimeStampResponse(Convert.FromBase64String(timestampResultEncoded));
            //ViewBag.soapResult = response.ToString();

            //var soapResultBytes = System.Text.Encoding.UTF8.GetBytes(soapResult);
            //TimeStampParser.TSSoapClient ts = new TimeStampParser.TSSoapClient();
            //string ret = ts.GetTimestamp(Convert.ToBase64String(soapResultBytes));

            ViewBag.soapResult = System.Text.Encoding.UTF8.GetString(response.GetEncoded());
            var token = response.TimeStampToken;

            return View();
        }

        private static HttpWebRequest CreateWebRequest()
        {
            HttpWebRequest webRequest = (HttpWebRequest)WebRequest.Create(@"http://test.ditec.sk/timestampws/TS.asmx?op=GetTimestamp");
            webRequest.Headers.Add(@"SOAP:Action");
            webRequest.ContentType = "text/xml;charset=\"utf-8\"";
            webRequest.Accept = "text/xml";
            webRequest.Method = "POST";
            return webRequest;
        }

        private static XmlDocument CreateSoapEnvelope(String dataB64)
        {
            XmlDocument soapEnvelopeDocument = new XmlDocument();
            String xmlString = String.Format(
                @"<soap:Envelope
                    xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance""
                    xmlns:xsd=""http://www.w3.org/2001/XMLSchema""
                    xmlns:soap=""http://schemas.xmlsoap.org/soap/envelope/"">
                    <soap:Body>
                      <GetTimestamp xmlns=""http://www.ditec.sk/"">
                        <dataB64>{0}</dataB64>
                      </GetTimestamp>
                    </soap:Body>
                  </soap:Envelope>",
                dataB64
            );
            soapEnvelopeDocument.LoadXml(xmlString);

            return soapEnvelopeDocument;
        }

        private static void InsertSoapEnvelopeIntoWebRequest(XmlDocument soapEnvelopeXml, HttpWebRequest webRequest)
        {
            using (Stream stream = webRequest.GetRequestStream())
            {
                soapEnvelopeXml.Save(stream);
            }
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

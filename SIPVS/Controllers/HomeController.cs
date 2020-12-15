using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Web;
using System.Net;
using System.Web.Mvc;
using System.Xml;
using System.Xml.Schema;
using SIPVS.Models;
using System.Xml.Linq;
using System.Xml.Xsl;
using System.IO;
using System.Net;
using System.Xml.XPath;
using System.Xml.Serialization;
using System.Security;
using System.Security.Cryptography.Xml;
using System.Security.Cryptography.X509Certificates;
using Org.BouncyCastle.Tsp;
using System.Security.Cryptography;
using Org.BouncyCastle.X509;
using Org.BouncyCastle.Asn1;
using Org.BouncyCastle.Crypto.Tls;
using Org.BouncyCastle.Asn1.X509;
using HashAlgorithm = System.Security.Cryptography.HashAlgorithm;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Security;
using Org.BouncyCastle.Utilities.Encoders;
using X509Certificate = Org.BouncyCastle.X509.X509Certificate;
using System.Collections;
using Org.BouncyCastle.Cms;
using Org.BouncyCastle.X509.Store;
using Org.BouncyCastle.Asn1.Oiw;
using Org.BouncyCastle.Asn1.Nist;
using System.Text;
using Org.BouncyCastle.Crypto.Digests;
using Org.BouncyCastle.Security.Certificates;

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

        
        public ActionResult XadesT()
        {
            
            return View();
        }

        public ActionResult Result()
        {
            if (TempData["xadesResult"] != null)
            {
                ViewBag.xadesResult = TempData["xadesResult"];
            }

            return View();
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult XadesResult(String input)
        {
            //schemas.Add("http://www.ditec.sk/ep/signature_formats/xades_zep/v2.0", Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "//xades_zep.v2.0.xsd");

            try
            {
                //XDocument doc = XDocument.Load(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "//01XadesT.xml");
                XDocument doc = XDocument.Parse(input);

                // Overenie datovej obalky
                if (doc.Root.Attribute(XNamespace.Xmlns + "xzep").Value.Equals("http://www.ditec.sk/ep/signature_formats/xades_zep/v1.0") == false)
                {
                    throw new Exception("Atribút xmlns:xzep koreňového elementu neobsahuje hodnotu http://www.ditec.sk/ep/signature_formats/xades_zep/v1.0");
                }
                else if (doc.Root.Attribute(XNamespace.Xmlns + "ds").Value.Equals("http://www.w3.org/2000/09/xmldsig#") == false)
                {
                    throw new Exception("Atribút xmlns:ds koreňového elementu neobsahuje hodnotu http://www.w3.org/2000/09/xmldsig#");
                }

                //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

                // Overenie XML Signature

                // Kontrola obsahu ds:SignatureMethod

                XmlReader reader = XmlReader.Create(new StringReader(input));
                XElement root = XElement.Load(reader);
                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.LoadXml(input);
                XmlNameTable nameTable = reader.NameTable;
                XmlNamespaceManager namespaceManager = new XmlNamespaceManager(nameTable);

                namespaceManager.AddNamespace("ds", "http://www.w3.org/2000/09/xmldsig#");
                namespaceManager.AddNamespace("xzep", "http://www.ditec.sk/ep/signature_formats/xades_zep/v1.0");
                namespaceManager.AddNamespace("xades", "http://uri.etsi.org/01903/v1.3.2#");

                XElement sigMethod = root.XPathSelectElement("//ds:Signature/ds:SignedInfo/ds:SignatureMethod", namespaceManager);

                if (sigMethod == null)
                {
                    throw new Exception("Chyba pri kontrole elementu ds:Signature/ds:SignedInfo/ds:SignatureMethod. Element nebol v dokumente nájdený");
                }

                string[] sigMethods = { "http://www.w3.org/2000/09/xmldsig#dsa-sha1",
                                        "http://www.w3.org/2000/09/xmldsig#rsa-sha1",
                                        "http://www.w3.org/2001/04/xmldsig-more#rsa-sha256",
                                        "http://www.w3.org/2001/04/xmldsig-more#rsa-sha384",
                                        "http://www.w3.org/2001/04/xmldsig-more#rsa-sha512"};

                if (Array.Exists(sigMethods, element => element == sigMethod.Attribute("Algorithm").Value) == false)
                {
                    throw new Exception("Atribút Algorithm elementu ds:SignatureMethod neobsahuje URI niektorého z podporovaných algoritmov.");
                }

                //--------------------------------------------------------------------------------------------------------------------

                // Kontrola obsahu ds:CanonicalizationMethod

                XElement canonMethod = root.XPathSelectElement("//ds:Signature/ds:SignedInfo/ds:CanonicalizationMethod", namespaceManager);

                if (canonMethod == null)
                {
                    throw new Exception("Chyba pri kontrole elementu ds:Signature/ds:SignedInfo/ds:CanonicalizationMethod. Element nebol v dokumente nájdený.");
                }

                string[] canonMethods = { "http://www.w3.org/TR/2001/REC-xml-c14n-20010315"};

                if (Array.Exists(canonMethods, element => element == canonMethod.Attribute("Algorithm").Value) == false)
                {
                    throw new Exception("Atribút Algorithm elementu ds:CanonicalizationMethod neobsahuje URI niektorého z podporovaných algoritmov");
                }

                //--------------------------------------------------------------------------------------------------------------------

                // Kontrola obsahu ds:Transforms vo vs. referenciach v ds:SignedInfo

                IEnumerable<XElement> transformsElems = root.XPathSelectElements("//ds:Signature/ds:SignedInfo/ds:Reference/ds:Transforms", namespaceManager);

                if (transformsElems == null)
                {
                    throw new Exception("Chyba pri kontrole elementu ds:Signature/ds:SignedInfo/ds:Reference/ds:Transforms. Element nebol v dokumente nájdený.");
                }

                string[] transformMethods = { "http://www.w3.org/TR/2001/REC-xml-c14n-20010315" };

                foreach (XElement el in transformsElems)
                {
                    XmlElement elem = (XmlElement)xmlDoc.ReadNode(el.CreateReader());
                    XmlElement transformElement = (XmlElement) elem.GetElementsByTagName("ds:Transform").Item(0);

                    // Kontrola obsahu ds:Transforms. Musi obsahovať URI niektorého z podporovaných algoritmov

                    if (Array.Exists(transformMethods, element => element == transformElement.GetAttribute("Algorithm")) == false)
                    {
                        throw new Exception("Atribút Algorithm elementu ds:Transforms neobsahuje URI niektorého z podporovaných algoritmov");
                    }
                }

                //--------------------------------------------------------------------------------------------------------------------

                // Kontrola obsahu ds:DigestMethod vo vs. referenciach v ds:SignedInfo

                IEnumerable<XElement> digestElems = root.XPathSelectElements("//ds:Signature/ds:SignedInfo/ds:Reference/ds:DigestMethod", namespaceManager);

                if (digestElems == null)
                {
                    throw new Exception("Chyba pri kontrole elementu ds:Signature/ds:SignedInfo/ds:Reference/ds:DigestMethod. Element nebol v dokumente nájdený.");
                }

                string[] digestMethods = {  "http://www.w3.org/2000/09/xmldsig#sha1",
                                            "http://www.w3.org/2001/04/xmldsig-more#sha224",
                                            "http://www.w3.org/2001/04/xmlenc#sha256",
                                            "http://www.w3.org/2001/04/xmldsig-more#sha384",
                                            "http://www.w3.org/2001/04/xmlenc#sha512"};

                foreach (XElement el in digestElems)
                {
                    //System.Diagnostics.Debug.WriteLine(el.Attribute("Algorithm").Value);

                    if (Array.Exists(digestMethods, element => element == el.Attribute("Algorithm").Value) == false)
                    {
                        throw new Exception("Atribút Algorithm elementu ds:DigestMethod neobsahuje URI niektorého z podporovaných algoritmov");
                    }
                }

                //--------------------------------------------------------------------------------------------------------------------

                /*
                 * Core validation (podľa špecifikácie XML Signature)
                 * Dereferencovanie URI, kanonikalizácia referencovaných ds:Manifest elementov
                 * a overenie hodnôt odtlačkov ds:DigestValue
                 */

                Dictionary<string, string> digestAlgo = new Dictionary<string, string>() {
                    { "http://www.w3.org/2000/09/xmldsig#sha1", "SHA-1" },
                    { "http://www.w3.org/2001/04/xmldsig-more#sha224", "SHA-224" },
                    { "http://www.w3.org/2001/04/xmlenc#sha256", "SHA-256" },
                    { "http://www.w3.org/2001/04/xmldsig-more#sha384", "SHA-384" },
                    { "http://www.w3.org/2001/04/xmlenc#sha512", "SHA-512" } 
                };

                IEnumerable<XElement> refElems = root.XPathSelectElements("//ds:Signature/ds:SignedInfo/ds:Reference", namespaceManager);

                if (refElems == null)
                {
                    throw new Exception("Chyba pri ziskavani elementu ds:Signature/ds:SignedInfo/ds:Reference. Element nebol v dokumente nájdený.");
                }

                foreach (XElement el in refElems)
                {
                    XmlElement refElement = (XmlElement)xmlDoc.ReadNode(el.CreateReader());
                    string uri = refElement.GetAttribute("URI").Substring(1);

                    XmlElement manifestElement = findByAttributeValue("ds:Manifest", "Id", uri, xmlDoc);
                    //System.Diagnostics.Debug.WriteLine(manifestElement);

                    if (manifestElement == null)
                    {
                        continue;
                    }

                    XmlElement digestValueElement = (XmlElement)refElement.GetElementsByTagName("ds:DigestValue").Item(0);
                    string expectedDigestValue = digestValueElement.InnerText;
                    XmlElement digestMethodElement = (XmlElement)refElement.GetElementsByTagName("ds:DigestMethod").Item(0);

                    if (Array.Exists(digestMethods, element => element == digestMethodElement.GetAttribute("Algorithm")) == false)
                    {

                        throw new Exception("Atribút Algorithm elementu ds:DigestMethod (" + digestMethodElement.GetAttribute("Algorithm") +") neobsahuje URI niektorého z podporovaných algoritmov");
                    }

                    string digestMethod = digestMethodElement.GetAttribute("Algorithm");

                    //System.Diagnostics.Debug.WriteLine(digestMethod);
                    digestMethod = digestAlgo[digestMethod];
                    //System.Diagnostics.Debug.WriteLine(digestMethod);

                    byte[] manifestElementBytes = null;

                    try
                    {
                        manifestElementBytes = System.Text.Encoding.UTF8.GetBytes(manifestElement.OuterXml);
                    }
                    catch (Exception e)
                    {

                        throw new Exception("Core validacia zlyhala. Chyba pri tranformacii z Element do String", e);
                    }

                    XmlNodeList transformsElements = manifestElement.GetElementsByTagName("ds:Transforms");

                    foreach (XmlElement transformsElement in transformsElements)
                    {
                        XmlElement transformElement = (XmlElement) transformsElement.GetElementsByTagName("ds:Transform").Item(0);
                        string transformMethod = transformElement.GetAttribute("Algorithm");

                        if (transformMethod.Equals("http://www.w3.org/TR/2001/REC-xml-c14n-20010315"))
                        {
                            try
                            {
                                XmlDsigC14NTransform xmlTransform = new XmlDsigC14NTransform();
                                xmlTransform.LoadInput(new MemoryStream(manifestElementBytes));
                                MemoryStream stream = (MemoryStream) xmlTransform.GetOutput();
                                manifestElementBytes = stream.ToArray();
                            }
                            catch (Exception e)
                            {

                                throw new Exception("Core validation zlyhala. Chyba pri kanonikalizacii", e);
                            }
                        }
                    }

                    HashAlgorithm hashAlgo = null;
                    switch (digestMethod)
                    {
                        case "SHA-1":
                            hashAlgo = SHA1.Create();
                            break;
                        case "SHA-256":
                            hashAlgo = SHA256.Create();
                            break;
                        case "SHA-384":
                            hashAlgo = SHA384.Create();
                            break;
                        case "SHA-512":
                            hashAlgo = SHA512.Create();
                            break;
                        default:
                            throw new Exception("Core validation zlyhala, neznamy algoritmus " + digestMethod);
                    }
                    string actualDigestValue = Convert.ToBase64String(hashAlgo.ComputeHash(manifestElementBytes));

                    if (expectedDigestValue.Equals(actualDigestValue) == false)
                    {
                        throw new Exception("Core validation zlyhala. Hodnota ds:DigestValue elementu ds:Reference sa nezhoduje s hash hodnotou elementu ds:Manifest");
                    }
                }

                //--------------------------------------------------------------------------------------------------------------------

                /*
                 * Element ds:Signature:
                 * 	- musí mať Id atribút,
                 * 	- musí mať špecifikovaný namespace xmlns:ds
                 */

                 XmlElement signatureElement = (XmlElement) xmlDoc.GetElementsByTagName("ds:Signature").Item(0);

                 if (signatureElement == null)
                 {
                    
                    throw new Exception("Element ds:Signature sa nenašiel");
                 }

                 if (signatureElement.HasAttribute("Id") == false)
                 {

                    throw new Exception("Element ds:Signature neobsahuje atribút Id");
                 }

                 if (signatureElement.GetAttribute("Id") == "" || signatureElement.GetAttribute("Id") == null)
                 {

                    throw new Exception("Atribút Id elementu ds:Signature neobsahuje žiadnu hodnotu");
                 }

                 if (signatureElement.GetAttribute("xmlns:ds").Equals("http://www.w3.org/2000/09/xmldsig#") == false)
                 {

                    throw new Exception("Element ds:Signature nemá nastavený namespace xmlns:ds");
                 }

                //--------------------------------------------------------------------------------------------------------------------

                /*
                 * Element ds:SignatureValue
                 * 	– musí mať Id atribút
                 */

                 XmlElement signatureValueElement = (XmlElement) xmlDoc.GetElementsByTagName("ds:SignatureValue").Item(0);

                 if (signatureValueElement == null)
                 {

                    throw new Exception("Element ds:SignatureValue sa nenašiel");
                 }

                 if (signatureValueElement.HasAttribute("Id") == false)
                 {

                    throw new Exception("Element ds:SignatureValue neobsahuje atribút Id");
                 }

                //--------------------------------------------------------------------------------------------------------------------

                /*
                 * Overenie existencie referencií v ds:SignedInfo a hodnôt atribútov Id a Type voči profilu XAdES_ZEP pre:
                 * 	- ds:KeyInfo element,
                 * 	- ds:SignatureProperties element,
                 * 	- xades:SignedProperties element,
                 * 	- všetky ostatné referencie v rámci ds:SignedInfo musia byť referenciami na ds:Manifest elementy
                 */

                IEnumerable<XElement> referencesElements = root.XPathSelectElements("//ds:Signature/ds:SignedInfo/ds:Reference", namespaceManager);

                if (referencesElements == null)
                {
                    throw new Exception("Chyba pri ziskavani elementu ds:Signature/ds:SignedInfo/ds:Reference. Element nebol v dokumente najdeny");
                }

                foreach (XElement el in referencesElements)
                {
                    XmlElement referenceElement = (XmlElement)xmlDoc.ReadNode(el.CreateReader());
                    string uri = referenceElement.GetAttribute("URI").Substring(1);
                    string actualType = referenceElement.GetAttribute("Type");

                    XElement referencedElement = null;
                    try
                    {
                        referencedElement = (XElement) root.XPathSelectElement(String.Format("//ds:Signature//*[@Id='{0}']", uri), namespaceManager);
                    }
                    catch (XPathException e)
                    {

                        throw new Exception("Chyba pri overeni existencie referencií v ds:SignedInfo. Chyba pri ziskavani elementu s Id " + uri);
                    }

                    if (referencedElement == null)
                    {
                        throw new Exception("Chyba pri overeni existencie referencií v ds: SignedInfo.Neexistuje element s Id: " + uri);
                    }

                    XmlElement helper = (XmlElement)xmlDoc.ReadNode(referencedElement.CreateReader());
                    string referencedElementName = helper.Name;

                    Dictionary<string, string> references = new Dictionary<string, string>() {
                        { "ds:KeyInfo", "http://www.w3.org/2000/09/xmldsig#Object" },
                        { "ds:SignatureProperties", "http://www.w3.org/2000/09/xmldsig#SignatureProperties" },
                        { "xades:SignedProperties", "http://uri.etsi.org/01903#SignedProperties" },
                        { "ds:Manifest", "http://www.w3.org/2000/09/xmldsig#Manifest" },
                    };

                    if (references.ContainsKey(referencedElementName) == false)
                    {
                        throw new Exception("Chyba pri overeni existencie referencií v ds:SignedInfo. Neznama referencia " + referencedElementName);
                    }

                    string expectedReferenceType = references[referencedElementName];

                    if (actualType.Equals(expectedReferenceType) == false)
                    {

                        throw new Exception("Chyba pri overeni zhody referencií v ds:SignedInfo. " + actualType + " sa nezhoduje s " + expectedReferenceType);
                    }

                    XElement keyInfoReferenceElement = null;
                    try
                    {
                        
                        keyInfoReferenceElement = (XElement) root.XPathSelectElement("//ds:Signature/ds:SignedInfo/ds:Reference[@Type='http://www.w3.org/2000/09/xmldsig#Object']", namespaceManager);
                    }
                    catch (Exception e)
                    {

                        throw new Exception("Chyba pri overeni existencie referencií v ds:SignedInfo." + "Chyba pri ziskavani elementu s Type http://www.w3.org/2000/09/xmldsig#Object", e);
                    }

                    if (keyInfoReferenceElement == null)
                    {

                        throw new Exception("Neexistuje referencia na ds:KeyInfo element v elemente ds:Reference");
                    }

                    XElement signaturePropertieReferenceElement = null;
                    try
                    {
                        signaturePropertieReferenceElement = (XElement) root.XPathSelectElement("//ds:Signature/ds:SignedInfo/ds:Reference[@Type='http://www.w3.org/2000/09/xmldsig#SignatureProperties']", namespaceManager);

                    }
                    catch (Exception e)
                    {
                        throw new Exception("Chyba pri overeni existencie referencií v ds:SignedInfo." + "Chyba pri ziskavani elementu s Type http://www.w3.org/2000/09/xmldsig#SignatureProperties", e);
                    }

                    if (signaturePropertieReferenceElement == null)
                    {

                        throw new Exception("Neexistuje referencia na ds:SignatureProperties element v elemente ds:Reference");
                    }

                    XElement signedInfoReferenceElement = null;
                    try
                    {
                        signedInfoReferenceElement = (XElement) root.XPathSelectElement("//ds:Signature/ds:SignedInfo/ds:Reference[@Type='http://uri.etsi.org/01903#SignedProperties']", namespaceManager);

                    }
                    catch (Exception e)
                    {
                        throw new Exception("Chyba pri overeni existencie referencií v ds:SignedInfo." + "Chyba pri ziskavani elementu s Type http://uri.etsi.org/01903#SignedProperties", e);
                    }

                    if (signedInfoReferenceElement == null)
                    {

                        throw new Exception("Neexistuje referencia na xades:SignedProperties element v elemente ds:Reference");
                    }


                }

                //--------------------------------------------------------------------------------------------------------------------

                /*
                 * Overenie obsahu ds:KeyInfo:
                 * 	- musí mať Id atribút,
                 * 	- musí obsahovať ds:X509Data, ktorý obsahuje elementy: ds:X509Certificate, ds:X509IssuerSerial, ds:X509SubjectName,
                 * 	- hodnoty elementov ds:X509IssuerSerial a ds:X509SubjectName súhlasia s príslušnými hodnatami v certifikáte,
                 * 	  ktorý sa nachádza v ds:X509Certificate
                 */

                XmlElement keyInfoElement = (XmlElement) xmlDoc.GetElementsByTagName("ds:KeyInfo").Item(0);

                if (keyInfoElement == null)
                {

                    throw new Exception("Element ds:Signature sa nenašiel");
                }

                if (keyInfoElement.HasAttribute("Id") == false)
                {

                    throw new Exception("Element ds:Signature neobsahuje atribút Id");
                }

                if (keyInfoElement.GetAttribute("Id") == "" || keyInfoElement.GetAttribute("Id") == null)
                {

                    throw new Exception("Atribút Id elementu ds:Signature neobsahuje žiadnu hodnotu");
                }

                XmlElement xDataElement = (XmlElement) keyInfoElement.GetElementsByTagName("ds:X509Data").Item(0);

                if (xDataElement == null)
                {

                    throw new Exception("Element ds:KeyInfo neobsahuje element ds:X509Data");
                }

                XmlElement xCertificateElement = (XmlElement)xDataElement.GetElementsByTagName("ds:X509Certificate").Item(0);
                XmlElement xIssuerSerialElement = (XmlElement)xDataElement.GetElementsByTagName("ds:X509IssuerSerial").Item(0);
                XmlElement xSubjectNameElement = (XmlElement)xDataElement.GetElementsByTagName("ds:X509SubjectName").Item(0);

                if (xCertificateElement == null)
                {

                    throw new Exception("Element ds:X509Data neobsahuje element ds:X509Certificate");
                }

                if (xIssuerSerialElement == null)
                {

                    throw new Exception("Element ds:X509Data neobsahuje element ds:X509IssuerSerial");
                }

                if (xSubjectNameElement == null)
                {

                    throw new Exception("Element ds:X509Data neobsahuje element ds:X509SubjectName");
                }

                XmlElement xIssuerNameElement = (XmlElement) xIssuerSerialElement.GetElementsByTagName("ds:X509IssuerName").Item(0);
                XmlElement xSerialNumberElement = (XmlElement) xIssuerSerialElement.GetElementsByTagName("ds:X509SerialNumber").Item(0);

                if (xIssuerNameElement == null)
                {

                    throw new Exception("Element ds:X509IssuerSerial neobsahuje element ds:X509IssuerName");
                }

                if (xSerialNumberElement == null)
                {

                    throw new Exception("Element ds:X509IssuerSerial neobsahuje element ds:X509SerialNumber");
                }

                X509Certificate certificateKeyInfo = null;
                try
                {
                    certificateKeyInfo = getCertificate(xmlDoc);

                }
                catch (Exception e)
                {

                    throw new Exception("X509 certifikát sa v dokumente nepodarilo nájsť", e);
                }

                System.Security.Cryptography.X509Certificates.X509Certificate cert = new System.Security.Cryptography.X509Certificates.X509Certificate(certificateKeyInfo.GetEncoded());
                String certifIssuerName = cert.Issuer;//.replaceAll("ST=", "S=")
                String certifSerialNumber = certificateKeyInfo.SerialNumber.ToString();
                String certifSubjectName = cert.Subject;

                //System.Diagnostics.Debug.WriteLine(certifIssuerName);

                if (xIssuerNameElement.FirstChild.Value.Equals(certifIssuerName) == false)
                {

                    throw new Exception("Element ds:X509IssuerName sa nezhoduje s hodnotou na certifikáte");
                }

                if (xSerialNumberElement.FirstChild.Value.Equals(certifSerialNumber) == false)
                {

                    throw new Exception("Element ds:X509SerialNumber sa nezhoduje s hodnotou na certifikáte");
                }

                if (xSubjectNameElement.FirstChild.Value.Equals(certifSubjectName) == false)
                {

                    throw new Exception("Element ds:X509SubjectName neobsahuje element ds:X509SerialNumber");
                }

                //--------------------------------------------------------------------------------------------------------------------

                /*
                 * Overenie obsahu ds:SignatureProperties:
                 * 	- musí mať Id atribút,
                 * 	- musí obsahovať dva elementy ds:SignatureProperty pre xzep:SignatureVersion a xzep:ProductInfos,
                 * 	- obidva ds:SignatureProperty musia mať atribút Target nastavený na ds:Signature
                 */

                XmlElement signaturePropertiesElement = (XmlElement) xmlDoc.GetElementsByTagName("ds:SignatureProperties").Item(0);

                if (signaturePropertiesElement == null)
                {
                    throw new Exception("Element ds:SignatureProperties sa nenašiel");
                }

                if (signaturePropertiesElement.HasAttribute("Id") == false)
                {
                    throw new Exception("Element ds:SignatureProperties neobsahuje atribút Id");
                }

                if (signaturePropertiesElement.GetAttribute("Id") == "" || signaturePropertiesElement.GetAttribute("Id") == null)
                {
                    throw new Exception("Atribút Id elementu ds:SignatureProperties neobsahuje žiadnu hodnotu");
                }

                XmlElement signatureVersionElement = null;
                XmlElement productInfosElement = null;

                XmlNodeList sigPropsElement = signaturePropertiesElement.GetElementsByTagName("ds:SignatureProperty");

                foreach (XmlElement tempElement in sigPropsElement)
                {

                    if (tempElement != null)
                    {

                        XmlElement tempElement2 = (XmlElement) tempElement.GetElementsByTagName("xzep:SignatureVersion").Item(0);

                        if (tempElement2 != null)
                        {
                            signatureVersionElement = tempElement2;
                        }

                        else
                        {
                            tempElement2 = (XmlElement) tempElement.GetElementsByTagName("xzep:ProductInfos").Item(0);

                            if (tempElement != null)
                            {
                                productInfosElement = tempElement2;
                            }
                        }
                    }
                }

                if (signatureVersionElement == null)
                {

                    throw new Exception("ds:SignatureProperties neobsahuje taký element ds:SignatureProperty, ktorý by obsahoval element xzep:SignatureVersion");

                }

                if (productInfosElement == null)
                {
                    throw new Exception("ds:SignatureProperties neobsahuje taký element ds:SignatureProperty, ktorý by obsahoval element xzep:ProductInfos");
                }

                XmlElement signature = (XmlElement) xmlDoc.GetElementsByTagName("ds:Signature").Item(0);

                if (signature == null)
                {
                    throw new Exception("Element ds:Signature sa nenašiel");
                }

                String signatureId = signature.GetAttribute("Id");
                XmlElement sigVerParentElement = (XmlElement) signatureVersionElement.ParentNode;
                XmlElement pInfoParentElement = (XmlElement) productInfosElement.ParentNode;
                String targetSigVer = sigVerParentElement.GetAttribute("Target");
                String targetPInfo = pInfoParentElement.GetAttribute("Target");

                if (targetSigVer.Equals("#" + signatureId) == false)
                {
                    throw new Exception("Atribút Target elementu xzep:SignatureVersion sa neodkazuje na daný ds:Signature");
                }

                if (targetPInfo.Equals("#" + signatureId) == false)
                {
                    throw new Exception("Atribút Target elementu xzep:ProductInfos sa neodkazuje na daný ds:Signature");
                }

                //--------------------------------------------------------------------------------------------------------------------

                /*
                 * Overenie ds:Manifest elementov:
                 * 	- každý ds:Manifest element musí mať Id atribút,
                 * 	- ds:Transforms musí byť z množiny podporovaných algoritmov pre daný element podľa profilu XAdES_ZEP,
                 * 	- ds:DigestMethod – musí obsahovať URI niektorého z podporovaných algoritmov podľa profilu XAdES_ZEP,
                 * 	- overenie hodnoty Type atribútu voči profilu XAdES_ZEP,
                 * 	- každý ds:Manifest element musí obsahovať práve jednu referenciu na ds:Object
                 */

                IEnumerable<XElement> manifestElems = root.XPathSelectElements("//ds:Signature/ds:Object/ds:Manifest", namespaceManager);

                if (manifestElems == null)
                {
                    throw new Exception("Chyba pri hladaní ds:Manifest elementov v dokumente.");
                }

                foreach (XElement manifestElem in manifestElems)
                {
                    /*
                     * každý ds:Manifest element musí mať Id atribút,
                     */

                    XmlElement manElement = (XmlElement)xmlDoc.ReadNode(manifestElem.CreateReader());

                    if (manElement.HasAttribute("Id") == false)
                    {
                        throw new Exception("Element ds:Manifest nema atribut Id");
                    }

                    IEnumerable<XElement> referElements = manifestElem.XPathSelectElements("ds:Reference", namespaceManager);

                    if (referElements == null)
                    {
                        throw new Exception("Chyba pri hladanii ds:Reference elementov v ds:Manifest elemente");
                    }

                    /*
                     * každý ds:Manifest element musí obsahovať práve jednu referenciu na ds:Object
                     */
                    if (referElements.Count() != 1)
                    {
                        throw new Exception("ds:Manifest element neobsahuje prave jednu referenciu na objekt");
                    }
                }

                IEnumerable<XElement> referenceElements = root.XPathSelectElements("//ds:Signature/ds:Object/ds:Manifest/ds:Reference", namespaceManager);

                if (referenceElements == null)
                {
                    throw new Exception("Chyba pri hladani ds:Reference elementov v dokumente.");
                }

                foreach(XElement referenceElement in referenceElements)
                {
                    IEnumerable<XElement> transformElems = root.XPathSelectElements("ds:Transforms/ds:Transform", namespaceManager);

                    if (transformElems == null)
                    {
                        throw new Exception("Chyba pri hladani ds:Transform elementov v dokumente.");
                    }

                    foreach(XElement transformElem in transformElems)
                    {
                        string[] manifestMethods = {
                                                    "http://www.w3.org/TR/2001/REC-xml-c14n-20010315",
                                                    "http://www.w3.org/2000/09/xmldsig#base64",
                                                    };
                        /*
                         * ds:Transforms musí byť z množiny podporovaných algoritmov pre daný element podľa profilu XAdES_ZEP
                         */
                        if (Array.Exists(manifestMethods, element => element == transformElem.Attribute("Algorithm").Value) == false)
                        {

                            throw new Exception("Element ds:Transform obsahuje nepovoleny typ algoritmu");
                        }
                    }

                    XElement digestMethodElement = referenceElement.XPathSelectElement("ds:DigestMethod", namespaceManager);

                    if (digestMethodElement == null)
                    {
                        throw new Exception("Chyba pri hladani ds:DigestMethod elementov v dokumente");
                    }

                    /*
                     * ds:DigestMethod – musí obsahovať URI niektorého z podporovaných algoritmov podľa profilu XAdES_ZEP
                     */
                    if (Array.Exists(digestMethods, element => element == digestMethodElement.Attribute("Algorithm").Value) == false)
                    {
                        throw new Exception("Atribút Algorithm elementu ds:DigestMethod neobsahuje URI niektorého z podporovaných algoritmov");
                    }

                    /*
                     * overenie hodnoty Type atribútu voči profilu XAdES_ZEP
                     */
                    if (referenceElement.Attribute("Type").Value.Equals("http://www.w3.org/2000/09/xmldsig#Object") == false)
                    {
                        throw new Exception("Atribút Type elementu ds:Reference neobsahuje hodnotu http://www.w3.org/2000/09/xmldsig#Object");
                    }

                }

                //--------------------------------------------------------------------------------------------------------------------

                TimeStampToken ts_token = null;

                XElement timestamp = null;

                timestamp = (XElement) root.XPathSelectElement("//xades:EncapsulatedTimeStamp", namespaceManager);

                if (timestamp == null)
                {
                    throw new Exception("Dokument neobsahuje casovu peciatku.");
                }

                ts_token = new TimeStampToken(new CmsSignedData(Base64.Decode(timestamp.Value)));

                //WebClient webClient = new WebClient();
                //webClient.DownloadFile("http://test.ditec.sk/TSAServer/crl/dtctsa.crl", Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "//dtctsa.crl");
                //string crlData = System.IO.File.ReadAllText(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "//DTCCACrl.crl");

                /*
                 * Overenie platnosti podpisového certifikátu časovej pečiatky voči času UtcNow
                 * a voči platnému poslednému CRL
                 */

                X509CrlParser crlParser = new X509CrlParser();
                X509Crl crl = crlParser.ReadCrl(System.IO.File.ReadAllBytes(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "//dtctsa.crl"));

                X509Certificate signerCert = null;
                IX509Store x509Certs = ts_token.GetCertificates("Collection");
                ArrayList certs = new ArrayList(x509Certs.GetMatches(null));

                // nájdenie podpisového certifikátu tokenu v kolekcii
                foreach (X509Certificate certifikat in certs)
                {
                    string cerIssuerName = certifikat.IssuerDN.ToString(true, new Hashtable());
                    string signerIssuerName = ts_token.SignerID.Issuer.ToString(true, new Hashtable());

                    // kontrola issuer name a seriového čísla
                    if (cerIssuerName == signerIssuerName &&
                        certifikat.SerialNumber.Equals(ts_token.SignerID.SerialNumber))
                    {
                        signerCert = certifikat;
                        //System.Diagnostics.Debug.WriteLine(certifikat.SerialNumber);
                        break;
                    }
                }

                if (signerCert == null)
                {
                    throw new Exception("V dokumente sa nenachadza certifikat casovej peciatky.");
                }

                if (!signerCert.IsValidNow)
                {
                    throw new Exception("Podpisový certifikát časovej pečiatky nie je platný voči aktuálnemu času.");
                }

                if (crl.GetRevokedCertificate(signerCert.SerialNumber) != null)
                {
                    throw new Exception("Podpisový certifikát časovej pečiatky nie je platný voči platnému poslednému CRL.");
                }

                /*
                 * Overenie MessageImprint z časovej pečiatky voči podpisu ds:SignatureValue
                 */

                byte[] messageImprint = ts_token.TimeStampInfo.GetMessageImprintDigest();
                String hashAlg = ts_token.TimeStampInfo.HashAlgorithm.Algorithm.Id;

                XElement signatureValueNode = null;

                signatureValueNode = root.XPathSelectElement("//ds:Signature/ds:SignatureValue", namespaceManager);

                if (signatureValueNode == null)
                {
                    throw new Exception("Element ds:SignatureValue nenájdený.");
                }

                XmlElement sigValueNode = (XmlElement)xmlDoc.ReadNode(signatureValueNode.CreateReader());

                byte[] signatureValue = Encoding.UTF8.GetBytes(sigValueNode.InnerText);


                Dictionary<string, string> algorithms = new Dictionary<string, string>() {
                    { OiwObjectIdentifiers.IdSha1.Id, "SHA-1"},
                    { NistObjectIdentifiers.IdSha256.Id, "SHA-256"},
                    { NistObjectIdentifiers.IdSha384.Id, "SHA-384"},
                    { NistObjectIdentifiers.IdSha512.Id, "SHA-512"},
                    };

                string digMethod = algorithms[hashAlg];

                HashAlgorithm hashTAlgo = null;
                switch (digMethod)
                {
                    case "SHA-1":
                        hashTAlgo = SHA1.Create();
                        break;
                    case "SHA-256":
                        hashTAlgo = SHA256.Create();
                        break;
                    case "SHA-384":
                        hashTAlgo = SHA384.Create();
                        break;
                    case "SHA-512":
                        hashTAlgo = SHA512.Create();
                        break;
                    default:
                        throw new Exception("Core validation zlyhala, neznamy algoritmus " + digMethod);
                }

                byte[] comparisonVal = hashTAlgo.ComputeHash(signatureValue, 0, signatureValue.Length);

                if (messageImprint.Equals(comparisonVal)){
			        throw new Exception("MessageImprint z časovej pečiatky a podpis ds:SignatureValue sa nezhodujú.");
		        }


                //--------------------------------------------------------------------------------------------------------------------

                /*
                 * Overenie platnosti podpisového certifikátu dokumentu voči času T z časovej pečiatky
                 * a voči platnému poslednému CRL
                 */

                X509Crl crl2 = crlParser.ReadCrl(System.IO.File.ReadAllBytes(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "//DTCCACrl.crl"));

                XElement certificateNode = null;

                certificateNode = root.XPathSelectElement("//ds:Signature/ds:KeyInfo/ds:X509Data/ds:X509Certificate", namespaceManager);

                if (certificateNode == null)
                {
                    throw new Exception("Element ds:X509Certificate nenájdený.");
                }

                X509Certificate signCert = null;
                Asn1InputStream asn1is = null;

                try
                {
                    asn1is = new Asn1InputStream(new MemoryStream(Encoding.UTF8.GetBytes(certificateNode.Value)));
                    System.Diagnostics.Debug.WriteLine(certificateNode.Value);
                    Asn1Sequence sq = (Asn1Sequence) Asn1Object.FromStream(asn1is).ToAsn1Object();//asn1is.ReadObject()
                    System.Diagnostics.Debug.WriteLine("WTFFF");
                    signCert = new X509Certificate(X509CertificateStructure.GetInstance(sq));
                }
                catch (Exception e) {
                    throw new Exception("OJOJ" + e);
                } finally
                {
                    if (asn1is != null)
                    {
                        try
                        {
                            asn1is.Close();
                        }
                        catch (Exception e)
                        {
                            throw new Exception("Nie je možné prečítať certifikát dokumentu.");
                        }
                    }
                }

                try
                {
                    signCert.CheckValidity(ts_token.TimeStampInfo.GenTime);
                }
                catch (CertificateExpiredException e)
                {
                    throw new CertificateExpiredException("Certifikát dokumentu bol pri podpise expirovaný.");
                }
                catch (CertificateNotYetValidException e)
                {
                    throw new CertificateNotYetValidException("Certifikát dokumentu ešte nebol platný v čase podpisovania.");
                }

                X509CrlEntry entry = crl.GetRevokedCertificate(signCert.SerialNumber);
                if (entry != null && entry.RevocationDate < (ts_token.TimeStampInfo.GenTime))
                {
                    throw new Exception("Certifikát bol zrušený v čase podpisovania.");
                }

                //--------------------------------------------------------------------------------------------------------------------

                /*
                 * Overenie referencií v elementoch ds:Manifest:
                 * 	- dereferencovanie URI, aplikovanie príslušnej ds:Transforms transformácie (pri base64 decode),
                 * 	- overenie hodnoty ds:DigestValue
                 */

                IEnumerable<XElement> manRefElements = root.XPathSelectElements("//ds:Signature/ds:Object/ds:Manifest/ds:Reference", namespaceManager);

                if (manRefElements == null)
                {
                    throw new Exception("Chyba pri hladanii ds:Reference elementov v dokumente");
                }

                for (int i = 0; i < manRefElements.Count(); i++)
                {
                    XElement refElem = (XElement) manRefElements.ElementAt(i);
                    XmlElement referElement = (XmlElement) xmlDoc.ReadNode(refElem.CreateReader());
                    String uri = referElement.GetAttribute("URI").Substring(1);

                    XmlElement objectElement = findByAttributeValue("ds:Object", "Id", uri, xmlDoc);

                    XmlElement digestValElement = (XmlElement) referElement.GetElementsByTagName("ds:DigestValue").Item(0);
                    XmlElement digestMethodlement = (XmlElement) referElement.GetElementsByTagName("ds:DigestMethod").Item(0);

                    String digestMethod = digestMethodlement.GetAttribute("Algorithm");
                    digestMethod = digestAlgo[digestMethod];

                    XmlNodeList transfsElements = referElement.GetElementsByTagName("ds:Transforms");

                    for (int j = 0; j < transfsElements.Count; j++)
                    {
                        XmlElement transfsElement = (XmlElement)transfsElements.Item(j);
                        XmlElement transfElement = (XmlElement) transfsElement.GetElementsByTagName("ds:Transform").Item(j);

                        String transfMethod = transfElement.GetAttribute("Algorithm");
                        byte[] objectElementBytes = System.Text.Encoding.UTF8.GetBytes(objectElement.OuterXml);

                        if (objectElementBytes == null)
                        {
                            throw new Exception("Overenie referencií v elementoch ds:Manifest zlyhalo. Chyba pri tranformacii z Element do String");
                        }

                        MemoryStream str = new MemoryStream(objectElementBytes);

                        if ("http://www.w3.org/TR/2001/REC-xml-c14n-20010315".Equals(transfMethod))
                        {
                            try
                            {
                                XmlDsigC14NTransform xxmlTransform = new XmlDsigC14NTransform();
                                xxmlTransform.LoadInput(str);
                                MemoryStream stream = (MemoryStream)xxmlTransform.GetOutput();
                                objectElementBytes = stream.ToArray();

                            }
                            catch (Exception e) {

                                throw new Exception("Core validation zlyhala. Chyba pri kanonikalizacii", e);
                            }
                        }

                        if ("http://www.w3.org/2000/09/xmldsig#base64".Equals(transfMethod))
                        {
                            objectElementBytes = Base64.Decode(objectElementBytes);
                        }


                        HashAlgorithm messageDigest = null;
                        switch (digestMethod)
                        {
                            case "SHA-1":
                                messageDigest = SHA1.Create();
                                break;
                            case "SHA-256":
                                messageDigest = SHA256.Create();
                                break;
                            case "SHA-384":
                                messageDigest = SHA384.Create();
                                break;
                            case "SHA-512":
                                messageDigest = SHA512.Create();
                                break;
                            default:
                                throw new Exception("Core validation zlyhala, neznamy algoritmus " + digestMethod);
                        }

                        string actDigestValue = Convert.ToBase64String(messageDigest.ComputeHash(objectElementBytes));
                        System.Diagnostics.Debug.WriteLine(digestMethod);
                        String expectedDigestVal = digestValElement.InnerText;
                        System.Diagnostics.Debug.WriteLine(expectedDigestVal);

                        if (expectedDigestVal.Equals(actDigestValue) == false)
                        {
                            throw new Exception("Hodnota ds:DigestValue elementu ds:Reference sa nezhoduje s hash hodnotou elementu ds:Manifest.");
                        }
                    }
                }

                //--------------------------------------------------------------------------------------------------------------------

                /*
                 * Core validation (podľa špecifikácie XML Signature)
                 * Kanonikalizácia ds:SignedInfo a overenie hodnoty ds:SignatureValue
                 * pomocou pripojeného podpisového certifikátu v ds:KeyInfo
                 */

                signatureElement = (XmlElement) xmlDoc.GetElementsByTagName("ds:Signature").Item(0);

                XmlElement signedInfoElement = (XmlElement) signatureElement.GetElementsByTagName("ds:SignedInfo").Item(0);
                XmlElement canonicalizationMethodElement = (XmlElement) signedInfoElement.GetElementsByTagName("ds:CanonicalizationMethod").Item(0);

                Dictionary<string, string> signAlgo = new Dictionary<string, string>() {
                    { "http://www.w3.org/2000/09/xmldsig#dsa-sha1", "SHA1withDSA" },
                    { "http://www.w3.org/2000/09/xmldsig#rsa-sha1", "SHA1withRSA/ISO9796-2" },
                    { "http://www.w3.org/2001/04/xmldsig-more#rsa-sha256", "SHA256withRSA" },
                    { "http://www.w3.org/2001/04/xmldsig-more#rsa-sha384", "SHA384withRSA" },
                    { "http://www.w3.org/2001/04/xmldsig-more#rsa-sha512", "SHA512withRSA" }
                };

                XmlElement signatureMethodElement = (XmlElement) signedInfoElement.GetElementsByTagName("ds:SignatureMethod").Item(0);
                signatureValueElement = (XmlElement) signatureElement.GetElementsByTagName("ds:SignatureValue").Item(0);

                byte[] signedInfoElementBytes = null;
                try
                {
                    signedInfoElementBytes = System.Text.Encoding.Default.GetBytes(signedInfoElement.OuterXml);
                }
                catch (Exception e)
                {

                    throw new Exception("Core validation zlyhala. Chyba pri tranformacii z Element do String", e);
                }

                string canonicalizationMethod = canonicalizationMethodElement.GetAttribute("Algorithm");

                if (canonicalizationMethod.Equals("http://www.w3.org/TR/2001/REC-xml-c14n-20010315"))
                {
                    try
                    {
                        XmlDsigC14NTransform xmlTransform = new XmlDsigC14NTransform();
                        xmlTransform.LoadInput(new MemoryStream(signedInfoElementBytes));
                        MemoryStream stream = (MemoryStream)xmlTransform.GetOutput();
                        signedInfoElementBytes = stream.ToArray();
                    }
                    catch (Exception e)
                    {

                        throw new Exception("Core validation zlyhala. Chyba pri kanonikalizacii", e);
                    }
                }

                X509Certificate certificate = null;

                try
                {
                    certificate = getCertificate(xmlDoc);

                }
                catch (Exception e)
                {

                    throw new Exception("X509 certifikát sa v dokumente nepodarilo nájsť", e);
                }

                string signatureMethod = signatureMethodElement.GetAttribute("Algorithm");
                signatureMethod = signAlgo[signatureMethod];
                ISigner signer = null;

                try
                {
                    signer = SignerUtilities.GetSigner(signatureMethod);
                    signer.Init(false, certificate.GetPublicKey());
                    signer.BlockUpdate(signedInfoElementBytes, 0, signedInfoElementBytes.Length);

                }
                catch (Exception e) {

                    throw new Exception("Core validation zlyhala. Chyba pri inicializacii prace s digitalnym podpisom", e);
                }

                byte[] signatureValueBytes = System.Text.Encoding.UTF8.GetBytes(signatureValueElement.OuterXml);

                bool verificationResult = false;

                try
                {
                    verificationResult = signer.VerifySignature(signatureValueBytes);

                }
                catch (SignatureException e)
                {

                    throw new Exception("Core validation zlyhala. Chyba pri verifikacii digitalneho podpisu", e);
                }

                if (verificationResult == false)
                {

                    throw new Exception("Podpisana hodnota ds:SignedInfo sa nezhoduje s hodnotou v elemente ds:SignatureValue");
                }


                TempData["xadesResult"] = "XML subor validny!";
                return RedirectToActionPermanent("Result");
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
                TempData["xadesResult"] = "Datova obalka chybna!" + ex.Message;
                return RedirectToActionPermanent("Result");
            }

            byte[] data = System.Text.Encoding.UTF8.GetBytes(input);

            XmlElement findByAttributeValue(string elType, string attributeName, string attributeValue, XmlDocument xmlDoc)
            {

                XmlNodeList elements = xmlDoc.GetElementsByTagName(elType);

                for (int i = 0; i < elements.Count; i++)
                {

                    XmlElement element = (XmlElement)elements.Item(i);

                    if (element.HasAttribute(attributeName) && element.GetAttribute(attributeName).Equals(attributeValue))
                    {
                        return element;
                    }
                }

                return null;
            }

            X509Certificate getCertificate(XmlDocument xmlDoc) {

                XmlElement keyInfoElement = (XmlElement)xmlDoc.GetElementsByTagName("ds:KeyInfo").Item(0);

                if (keyInfoElement == null)
                {
                    throw new Exception("Chyba pri ziskavani certifikatu: Dokument neobsahuje element ds:KeyInfo");
                }

                XmlElement x509DataElement = (XmlElement)keyInfoElement.GetElementsByTagName("ds:X509Data").Item(0);

                if (x509DataElement == null)
                {
                    throw new Exception("Chyba pri ziskavani certifikatu: Dokument neobsahuje element ds:X509Data");
                }

                XmlElement x509Certificate = (XmlElement)x509DataElement.GetElementsByTagName("ds:X509Certificate").Item(0);

                if (x509Certificate == null)
                {
                    throw new Exception("Chyba pri ziskavani certifikatu: Dokument neobsahuje element ds:X509Certificate");
                }

                X509Certificate certObject = null;
                Asn1InputStream inputStream = null;

                try
                {
                    inputStream = new Asn1InputStream(new MemoryStream(Convert.FromBase64String(x509Certificate.InnerText)));
                    Asn1Sequence sequence = (Asn1Sequence)inputStream.ReadObject();
                    certObject = new X509Certificate(X509CertificateStructure.GetInstance(sequence));

                }
                catch (Exception e) {

                    throw new Exception("Certifikát nebolo možné načítať", e);

                }
                finally
                {
                    if (inputStream != null)
                    {
                        inputStream.Close();
                    }
                }

                return certObject;
            }

            //Overenie casovej peciatky

            //Overenie platnosti podpisovaneho certifikatu
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
            //nacitanie xadesu
            XmlDocument doc = new XmlDocument();
            string xmlData = System.IO.File.ReadAllText(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "//xades.xml");
            doc.Load(new StringReader(xmlData));

            //getnutie elementu SignatureValue a prevedenie do base64
            string signatureValue = doc.GetElementsByTagName("ds:SignatureValue")[0].OuterXml.ToString();
            var signatureBytes = System.Text.Encoding.UTF8.GetBytes(signatureValue);
            string dataB64 = Convert.ToBase64String(signatureBytes);
            
            
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

            if (response.Status == 0)
            {
                //pridanie namespacu, vdaka ktorevu viem gettovat elementy s predponou xades
                XmlNamespaceManager nsmgr = new XmlNamespaceManager(doc.NameTable);
                nsmgr.AddNamespace("xades", "http://uri.etsi.org/01903/v1.3.2#");

                //prevedenie tokenu do Base64
                string tokenB64 = Convert.ToBase64String(response.TimeStampToken.GetEncoded());

                //pridanie struktury nodeov do xadesu
                XmlNode QualifyingProperties = doc.SelectSingleNode("//xades:QualifyingProperties", nsmgr);
                XmlNode SignedProperties = doc.SelectSingleNode("//xades:SignedProperties", nsmgr);

                XmlNode UnsignedProperties = doc.CreateNode(XmlNodeType.Element, "xades", "UnsignedProperties", "http://uri.etsi.org/01903/v1.3.2#");
                QualifyingProperties.InsertAfter(UnsignedProperties, SignedProperties);

                XmlNode UnsignedSignatureProperties = doc.CreateNode(XmlNodeType.Element, "xades", "UnsignedSignatureProperties", "http://uri.etsi.org/01903/v1.3.2#");
                UnsignedProperties.AppendChild(UnsignedSignatureProperties);

                XmlNode SignatureTimeStamp = doc.CreateNode(XmlNodeType.Element, "xades",  "SignatureTimeStamp", "http://uri.etsi.org/01903/v1.3.2#");
                UnsignedSignatureProperties.AppendChild(SignatureTimeStamp);

                //pridanie id a tokenu do elementu EncapsulatedTimeStamp
                XmlNode EncapsulatedTimeStamp = doc.CreateNode(XmlNodeType.Element, "xades", "EncapsulatedTimeStamp", "http://uri.etsi.org/01903/v1.3.2#");
                EncapsulatedTimeStamp.InnerText = tokenB64;
                var idAttr = doc.CreateAttribute("id");
                idAttr.Value = "TimeStampID";
                EncapsulatedTimeStamp.Attributes.Append(idAttr);

                SignatureTimeStamp.AppendChild(EncapsulatedTimeStamp);

                System.IO.File.WriteAllText(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "//xades-t.xml", doc.OuterXml.ToString());
                ViewBag.soapResult = "Bola vygenerovaná časová pečiatka do súboru /Documents/xades-t.xml";
            }
            else if (response.Status == 1)
            {
                // ak nam responce vrati status 1 tak bol podpi poskodeny
                ViewBag.soapResult = "niekto ti poškodil podpis";
            }
            else
            {
                ViewBag.soapResult = "nepodarilo sa podpisat";
            }

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

        private static void ValidationEventHandler(object sender, ValidationEventArgs e)
        {
            switch (e.Severity)
            {
                case XmlSeverityType.Error:
                    Console.WriteLine("Error: {0}", e.Message);
                    break;
                case XmlSeverityType.Warning:
                    Console.WriteLine("Warning {0}", e.Message);
                    break;
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

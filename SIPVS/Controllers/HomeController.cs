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
using System.Xml.XPath;
using System.Xml.Serialization;
using System.Security;
using System.Security.Cryptography.Xml;
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
                    string expectedDigestValue = digestValueElement.InnerText;//getTextContent()
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
                        manifestElementBytes = System.Text.Encoding.Default.GetBytes(manifestElement.OuterXml);

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
                    signedInfoElementBytes = System.Text.Encoding.UTF8.GetBytes(signedInfoElement.OuterXml);
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

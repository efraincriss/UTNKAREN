using Abp.Runtime.Security;
using com.cpp.calypso.comun.aplicacion;
using com.cpp.calypso.comun.dominio;
using com.cpp.calypso.framework;
using com.cpp.calypso.proyecto.aplicacion.Dto;
using com.cpp.calypso.proyecto.aplicacion.WebService;
using com.cpp.calypso.proyecto.dominio;
using com.cpp.calypso.proyecto.dominio.Accesos;
using com.cpp.calypso.proyecto.dominio.Constantes;
using com.cpp.calypso.proyecto.dominio.Models;
using com.cpp.calypso.proyecto.dominio.WebService;
using Microsoft.Office.Interop.Word;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;

namespace com.cpp.calypso.proyecto.aplicacion
{
    public class WebServiceAsyncBaseCrudAppService : AsyncBaseCrudAppService<ServicioWeb, ServicioWebDto, PagedAndFilteredResultRequestDto>,
           IWebServiceAsyncBaseCrudAppService
    {
        private readonly IBaseRepository<AccionServicioWeb> _accionrepository;
        private readonly IBaseRepository<ParametroServicioWeb> _parametrorepository;
        private readonly IBaseRepository<ConsultaPublica> _consultaPublicaRepository;
        private readonly IBaseRepository<Catalogo> _catalogoRepository;
        public WebServiceAsyncBaseCrudAppService(
            IBaseRepository<ServicioWeb> repository,
            IBaseRepository<AccionServicioWeb> accionrepository,
            IBaseRepository<ParametroServicioWeb> parametrorepository,
             IBaseRepository<ConsultaPublica> consultaPublicaRepository,
              IBaseRepository<Catalogo> catalogoRepository
            ) : base(repository)
        {
            _accionrepository = accionrepository;
            _parametrorepository = parametrorepository;
            _consultaPublicaRepository = consultaPublicaRepository;
            _catalogoRepository = catalogoRepository;
        }


        public XmlNode BusquedaPorCedulaRegistroCivil(string cedula)
        {
            //codigo registro civil base de datos

            string codigo_webservice = "_registrocivil";
            //Busqueda de Werb service por codigo

            var webservice = Repository.GetAll()
                                                .Where(c => c.vigente)
                                                .FirstOrDefault(c => c.codigo == codigo_webservice);

            var accion = _accionrepository.GetAllIncluding(c => c.ServicioWeb)
                                               .Where(c => c.vigente)
                                               .Where(c => c.codigo == "BusquedaporNui")
                                               .FirstOrDefault(c => c.ServicioWebId == webservice.Id);

            //cREO EL cUERPO SOAP XML
            XmlDocument soapEnvelopeXml = CreateSoapEnvelope(codigo_webservice, cedula);
            ServicePointManager.Expect100Continue = true;
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls
                   | SecurityProtocolType.Tls11
                   | SecurityProtocolType.Tls12
                   | SecurityProtocolType.Ssl3;


            //se crea la peticion
            HttpWebRequest webRequest = CreateWebRequest(webservice.url, accion.nombre_accion);
            /*IWebProxy proxyObject = new WebProxy("proxyec01.teic.techint.net", 8080);

            proxyObject.Credentials = new NetworkCredential("tcuwri", "wiljack22wr33");
            webRequest.Proxy = proxyObject;
            */

            var parameter = _parametrorepository.GetAll().Where(c => c.codigo == "HOST.TECHINT").FirstOrDefault();
            if (parameter != null && parameter.Id > 0)
            {
                webRequest.Host = parameter.valor;
                ElmahExtension.LogToElmah(new Exception("HOST CONSUMO WS:" + parameter.valor));
            }
            InsertSoapEnvelopeIntoWebRequest(soapEnvelopeXml, webRequest);


            // iniciar  llamada async a solicitud web.
            IAsyncResult asyncResult = webRequest.BeginGetResponse(null, null);




            // suspende este hilo hasta que se complete la llamada. Tu podrías querer
            // haz algo útil aquí como actualizar tu interfaz de usuario
            asyncResult.AsyncWaitHandle.WaitOne();


            // obtener la respuesta de la solicitud web completada.
            string soapResult;
            using (WebResponse webResponse = webRequest.EndGetResponse(asyncResult))
            {
                using (StreamReader rd = new StreamReader(webResponse.GetResponseStream()))
                {
                    soapResult = rd.ReadToEnd();
                }

            }
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(soapResult);
            XmlNodeList node = doc.GetElementsByTagName("return");

            return node[0];
        }

        public HttpWebRequest CreateWebRequest(string url, string action)
        {

            HttpWebRequest webRequest = (HttpWebRequest)WebRequest.Create(url);
            webRequest.Headers.Add("SOAPAction", action);
            webRequest.ContentType = "text/xml;charset=\"utf-8\"";
            webRequest.Accept = "text/xml";
            webRequest.Method = "POST";
            return webRequest;
        }
        private static void InsertSoapEnvelopeIntoWebRequest(XmlDocument soapEnvelopeXml, HttpWebRequest webRequest)
        {
            using (Stream stream = webRequest.GetRequestStream())
            {
                soapEnvelopeXml.Save(stream);
            }
        }
        private XmlDocument CreateSoapEnvelope(string codigo_webservice, string cedula)
        {

            var webservice = Repository.GetAll().Where(c => c.vigente)
                                                .Where(c => c.codigo == codigo_webservice)
                                                .FirstOrDefault();

            var accion = _accionrepository.GetAllIncluding(c => c.ServicioWeb).Where(c => c.vigente)
                                               .Where(c => c.codigo == "BusquedaporNui")
                                               .Where(c => c.ServicioWebId == webservice.Id)
                                               .FirstOrDefault();

            var parametros = _parametrorepository.GetAllIncluding(c => c.ServicioWeb).Where(c => c.vigente)
                                              .Where(c => c.ServicioWebId == webservice.Id)
                                              .ToList();



            XmlDocument soapEnvelopeDocument = new XmlDocument();

            string xmlcuerpo = "";


            foreach (var _parametro in parametros)
            {


                if (_parametro.codigo == "_cedula")
                {
                    xmlcuerpo = xmlcuerpo +
                    "<" + _parametro.nombre + ">" + cedula + "</" + _parametro.nombre + ">";
                }
                else if (_parametro.codigo == "_huella")
                {

                }
                else
                {
                    xmlcuerpo = xmlcuerpo +
                    "<" + _parametro.nombre + ">" + _parametro.valor + "</" + _parametro.nombre + ">";

                }

            }
            string[] nombre_accion = accion.nombre_accion.Split('/');
            soapEnvelopeDocument.LoadXml(@"
            <soapenv:Envelope xmlns:soapenv=""http://schemas.xmlsoap.org/soap/envelope/"" 
                              xmlns:reg=""http://www.registrocivil.gob.ec"">" +
                              "<soapenv:Header/>" +
                              "<soapenv:Body>" +
                              "<reg:" + nombre_accion[nombre_accion.Length - 1] + ">" +
                              xmlcuerpo +
                              "</reg:" + nombre_accion[nombre_accion.Length - 1] + ">" +
                              "</soapenv:Body></soapenv:Envelope>");
            return soapEnvelopeDocument;
        }

        private XmlDocument CreateSoapEnvelopeHuella(string codigo_webservice, string cedula, string huella_dactilar)
        {

            var webservice = Repository.GetAll().Where(c => c.vigente)
                                                .Where(c => c.codigo == codigo_webservice)
                                                .FirstOrDefault();

            var accion = _accionrepository.GetAllIncluding(c => c.ServicioWeb).Where(c => c.vigente)
                                               .Where(c => c.codigo == "BusquedaPorNuiIndividual")
                                               .Where(c => c.ServicioWebId == webservice.Id)
                                               .FirstOrDefault();

            var parametros = _parametrorepository.GetAllIncluding(c => c.ServicioWeb).Where(c => c.vigente)
                                              .Where(c => c.ServicioWebId == webservice.Id)
                                              .ToList();



            XmlDocument soapEnvelopeDocument = new XmlDocument();

            string xmlcuerpo = "";


            foreach (var _parametro in parametros)
            {


                if (_parametro.codigo == "_cedula")
                {
                    xmlcuerpo = xmlcuerpo +
                    "<" + _parametro.nombre + ">" + cedula + "</" + _parametro.nombre + ">";
                }
                else
                if (_parametro.codigo == "_huella")
                {
                    xmlcuerpo = xmlcuerpo +
                   "<" + _parametro.nombre + ">" + huella_dactilar + "</" + _parametro.nombre + ">";
                }
                else
                {
                    xmlcuerpo = xmlcuerpo +
                    "<" + _parametro.nombre + ">" + _parametro.valor + "</" + _parametro.nombre + ">";

                }

            }
            string[] nombre_accion = accion.nombre_accion.Split('/');
            soapEnvelopeDocument.LoadXml(@"
            <soapenv:Envelope xmlns:soapenv=""http://schemas.xmlsoap.org/soap/envelope/"" 
                              xmlns:reg=""http://www.registrocivil.gob.ec"">" +
                              "<soapenv:Header/>" +
                              "<soapenv:Body>" +
                              "<reg:" + nombre_accion[nombre_accion.Length - 1] + ">" +
                              xmlcuerpo +
                              "</reg:" + nombre_accion[nombre_accion.Length - 1] + ">" +
                              "</soapenv:Body></soapenv:Envelope>");
            return soapEnvelopeDocument;
        }


        public Object ObjectToXML(string xml, Type objectType)
        {
            StringReader strReader = null;
            XmlSerializer serializer = null;
            XmlTextReader xmlReader = null;
            Object obj = null;
            try
            {
                strReader = new StringReader(xml);
                serializer = new XmlSerializer(objectType);
                xmlReader = new XmlTextReader(strReader);
                obj = serializer.Deserialize(xmlReader);
            }
            catch (Exception exp)
            {
                //Handle Exception Code
            }
            finally
            {
                if (xmlReader != null)
                {
                    xmlReader.Close();
                }
                if (strReader != null)
                {
                    strReader.Close();
                }
            }
            return obj;
        }

        public XmlNode BusquedaPorCedulaRegistroCivilHuellaDigital(string cedula, string huella_dactilar)
        {
            //  CORTAR HUELA DACTILAR

            huella_dactilar = huella_dactilar.Substring(0, 6);
            //codigo registro civil base de datos

            string codigo_webservice = "_registrocivil";
            //Busqueda de Werb service por codigo

            var webservice = Repository.GetAll().Where(c => c.vigente)
                                                .Where(c => c.codigo == codigo_webservice)
                                                .FirstOrDefault();

            var accion = _accionrepository.GetAllIncluding(c => c.ServicioWeb).Where(c => c.vigente)
                                               .Where(c => c.codigo == "BusquedaPorNuiIndividual")
                                               .Where(c => c.ServicioWebId == webservice.Id)
                                               .FirstOrDefault();

            //cREO EL cUERPO SOAP XML
            XmlDocument soapEnvelopeXml = CreateSoapEnvelopeHuella(codigo_webservice, cedula, huella_dactilar);
            ServicePointManager.Expect100Continue = true;
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls
                   | SecurityProtocolType.Tls11
                   | SecurityProtocolType.Tls12
                   | SecurityProtocolType.Ssl3;


            //se crea la peticion
            HttpWebRequest webRequest = CreateWebRequest(webservice.url, accion.nombre_accion);
            InsertSoapEnvelopeIntoWebRequest(soapEnvelopeXml, webRequest);
            var parameter = _parametrorepository.GetAll().Where(c => c.codigo == "HOST.TECHINT").FirstOrDefault();
            if (parameter != null && parameter.Id > 0)
            {
                webRequest.Host = parameter.valor;
                ElmahExtension.LogToElmah(new Exception("HOST CONSUMO WS:" + parameter.valor));
            }

            // iniciar  llamada async a solicitud web.
            IAsyncResult asyncResult = webRequest.BeginGetResponse(null, null);




            // suspende este hilo hasta que se complete la llamada. Tu podrías querer
            // haz algo útil aquí como actualizar tu interfaz de usuario
            asyncResult.AsyncWaitHandle.WaitOne();


            // obtener la respuesta de la solicitud web completada.
            string soapResult;
            using (WebResponse webResponse = webRequest.EndGetResponse(asyncResult))
            {
                using (StreamReader rd = new StreamReader(webResponse.GetResponseStream()))
                {
                    soapResult = rd.ReadToEnd();
                }

            }
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(soapResult);
            XmlNodeList node = doc.GetElementsByTagName("return");

            return node[0];
        }

        public RegistroCivilDto ChangeResultXMLObject(XmlNode node)
        {
            var jsonText = JsonConvert.SerializeXmlNode(node);
            var jo = JObject.Parse(jsonText);
            var result = jo["return"].ToString();
            ElmahExtension.LogToElmah(new Exception("Convert Object" + result));
            var format = "dd/MM/yyyy"; // your datetime format
            var dateTimeConverter = new IsoDateTimeConverter { DateTimeFormat = format };

            var RegistroCivilDto = JsonConvert.DeserializeObject<RegistroCivilDto>(result, dateTimeConverter);
            return RegistroCivilDto;
        }
        public bool GuardaInformacionWebServiceenConsultaPublica(XmlNode datosWebService, bool huellaDactilar)
        {
            //Cast XML NODE  info del Web Service en objeto;


            try
            {
                RegistroCivilDto info = ChangeResultXMLObject(datosWebService);

                if (info.CodigoError != "000")
                {
                 
                    return false;
                }
                else
                {
                    int userId = Int32.Parse(System.Web.HttpContext.Current.User.Identity.GetUserId().ToString());
                    ConsultaPublica cp = new ConsultaPublica()
                    {
                        calle = info.Calle,
                        codigo_error = info.CodigoError,
                        error = info.Error,
                        domicilio = info.Domicilio,

                        condicion_cedulado = info.CondicionCedulado,
                        fecha_consulta = DateTime.Now,
                        conyugue = info.Conyuge,
                        estado_civil = info.EstadoCivil,
                        identificacion = info.NUI,
                        instruccion = info.Instruccion,
                        lugar_nacimiento = info.LugarNacimiento,
                        nacionalidad = info.Nacionalidad,
                        nombres_completos = info.Nombre,
                        numero_casa = info.NumeroCasa,
                        profesion = info.Profesion,
                        sexo = info.Sexo,
                        fecha_cedulacion = info.FechaCedulacion,
                        fecha_matrimonio = info.FechaMatrimonio,
                        fecha_nacimiento = info.FechaNacimiento,
                        fecha_fallecimiento = info.FechaFallecimiento,
                        ProyectoId = 0,
                        fotografia = info.Fotografia,
                        tipoRC = huellaDactilar ? "Biométrico" : "Demográfico",
                        usuarioConsumoId = userId

                    };
                    var tipoIdentificacion = _catalogoRepository.GetAll().Where(c => c.codigo == RRHHCodigos.CODIGO_IDENTIFICACION_CEDULA).FirstOrDefault();
                    var proyecto = _catalogoRepository.GetAll().Where(c => c.codigo == RRHHCodigos.CODIGO_CATALOGO_GENERAL).FirstOrDefault();
                    if (tipoIdentificacion != null)
                    {
                        cp.TipoIdentificacionId = tipoIdentificacion.Id;
                    }
                    if (proyecto != null)
                    {
                        cp.ProyectoId = proyecto.Id;
                    }

                    var existe = _consultaPublicaRepository.GetAll().Where(c => c.identificacion == cp.identificacion).FirstOrDefault();

                    if (existe == null) {
                    _consultaPublicaRepository.Insert(cp);
                    }

                    return true;
                }
            }
            catch (Exception e)
            {
                ElmahExtension.LogToElmah(new Exception("ERROR CAST OBJE TO" + e.Message));
                return false;
            }



        }
    }
}

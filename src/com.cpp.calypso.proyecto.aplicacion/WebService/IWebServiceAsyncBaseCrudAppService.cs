using com.cpp.calypso.comun.aplicacion;
using com.cpp.calypso.proyecto.dominio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace com.cpp.calypso.proyecto.aplicacion.WebService
{
    public interface IWebServiceAsyncBaseCrudAppService : IAsyncBaseCrudAppService<ServicioWeb, ServicioWebDto, PagedAndFilteredResultRequestDto>
    {

        //API colaboradores PARA WEB SERVICE REGISTOR CIVIL
        XmlNode BusquedaPorCedulaRegistroCivil(string cedula);
        HttpWebRequest CreateWebRequest(string url, string action);
        Object ObjectToXML(string xml, Type objectType);


        XmlNode BusquedaPorCedulaRegistroCivilHuellaDigital(string cedula, string huella_dactilar);

        bool GuardaInformacionWebServiceenConsultaPublica(XmlNode datosWebService, bool huellaDactilar);
    }
}

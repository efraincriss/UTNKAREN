using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using com.cpp.calypso.proyecto.aplicacion;
using com.cpp.calypso.proyecto.dominio;

namespace com.cpp.calypso.web.Areas.Proyecto.Models
{
    public class CertificadoViewModel
    {
        public ProyectoDto Proyecto { get; set; }
        public string fechaemision { get; set; }
        public string fechacorte { get; set; }
        public Contrato  Contrato { get; set; }
        public Cliente Cliente { get; set; }
        public int certificado { get; set; }
    }
}
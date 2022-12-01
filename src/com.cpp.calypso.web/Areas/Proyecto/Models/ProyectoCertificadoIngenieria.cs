using com.cpp.calypso.proyecto.aplicacion;
using com.cpp.calypso.proyecto.aplicacion.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace com.cpp.calypso.web.Areas.Proyecto.Models
{
    public class ProyectoCertificadoIngenieria
    {
        public ProyectoDto Proyecto { get; set; }
        public List<CertificadoIngenieriaDto> Certificados { get; set; }
    }
}
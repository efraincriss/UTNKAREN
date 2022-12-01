using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using com.cpp.calypso.proyecto.aplicacion.Dto;

namespace com.cpp.calypso.web.Areas.Proyecto.Models
{
    public class CertificadoDetalleViewModel
    {

        public CertificadoDto Certificado { get; set; }

        public List<DetalleCertificadoDto> detalles { get; set; }

        public decimal montoTotalAIU { get; set; }
        public decimal montoTotalSinAIU { get; set; }
    }
}
using com.cpp.calypso.proyecto.aplicacion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace com.cpp.calypso.web.Areas.Proyecto.Models
{
    public class FacturaViewModel
    {
        public FacturaDto factura { get; set; }

        public List<CertificadoFacturaDto> certificados { get; set; }

    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using com.cpp.calypso.proyecto.aplicacion;

namespace com.cpp.calypso.web.Areas.Proyecto.Models
{
    public class DetalleAvanceProcuraViewModel
    {

        public List<DetalleAvanceProcuraDto> DetalleAvanceProcuraDto { get; set; }

        public AvanceProcuraDto AvanceProcutProcuraDto { get; set; }
    }
}
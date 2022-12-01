using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using com.cpp.calypso.proyecto.aplicacion;

namespace com.cpp.calypso.web.Areas.Proyecto
{
    public class AvanceObraDetallesViewModel
    {
        public List<DetalleAvanceObraDto> DetallesAvanceObra { get; set; }

        public AvanceObraDto AvanceObra { get; set; }

        public ProyectoDto proyecto { get; set; }

        public List<ObraAdicionalDto> ObrasAdicionales { get; set; }

        public List<ArchivosAvanceObraDto> Archivos { get; set; }
    }
}
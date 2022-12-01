using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace com.cpp.calypso.web.Areas.Proyecto.Models
{
    public class CreateObraAdicionalViewModel
    {
        public int  AvanceObraId { get; set; }

        public int OfertaId { get; set; }

        public DateTime fecha_presentacion { get; set; }

        public string codigo_avance_obra { get; set; }

        public string nombre_proyecto { get; set; }
    }
}
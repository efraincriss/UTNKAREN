using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using com.cpp.calypso.proyecto.aplicacion;

namespace com.cpp.calypso.web.Areas.Proyecto.Models
{
    public class CreateGrViewModel
    {
        public GRDto Gr { get; set; }

        public List<ProyectoDto> Proyectos { get; set; }
    }
}
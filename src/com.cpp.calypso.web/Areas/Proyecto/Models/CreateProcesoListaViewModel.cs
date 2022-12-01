using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using com.cpp.calypso.proyecto.aplicacion;
using com.cpp.calypso.proyecto.dominio;

namespace com.cpp.calypso.web.Areas.Proyecto.Models
{
    public class CreateProcesoListaViewModel
    {
        public ProcesoListaDistribucionDto ProcesoListaDistribucion { get; set; }
        public List<ProcesoNotificacionDto> procesos { get; set; }

        public List<ListaDistribucionDto> listas { get; set; }

    }
}
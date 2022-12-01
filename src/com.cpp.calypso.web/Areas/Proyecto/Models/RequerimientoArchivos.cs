using com.cpp.calypso.proyecto.aplicacion;
using com.cpp.calypso.proyecto.aplicacion.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace com.cpp.calypso.web.Areas.Proyecto.Models
{
    public class RequerimientoArchivos
    {

        public RequerimientoDto Requerimiento { get; set; }

        public List<ArchivosRequerimientoDto> ListaArchivos { get; set; }
    }
}
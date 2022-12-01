using com.cpp.calypso.proyecto.aplicacion.Documentos.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace com.cpp.calypso.web.Models
{
    public class Sync
    {
       public List<SeccionDto> secciones { get; set; }
      public  List<ImagenSeccionDto> imagenesSeccion { get; set; }
    }
}
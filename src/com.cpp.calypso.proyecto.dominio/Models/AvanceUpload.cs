using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace com.cpp.calypso.proyecto.dominio.Models
{
   public class AvanceUpload
    {

        public DateTime fecha_presentacion { get; set; }
        public DateTime fecha_desde { get; set; }
        public DateTime fecha_hasta { get; set; }
        public HttpPostedFileBase UploadedFile { get; set; }
    }
}

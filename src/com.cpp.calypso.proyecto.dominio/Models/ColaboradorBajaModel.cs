using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace com.cpp.calypso.proyecto.dominio.Models
{
    public class ColaboradorBajaModel
    {
        public int ColaboradoresId { get; set; }
        public string estado { get; set; }
        public bool requiere_entrevista { get; set; }
        public string detalle_baja { get; set; }
        public DateTime fecha_baja { get; set; }
        public int catalogo_motivo_baja_id { get; set; }
        public HttpPostedFileBase UploadedFile { get; set; }
    }
}

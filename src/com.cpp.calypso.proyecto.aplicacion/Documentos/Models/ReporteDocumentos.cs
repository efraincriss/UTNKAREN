using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace com.cpp.calypso.proyecto.aplicacion.Documentos.Models
{
    public class ReporteDocumentos
    {
        public string Contrato { get; set; }
        public string CodigoDocumento { get; set; }
        public string NombreDocumento { get; set; }
        public string TipoDocumento { get; set; }
        public string CantidadPag { get; set; }
        public string CantidadSecciones { get; set; }
        public string UsuarioCreador { get; set; }
        public string TieneImagen { get; set; }
        public string Nombrecarpeta { get; set; }
        public string EstadoCarpeta { get; set; }
        public string FechaCreación { get; set; }

    }
}

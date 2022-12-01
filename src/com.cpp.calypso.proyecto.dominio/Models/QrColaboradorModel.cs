using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace com.cpp.calypso.proyecto.dominio.Models
{
    public class QrColaboradorModel
    {
        public int Id { get; set; }
        public virtual string CodigoSap { get; set; }
        public virtual string Identificacion { get; set; }
        public virtual string Nombres { get; set; }
        public virtual string Area { get; set; }
        public virtual string Cargo { get; set; }
        public virtual string TipoUsuario { get; set; }
        public virtual string PrimerApellido { get; set; }
        public virtual string SegundoApellido { get; set; }

        public virtual string NombreCompleto { get; set; }
        public virtual string CodigoSapLocal { get; set; }
        public virtual bool esExterno { get; set; }
    }
}

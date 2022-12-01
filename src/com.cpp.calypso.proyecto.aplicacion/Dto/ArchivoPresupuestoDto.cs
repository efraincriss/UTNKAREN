using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace com.cpp.calypso.proyecto.aplicacion.Dto
{
    public class ArchivoPresupuestoDto
    {

        public int Id { get; set; }
        public int PresupuestoId { get; set; }
        public string nombre { get; set; }
        public DateTime fecha_registro { get; set; }
        public string formatFechaRegistro { get; set; }
        public byte[] hash { get; set; }
        public string tipo_contenido { get; set; }
        public virtual string filebase64 { get; set; }
    }
}

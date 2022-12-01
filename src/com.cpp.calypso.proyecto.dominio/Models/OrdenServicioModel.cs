using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace com.cpp.calypso.proyecto.dominio.Models
{
    public class OrdenServicioModel
    {
        public int Id { get; set; }
        public string codigo_orden_servicio { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime fecha_orden_servicio { get; set; }
        public decimal monto_aprobado_os { get; set; } = 0;
        public decimal monto_aprobado_suministros { get; set; } = 0;
        public decimal monto_aprobado_construccion { get; set; } = 0;
        public decimal monto_aprobado_ingeniería { get; set; } = 0;
        public decimal monto_aprobado_subcontrato { get; set; } = 0;
        public string version_os { get; set; }
        public int EstadoId { get; set; }
        public int anio { get; set; }
        public string nombreEstado { get; set; }
        public string referencias_po { get; set; }
        public string formatFechaOs { get; set; }
        public string ofertasComerciales { get; set; }
        public bool tieneArchivo { get; set; }
        public int? ArchivoId { get; set; }
        public string comentarios { get; set; }
    }
}

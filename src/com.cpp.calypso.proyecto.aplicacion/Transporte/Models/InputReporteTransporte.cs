using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace com.cpp.calypso.proyecto.aplicacion.Transporte.Models
{
    public class InputReporteTransporte
    {
        public int? ProveedorId { get; set; }
        public int? RutaId { get; set; }
        public DateTime? FechaDesde { get; set; }
        public DateTime? FechaHasta { get; set; }
        public int? VehiculoId { get; set; }
        public DateTime? Fecha { get; set; }
        public bool check { get; set; }
    }
}

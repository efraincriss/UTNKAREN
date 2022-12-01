using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace com.cpp.calypso.proyecto.dominio.Models
{
    public class DatosViandas
    {
        public int SolicitudViandaId { get; set; }
        public DateTime FechaConsumo { get; set; }
        public DateTime  FechaSolicitud { get; set; }
        public string  NumeroIdentificacion { get; set; }
        public string Apellidos  { get; set; }
        public string Nombres { get; set; }
        public string NombreComida { get; set; }
        public int tipoComidaId { get; set; }
        public decimal PU { get; set; }
        public decimal TotalPedido { get; set; }
        public decimal TotalConsumido { get; set; }
    }
}

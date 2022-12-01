using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace com.cpp.calypso.proyecto.aplicacion.Proveedor.Dto
{
    public class FormatLiquidacionSolicitudVianda
    {
        public int Id { get; set; }
        public string NombreProveedor { get; set; }
        public string FechaConsumo { get; set; }
        public string Locacion { get; set; }
        public string IdSolicitante { get; set; }
        public string NombreSolicitante { get; set; }
        public string TipoComida { get; set; }
        public decimal TotalSolicitado { get; set; }
        public decimal Tarifa { get; set; }
        public decimal Total { get; set; }
        public bool liquidado { get; set; }
        public string FechaPedido { get; set; }
        public int ProveedorId { get; set; }

    }
}

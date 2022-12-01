using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace com.cpp.calypso.proyecto.dominio.Models
{
   public class ProcuraDatos
    {
        public int Id { get; set; }
        public int ComputoId { get; set; }
        public decimal Porcentaje { get; set; }
        public int DetalleOrdenCompraId { get; set; }
        public string Proyecto { get; set; }
        public int ProyectoId { get; set; }
        public string MR { get; set; }
        public string OC { get; set; }
        public string DescripcionSuministros { get; set; }
        public string PO { get; set; }
        public string FAT { get; set; }
        public string ArriboETA { get; set; }
        public decimal Costo { get; set; }
        public decimal Venta { get; set; }
        public decimal AvanceProcentaje { get; set; }
        public decimal AvanceCosto{ get; set; }
        public decimal AvanceProcentajeTotal { get; set; }
        public decimal AvanceCostoTotal { get; set; }
        public DateTime FechaAvance { get; set; }
        public decimal Cantidad { get; set; }
        public decimal PorcentajeDetalle { get; set; }
        public decimal ValorRealProcura { get; set; }
        public DateTime FechaPresentacionAvance { get; set; }
    }
}

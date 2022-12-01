using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace com.cpp.calypso.proyecto.dominio.Models
{
    public class IngenieriaDatos
    {
        public int Id { get; set; }
        public string Rubro { get; set; }
        public int ColaboradorId { get; set; }
        public string Nombre { get; set; }
        public string Categoria { get; set; }
        public string Unidad { get; set; }
        public decimal TotalHoras { get; set; }
        public decimal Tarifa { get; set; }
        public decimal CostoTotal { get; set; }
        public string Etapa { get; set; }
        public TipoColaborador TipoColaborador { get; set; }
    }
}

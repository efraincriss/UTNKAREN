using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace com.cpp.calypso.proyecto.dominio.Models
{
    public class ReportConsumo
    {
        public int Id { get; set; }
        public string identificacionProveedor { get; set; }
        public string razon_social { get; set; }
        public int ProveedorId { get; set; }
        public DateTime fechaConsumo { get; set; }
        public string NombresCopletos { get; set; }
        public string Identificacion { get; set; }
        public decimal precio { get; set; }
        public string nombretipoComida { get; set; }
        public string nombreopcionComida { get; set; }
        public int TipoComidaId { get; set; }
        public int OpcionComidaId { get; set; }
        public string formatfechaConsumo { get; set; }

        public string IdentificadorMovil { get; set; }

        public string ProveedorAnterior { get; set; }
        public string fechaConsumoAnterior { get; set; }

        public string identificador { get; set; }
        public string nombreOrigen { get; set; }

    }
}

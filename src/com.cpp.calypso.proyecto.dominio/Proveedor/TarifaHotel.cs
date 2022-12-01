using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.Domain.Entities;
using com.cpp.calypso.comun.dominio;

namespace com.cpp.calypso.proyecto.dominio.Proveedor
{
    [Serializable]
    public class TarifaHotel : Entity, ISoftDelete
    {
        [Obligado]
        [DisplayName("Tipo Habitación")]
        public int TipoHabitacionId { get; set; }
        public Catalogo TipoHabitacion { get; set; }


        [Obligado]
        [DisplayName("Contrato Proveedor")]
        public int ContratoProveedorId { get; set; }
        public ContratoProveedor ContratoProveedor { get; set; }

        [Obligado]
        [DisplayName("Capacidad")]
        public int capacidad { get; set; }


        [Obligado]
        [DisplayName("Costo por Persona")]
        public decimal costo_persona { get; set; }

        [Obligado] [DisplayName("Estado")] public bool estado { get; set; } = true;

        public string GetEstadoNombre()
        {
            return this.estado ? "ACTIVO" : "INACTIVO";
        }

        public decimal GetTotal()
        {
            var cap = Convert.ToDecimal(this.capacidad);
            return cap * this.costo_persona;
        }

        public bool IsDeleted { get; set; }
    }
}

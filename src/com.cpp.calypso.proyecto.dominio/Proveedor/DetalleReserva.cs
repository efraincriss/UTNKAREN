using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using com.cpp.calypso.comun.dominio;

namespace com.cpp.calypso.proyecto.dominio.Proveedor
{
    [Serializable]
    public class DetalleReserva : Entity, IFullAudited
    {
        [Obligado]
        [DisplayName("Reserva")]
        public int ReservaHotelId { get; set; }
        public ReservaHotel ReservaHotel { get; set; }

        [DisplayName("Origen Consumo")]
        public OrigenConsumo? origen_consumo_id { get; set; }

        [DataType(DataType.Date)]
        [DisplayName("Fecha Reserva")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime? fecha_reserva { get; set; }

        [DataType(DataType.Date)]
        [DisplayName("Fecha Consumo")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime? fecha_consumo { get; set; }

        [Obligado]
        [DisplayName("Consumido")]
        public bool consumido { get; set; }

        [Obligado]
        [DisplayName("Facturado")]
        public bool facturado { get; set; }


        [Obligado]
        [DisplayName("liquidado")]
        public bool liquidado { get; set; }


        [Obligado]
        [DisplayName("Detalle Liquidación")]
        public int liquidacion_detalle_id { get; set; }

        [DisplayName("Extemporaneo")]
        public bool extemporaneo { get; set; } = false;

        [DisplayName("Tiene Derecho")]
        public bool tiene_derecho { get; set; } = true;

        public bool aplica_lavanderia { get; set; } = false;

        public string GetConsumidoNombre()
        {
            return this.consumido ? "Si" : "No";
        }

        public string GetFacturadoNombre()
        {
            return this.facturado ? "Si" : "No";
        }


        public DateTime CreationTime { get; set; }

        public long? CreatorUserId { get; set; }

        public DateTime? LastModificationTime { get; set; }

        public long? LastModifierUserId { get; set; }

        public bool IsDeleted { get; set; }

        public DateTime? DeletionTime { get; set; }

        public long? DeleterUserId { get; set; }
    }


    public enum OrigenConsumo
    {
        [Description("Identificacion")]
        Identificacion = 1,

        [Description("QR")]
        Qr = 2,

        [Description("Huella")]
        Huella = 3
    }
}

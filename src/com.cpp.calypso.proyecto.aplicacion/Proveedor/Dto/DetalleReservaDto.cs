using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using com.cpp.calypso.comun.dominio;
using com.cpp.calypso.proyecto.dominio.Proveedor;

namespace com.cpp.calypso.proyecto.aplicacion.Proveedor.Dto
{
    [AutoMap(typeof(DetalleReserva))]
    [Serializable]
    public class DetalleReservaDto : EntityDto
    {
        [Obligado]
        [DisplayName("Reserva")]
        public int ReservaHotelId { get; set; }

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


        public bool aplica_lavanderia { get; set; } = false;



        [DisplayName("Tiene Derecho")]
        public bool tiene_derecho { get; set; } = true;




        public string origen_consumo_nombre { get; set; }


        public string consumido_nombre { get; set; }

        public string facturado_nombre { get; set; }

        public string fecha_reserva_format { get; set; }
        public string fecha_consumo_format { get; set; }
        public string aplica_lavanderia_nombre { get; set; }


    }
}

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
using com.cpp.calypso.proyecto.dominio;
using Newtonsoft.Json;

namespace com.cpp.calypso.proyecto.aplicacion
{
    [AutoMap(typeof(OrdenCompra))]
    [Serializable]
    public class OrdenCompraDto: EntityDto
    {

        [Obligado]
        [DisplayName("Oferta")]
        public int OfertaId { get; set; }

        [JsonIgnore]
        public virtual Oferta Oferta { get; set; }
        
        [Obligado]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [DisplayName("Fecha de Presentación")]
        public DateTime fecha_presentacion { get; set; }


        [Obligado]
        [DisplayName("Número Pedido Compra")]
        [LongitudMayor(50)]
        public string nro_pedido_compra { get; set; }


        [Obligado]
        [Range(0.0, Double.MaxValue, ErrorMessage = "El valor debe ser >= a cero")]
        [DisplayName("Valor Pedido de Compra")]
        public decimal valor_pedido_compra { get; set; }

        [Obligado]
        public bool vigente { get; set; } = true;

        [DisplayName("Estado")]
        public bool estado { get; set; } = true;

        [DisplayName("Referencia MR/SR/MT")]
        public string referencia { get; set; }

    }
}

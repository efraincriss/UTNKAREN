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

namespace com.cpp.calypso.proyecto.aplicacion
{
    [AutoMap(typeof(DetalleOrdenCompra))]
    [Serializable]
    public class DetalleOrdenCompraDto : EntityDto
    {

        [Obligado]
        [DisplayName("Orden Compra")]
        public int OrdenCompraId { get; set; }

        public virtual OrdenCompra OrdenCompra { get; set; }

        [DisplayName("Seleccione Item")]
        public int ComputoId { get; set; }
        public virtual Computo Computo { get; set; }

        [Obligado]
        [DisplayName("Tipo de Registro")]
        public DetalleOrdenCompra.TipoFecha tipoFecha { get; set; }



        [Obligado]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [DisplayName("Fecha")]
        public virtual DateTime fecha { get; set; }


        [DisplayName("Porcentaje")]
        public virtual decimal porcentaje { get; set; }

        [DisplayName("Costo Porcentaje a pagar(USD)")]

        public virtual decimal valor { get; set; }

        [Obligado]
        public virtual bool vigente { get; set; } = true;

        [Obligado]
        [DisplayName("Estado")]
        public virtual DetalleOrdenCompra.EstadoDetalleOrdenCompra estado { get; set; }
        
        public virtual List<ComputoDto> ItemsOrdenCompra { get; set; }
        public virtual Item  Item { get; set; }

        public virtual string nombreitem { get; set; }
        public virtual string tiporegistro { get; set; }
        public virtual string nombreestado { get; set; }
        public virtual string porcentajes { get; set; }
        public virtual string valores { get; set; }

        public virtual string fechas { get; set; }
    }
}

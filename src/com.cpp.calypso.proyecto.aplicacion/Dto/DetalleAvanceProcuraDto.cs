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
    [AutoMap(typeof(DetalleAvanceProcura))]
    [Serializable]
    public class DetalleAvanceProcuraDto: EntityDto
    {

        [Obligado]
        [DisplayName("Avance Procura")]
        public int AvanceProcuraId { get; set; }


        public virtual AvanceProcura AvanceProcura { get; set; }

        [Obligado]
        [DisplayName("Item")]
        public int DetalleOrdenCompraId { get; set; }

        public virtual DetalleOrdenCompra DetalleOrdenCompra { get; set; }


        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [DisplayName("Fecha de Presentación")]
        public virtual DateTime fecha_real { get; set; }



        [DisplayName("Cantidad")]
        public virtual decimal cantidad { get; set; }

        [Obligado]
        [DisplayName("Precio Unitario")]
        public virtual decimal precio_unitario { get; set; }

        [Obligado]
        [DisplayName("Valor Real")]
        public virtual decimal valor_real { get; set; }
        [Obligado]
        public virtual bool vigente { get; set; } = true;

        [Obligado]
        [DisplayName("Estado")]
        public virtual DetalleAvanceProcura.EstadoDetalleProcura estado { get; set; }


        [DisplayName("Ingreso Acumulado")]
        public decimal ingreso_acumulado { get; set; }

        [DisplayName("Cálculo Diario")]
        public decimal calculo_diario { get; set; }

        [DisplayName("Cálculo Anterior")]
        public decimal calculo_anterior { get; set; }

        public bool estacertificado { get; set; } = false;


        public virtual List<ComputoDto> ItemsOrdenCompra { get; set; }
        public virtual Oferta Oferta { get; set; }
        public virtual Item Item { get; set; }
        public virtual int OrdenCompraId { get; set; }
        [DisplayName("Orden de Compra")]
        public virtual OrdenCompra OrdenCompra { get; set; }
        public virtual Computo Computo { get; set; }
        public virtual List<OrdenCompraDto> OrdenesCompra { get; set; }

        public virtual string fechar { get; set; }

    }
}

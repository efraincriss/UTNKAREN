using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using com.cpp.calypso.comun.dominio;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace com.cpp.calypso.proyecto.dominio
{
    [Serializable]
    public class LiquidacionServicio: Entity, IFullAudited
    {
        [Obligado]
		[DisplayName("Contrato Proveedor")]
		public int ContratoProveedorId { get; set; }
		public virtual ContratoProveedor ContratoProveedor { get; set; }

        [DisplayName("Código")]
        public string Codigo { get; set; }


        [DataType(DataType.Date)]
        [DisplayName("Fecha Inicio")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public virtual DateTime FechaDesde { get; set; }


        [DataType(DataType.Date)]
        [DisplayName("Fecha Fin")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public virtual DateTime FechaHasta { get; set; }

        [DisplayName("Monto Consumido")]
        [DefaultValue(0.0)]
        public decimal MontoConsumido { get; set; }


        [DataType(DataType.Date)]
        [DisplayName("Fecha Pago")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public virtual DateTime? FechaPago { get; set; }


        [DisplayName("Estado")]
        public EstadoLiquidacion Estado { get; set; }

        [Obligado]
        [DisplayName("Servicios")]
        public int TipoServicioId { get; set; }
        public virtual Catalogo TipoServicio { get; set; }


        public long? CreatorUserId { get; set; }
        public DateTime CreationTime { get; set; }
        public long? LastModifierUserId { get; set; }
        public DateTime? LastModificationTime { get; set; }
        public long? DeleterUserId { get; set; }
        public DateTime? DeletionTime { get; set; }
        public bool IsDeleted { get; set; }

    }

    public enum EstadoLiquidacion
    {
        [Description("Generado")]
        Generado = 0,

        [Description("Pagado")]
        Pagado = 1,
        [Description("Anulado")]
        Anulado = 2,

    }
}


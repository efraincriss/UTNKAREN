using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using com.cpp.calypso.comun.dominio;
using com.cpp.calypso.proyecto.dominio;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace com.cpp.calypso.proyecto.aplicacion.Proveedor.Dto
{
    [AutoMap(typeof(LiquidacionServicio))]
    [Serializable]
    public class LiquidacionServicioDto:EntityDto
    {
        [Obligado]
        [DisplayName("Contrato Proveedor")]
        public int ContratoProveedorId { get; set; }

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

        public virtual string NombreContratoProveedor { get; set; }
        public virtual string FormatFechaDesde { get; set; }
        public virtual string FormatFechaHasta { get; set; }
        public virtual string FormatFechaPago { get; set; }
        public virtual string NombreEstado { get; set; }
        public virtual string NombreTipoServicio { get; set; }
        public virtual int   ProveedorId { get; set; }

    }
}

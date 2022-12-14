using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using com.cpp.calypso.comun.dominio;
using com.cpp.calypso.proyecto.dominio;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace com.cpp.calypso.proyecto.aplicacion.Dto
{
    [AutoMap(typeof(ContratoProveedor))]
    [Serializable]
    public class ContratoProveedorTipoOpcionesDto : EntityDto
    {
        public ContratoProveedorTipoOpcionesDto()
        {
            estado = ContratoEstado.Activo;

            deleteIds = new List<int>();
            tipo_opciones_comida = new List<TipoOpcionComidaDto>();
        }

        [Obligado]
        [DisplayName("Proveedor")]
        public int ProveedorId { get; set; }
        public string proveedor_nombre { get; set; }

        [DisplayName("Código")]
        public string codigo { get; set; }

        [Obligado]
        [DisplayName("Empresa")]
        public int EmpresaId { get; set; }

        [DisplayName("Cliente")]
        public string empresa_nombre { get; set; }

        [Obligado]
		[StringLength(500)]
		[DisplayName("Objeto")]
		public string objeto { get; set; }

        [Obligado]
        [DataType(DataType.Date)]
		[DisplayName("Fecha inicio")]
		[DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
		public DateTime? fecha_inicio { get; set; }

        [Obligado]
        [DataType(DataType.Date)]
		[DisplayName("Fecha fin")]
		[DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
		public DateTime? fecha_fin { get; set; }

		[Obligado]
		[DisplayName("Plazo Pago")]
		public int plazo_pago { get; set; }

        [Obligado]
        [DisplayName("Monto")]
        public decimal monto { get; set; }

        [Obligado]
		[StringLength(10)]
		[DisplayName("Orden de Compra")]
		public string orden_compra { get; set; }

		[Obligado]
		[DisplayName("Estado")]
		public ContratoEstado estado { get; set; }
        public string estado_nombre { get; set; }


        public int? documentacion_id { get; set; }

        /// <summary>
        /// Asociar un archivo. 
        /// </summary>
        public virtual ArchivoDto documentacion_subida { get; set; }

        /// <summary>
        /// Lista de Ids tipos opciones comida a eliminar
        /// </summary>
        public List<int> deleteIds { get; set; }

        public virtual ICollection<TipoOpcionComidaDto> tipo_opciones_comida { get; set; }
    }
}

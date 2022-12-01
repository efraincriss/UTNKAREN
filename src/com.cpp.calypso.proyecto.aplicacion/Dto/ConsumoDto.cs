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

namespace com.cpp.calypso.proyecto.aplicacion.Dto
{
    [AutoMap(typeof(Consumo))]
    [Serializable]
    public class ConsumoDto : EntityDto
    {
		[Obligado]
		[DisplayName("Proveedor")]
		public int ProveedorId { get; set; }
		public virtual dominio.Proveedor.Proveedor Proveedor { get; set; }

		[Obligado]
		[DisplayName("Colaborador")]
		public int ColaboradorId { get; set; }
		public virtual Colaborador Colaborador { get; set; }

		[DataType(DataType.Date)]
		[DisplayName("Fecha")]
		[DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
		public DateTime fecha { get; set; }

		[Obligado]
		[DisplayName("Tipo Comida")]
		public int TipoOpcionComidaId { get; set; }
		public virtual TipoOpcionComida TipoComida { get; set; }

		[Obligado]
		[DisplayName("Opción Comida")]
		public int OpcionComidaId { get; set; }
		public virtual OpcionComida OpcionComida { get; set; }


		[DisplayName("Observación")]
		public string observacion { get; set; }

		[Obligado]
		[DisplayName("Vigente")]
		public bool vigente { get; set; } = true;
	}
}

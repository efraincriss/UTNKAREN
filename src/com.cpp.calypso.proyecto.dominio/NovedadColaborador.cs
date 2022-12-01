using Abp.Domain.Entities;
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
    public class NovedadColaborador : Entity
    {

        [Obligado]
        [DisplayName("Colaborador")]
        public int ColaboradorId { get; set; }
        public Colaboradores  Colaborador { get; set; }

        [Obligado]
		[DisplayName("Proveedor")]
		public int ProveedorId { get; set; }
		public virtual dominio.Proveedor.Proveedor Proveedor { get; set; }

	
		[DisplayName("Opcion Comida")]
		public int? OpcionComidaId { get; set; }
		public virtual OpcionComida OpcionComida { get; set; }

		[DataType(DataType.Date)]
		[DisplayName("Fecha")]
		[DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
		public DateTime fecha { get; set; }

		[Obligado]
		[StringLength(500)]
		[DisplayName("Observación")]
		public string observacion { get; set; }

		[Obligado]
		[DisplayName("Vigente")]
		public bool vigente { get; set; } = true;

		
		[DisplayName("Servicio")]
		public int? ServicioId { get; set; }
		public virtual Servicio Servicio { get; set; }
	}
}

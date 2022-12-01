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
    public class TipoAccionEmpresa : Entity
    {
		[Obligado]
		[DisplayName("Empresa")]
		public int EmpresaId { get; set; }
		public virtual Empresa Empresa { get; set; }

	
        [Obligado]
        [DisplayName("Tipo Comida")]
        public int tipo_comida_id { get; set; }
        public virtual Catalogo tipo_comida { get; set; }

        [Obligado]
		[DisplayName("Accion")]
		public int AccionId { get; set; }
		public virtual Catalogo Accion { get; set; }

        [Obligado]
        [DisplayName("Hora Desde")]
		[DisplayFormat(DataFormatString = "{0:HH:mm}", ApplyFormatInEditMode = true)]
		public TimeSpan hora_desde { get; set; }

        [Obligado]
        [DisplayName("Hora Hasta")]
		[DisplayFormat(DataFormatString = "{0:HH:mm}", ApplyFormatInEditMode = true)]
		public TimeSpan hora_hasta { get; set; }

         

    }
}

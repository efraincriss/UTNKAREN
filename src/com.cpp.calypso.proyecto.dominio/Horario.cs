using Abp.Domain.Entities;
using com.cpp.calypso.comun.dominio;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace com.cpp.calypso.proyecto.dominio
{
    [Serializable]
    public class Horario : Entity
    {

        [Obligado]
        [LongitudMayor(10)]
        [DisplayName("Código")]
        public string codigo { get; set; }

        [Obligado]
		[DisplayName("Nombre")]
		public string nombre { get; set; }

		[Obligado]
		[DisplayName("Hora Inicio")]
		[DisplayFormat(DataFormatString = "{0:HH:mm}", ApplyFormatInEditMode = true)]
		public TimeSpan hora_inicio { get; set; }

		[Obligado]
		[DisplayName("Hora Fin")]
		[DisplayFormat(DataFormatString = "{0:HH:mm}", ApplyFormatInEditMode = true)]
		public TimeSpan hora_fin { get; set; }

		[Obligado]
		[DisplayName("Estado")]
		public bool vigente { get; set; } = true;

	}
}

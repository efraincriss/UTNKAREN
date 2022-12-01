using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using com.cpp.calypso.comun.dominio;
using com.cpp.calypso.proyecto.dominio;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace com.cpp.calypso.proyecto.aplicacion.Dto
{
    [AutoMap(typeof(TipoAccionEmpresa))]
    [Serializable]
    public class TipoAccionEmpresaDto : EntityDto
    {
		[Obligado]
		[DisplayName("Empresa")]
		public int EmpresaId { get; set; }
		public virtual string empresa_nombre { get; set; }

		[Obligado]
		[DisplayName("Tipo Comida")]
        public int tipo_comida_id { get; set; }
        public virtual string tipo_comida_nombre { get; set; }

        [Obligado]
		[DisplayName("Accion")]
		public int AccionId { get; set; }
		public virtual string accion_nombre { get; set; }

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

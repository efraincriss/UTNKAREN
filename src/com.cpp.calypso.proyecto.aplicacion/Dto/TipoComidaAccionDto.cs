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
    [AutoMap(typeof(TipoComidaAccion))]
    [Serializable]
    public class TipoComidaAccionDto : EntityDto
    {
        [Obligado]
        [DisplayName("Empresa")]
        public int empresa_id { get; set; }
        public virtual Empresa empresas { get; set; }

        [Obligado]
        [DisplayName("Tipo Comida")]
        public int tipo_comida_id { get; set; }
        public virtual TipoComida tipos_comidas { get; set; }

        [Obligado]
        [DisplayName("Accion")]
        public int accion_id { get; set; }
        public virtual comun.dominio.Accion acciones { get; set; }

        [DataType(DataType.Time)]
        [DisplayName("Hora Desde")]
        [DisplayFormat(DataFormatString = "{0:HH:mm}", ApplyFormatInEditMode = true)]
        public DateTime hora_desde { get; set; }

        [DataType(DataType.Time)]
        [DisplayName("Hora Hasta")]
        [DisplayFormat(DataFormatString = "{0:HH:mm}", ApplyFormatInEditMode = true)]
        public DateTime hora_hasta { get; set; }

        [Obligado]
        [DisplayName("Vigente")]
        public bool vigente { get; set; } = true;
    }
}

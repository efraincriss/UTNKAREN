using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using Abp.Domain.Entities.Auditing;
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
    [AutoMap(typeof(ColaboradoresHuellaDigital))]
    [Serializable]
    public class ColaboradoresHuellaDigitalDto : EntityDto, IFullAudited
    {
        [Obligado]
        [DisplayName("Colaborador")]
        public int colaborador_id { get; set; }

        [Obligado]
        [DisplayName("Dedo Huella")]
        public int catalogo_dedo_id { get; set; }

        [Obligado]
        [DisplayName("Huella")]
        public String huella { get; set; }

        [Obligado]
        [DisplayName("Plantilla Base64")]
        public String plantilla_base64 { get; set; }

        [DataType(DataType.Date)]
        [DisplayName("Fecha Registro")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime? fecha_registro { get; set; }

        [Obligado]
        [DisplayName("Estado")]
        public bool vigente { get; set; } = true;

        [Obligado]
        [DisplayName("Principal")]
        public bool principal { get; set; } = false;

        public virtual String dedo{ get; set; }

        public virtual String esPrincipal { get; set; }

        public virtual int nro_huella { get; set; }

        public long? CreatorUserId { get; set; }
        public DateTime CreationTime { get; set; }
        public long? LastModifierUserId { get; set; }
        public DateTime? LastModificationTime { get; set; }
        public long? DeleterUserId { get; set; }
        public DateTime? DeletionTime { get; set; }
        public bool IsDeleted { get; set; }
    }
}

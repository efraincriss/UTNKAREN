using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using com.cpp.calypso.comun.dominio;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;


namespace com.cpp.calypso.proyecto.dominio
{
    [Serializable]
    public class ColaboradoresHuellaDigital : Entity, IFullAudited
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
        public DateTime? fecha_registro{ get; set; }

        [Obligado]
        [DisplayName("Estado")]
        public bool vigente { get; set; } = true;

        [Obligado]
        [DisplayName("Principal")]
        public bool principal { get; set; } = false;

        public long? CreatorUserId { get; set; }
        public DateTime CreationTime { get; set; }
        public long? LastModifierUserId { get; set; }
        public DateTime? LastModificationTime { get; set; }
        public long? DeleterUserId { get; set; }
        public DateTime? DeletionTime { get; set; }
        public bool IsDeleted { get; set; }
    }
}

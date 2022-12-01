using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using Abp.Domain.Entities.Auditing;
using com.cpp.calypso.comun.dominio;
using com.cpp.calypso.proyecto.dominio;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace com.cpp.calypso.proyecto.aplicacion.Dto
{
    [AutoMap(typeof(ColaboradoresAusentismoRequisitos))]
    [Serializable]
    public class ColaboradoresAusentismoRequisitosDto : EntityDto, IFullAudited
    {
        [Obligado]
        [DisplayName("Colaborador")]
        public int colaborador_ausentismo_id { get; set; }

        public virtual ColaboradoresAusentismo ColaboradorAusentismo { get; set; }

        [Obligado]
        [DisplayName("Requisito")]
        public int requisito_id { get; set; }
        
        [DisplayName("Archivo")]
        public int? archivo_id { get; set; }

        [Obligado]
        [DisplayName("Cumple")]
        public bool cumple { get; set; } = true;

        public virtual HttpPostedFileBase archivo_usuario { get; set; }
        public virtual Archivo Archivo { get; set; }
        public virtual Requisitos Requisitos { get; set; }

        public long? CreatorUserId { get; set; }
        public DateTime CreationTime { get; set; }
        public long? LastModifierUserId { get; set; }
        public DateTime? LastModificationTime { get; set; }
        public long? DeleterUserId { get; set; }
        public DateTime? DeletionTime { get; set; }
        public bool IsDeleted { get; set; }
    }
}

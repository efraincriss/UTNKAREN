using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using Abp.Domain.Entities.Auditing;
using com.cpp.calypso.proyecto.dominio;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace com.cpp.calypso.proyecto.aplicacion.Dto
{
    [AutoMap(typeof(ColaboradorMovilizacion))]
    [Serializable]
    public class ColaboradorMovilizacionDto: EntityDto
    {
        [DisplayName("Colaborador")]
        public int ColaboradorServicioId { get; set; }
        public virtual ColaboradorServicio ColaboradorServicio { get; set; }

        [DisplayName("Parroquia")]
        public int? ParroquiaId { get; set; }
        public virtual Parroquia Parroquia { get; set; }

        [DisplayName("Comunidad")]
        public int? ComunidadId { get; set; }
        public virtual Comunidad Comunidad { get; set; }

        [DisplayName("Tipo comida")]
        public int catalogo_tipo_movilizacion_id { get; set; }

        //public long? CreatorUserId { get; set; }
        //public DateTime CreationTime { get; set; }
        //public long? LastModifierUserId { get; set; }
        //public DateTime? LastModificationTime { get; set; }
        //public long? DeleterUserId { get; set; }
        //public DateTime? DeletionTime { get; set; }
        //public bool IsDeleted { get; set; }
    }
}

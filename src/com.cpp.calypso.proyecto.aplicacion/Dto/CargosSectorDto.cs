using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using Abp.Domain.Entities.Auditing;
using com.cpp.calypso.proyecto.dominio;
using JetBrains.Annotations;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace com.cpp.calypso.proyecto.aplicacion.Dto
{
    [AutoMap(typeof(CargosSector))]
    [Serializable]
    public class CargosSectorDto : EntityDto, IFullAudited
    {
     
        [DisplayName("Cargo")]
        public int CargoId { get; set; }
        public virtual Catalogo Cargo { get; set; }

       
        [DisplayName("Sector")]
        public int SectorId { get; set; }
        public virtual Catalogo Sector { get; set; }
        
        [DisplayName("Estado")]
        public bool vigente { get; set; } = true;

        public long? CreatorUserId { get; set; }
        public DateTime CreationTime { get; set; }
        public long? LastModifierUserId { get; set; }
        public DateTime? LastModificationTime { get; set; }
        public long? DeleterUserId { get; set; }
        public DateTime? DeletionTime { get; set; }
        public bool IsDeleted { get; set; }

        public virtual string nombre_cargo { get; set; }

        public virtual string nombre_sector { get; set; }

        public virtual int nro { get; set; }
    }
}

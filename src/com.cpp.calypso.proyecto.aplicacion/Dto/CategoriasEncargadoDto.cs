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
    [AutoMap(typeof(CategoriasEncargado))]
    [Serializable]
    public class CategoriasEncargadoDto : EntityDto, IFullAudited
    {
       
        [DisplayName("Categoría")]
        public int CategoriaId { get; set; }
        public virtual Catalogo Categoria { get; set; }

      
        [DisplayName("Encargado")]
        public int EncargadoId { get; set; }
        public virtual Catalogo Encargado { get; set; }

        [DisplayName("Estado")]
        public bool vigente { get; set; } = true;

        public long? CreatorUserId { get; set; }
        public DateTime CreationTime { get; set; }
        public long? LastModifierUserId { get; set; }
        public DateTime? LastModificationTime { get; set; }
        public long? DeleterUserId { get; set; }
        public DateTime? DeletionTime { get; set; }
        public bool IsDeleted { get; set; }

        public virtual string nombre_categoria { get; set; }

        public virtual string nombre_encargado { get; set; }

        public virtual int nro { get; set; }
    }
}

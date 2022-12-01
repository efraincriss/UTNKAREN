using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using com.cpp.calypso.proyecto.dominio.Documentos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace com.cpp.calypso.proyecto.aplicacion.Documentos.Dto
{
    [AutoMap(typeof(ImagenSeccion))]
    [Serializable]
    public class ImagenSeccionDto : EntityDto
    {
        public int SeccionId { get; set; }

        public string ImagenBase64 { get; set; }

        public string NombreImagen { get; set; }

        public bool Sincronizado { get; set; } = false;

        public bool IsDeleted { get; set; }

        public long? CreatorUserId { get; set; }

        public DateTime CreationTime { get; set; }

        public long? LastModifierUserId { get; set; }

        public DateTime? LastModificationTime { get; set; }

        public long? DeleterUserId { get; set; }

        public DateTime? DeletionTime { get; set; }
    }
}

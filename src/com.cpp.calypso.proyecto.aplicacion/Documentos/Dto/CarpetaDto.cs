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
    [AutoMap(typeof(Carpeta))]
    [Serializable]
    public class CarpetaDto : EntityDto
    {
        public string Codigo { get; set; }

        public string NombreCorto { get; set; }

        public string NombreCompleto { get; set; }

        public string Descripcion { get; set; }

        public string EstadoNombre { get; set; }

        public int EstadoId { get; set; }

        public bool Publicado { get; set; } = false;

        public int NumeroDocumentos { get; set; }

        public bool IsDeleted { get; set; }

        public long? CreatorUserId { get; set; }

        public DateTime? CreationTime { get; set; }

        public long? LastModifierUserId { get; set; }

        public DateTime? LastModificationTime { get; set; }

        public long? DeleterUserId { get; set; }

        public DateTime? DeletionTime { get; set; }
    }
}

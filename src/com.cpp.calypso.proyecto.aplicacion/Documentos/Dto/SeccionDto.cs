using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using com.cpp.calypso.proyecto.dominio.Documentos;
using JetBrains.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace com.cpp.calypso.proyecto.aplicacion.Documentos.Dto
{
    [AutoMap(typeof(Seccion))]
    [Serializable]
    public class SeccionDto : EntityDto
    {
        public string NombreSeccion { get; set; }

        public string Codigo { get; set; }

        [CanBeNull]
        public string Contenido { get; set; }

        [CanBeNull]
        public string Contenido_Plano { get; set; }

        public int DocumentoId { get; set; }

        public int? SeccionPadreId { get; set; }

        public string NumeroPagina { get; set; }

        public int Ordinal { get; set; }

        public bool IsDeleted { get; set; }

        public long? CreatorUserId { get; set; }

        public DateTime? CreationTime { get; set; }

        public long? LastModifierUserId { get; set; }

        public DateTime? LastModificationTime { get; set; }

        public long? DeleterUserId { get; set; }

        public DateTime? DeletionTime { get; set; }

        public virtual List<ImagenSeccion> Imagenes { get; set; }

        public virtual string nombreDocumento { get; set; }
        public virtual string contenidoCorto { get; set; }
    }
}

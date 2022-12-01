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
    [AutoMap(typeof(Documento))]
    [Serializable]
    public class DocumentoDto : EntityDto
    {
        public string Codigo { get; set; }

        public string Nombre { get; set; }

        public CarpetaDto Carpeta { get; set; }

        public int CarpetaId { get; set; }

        public int CantidadPaginas { get; set; }

        public string TipoDocumentoNombre { get; set; }

        public int TipoDocumentoId { get; set; }

        public bool EsImagen { get; set; }

        public string Imagen { get; set; }

        public int? DocumentoPadreId { get; set; }

        public DocumentoDto DocumentoPadre { get; set; }

        public string DocumentoPadreCodigo { get; set; }

        public bool IsDeleted { get; set; }

        public int orden { get; set; }

        public long? CreatorUserId { get; set; }

        public DateTime? CreationTime { get; set; }

        public long? LastModifierUserId { get; set; }

        public DateTime? LastModificationTime { get; set; }

        public long? DeleterUserId { get; set; }

        public DateTime? DeletionTime { get; set; }
    }
}

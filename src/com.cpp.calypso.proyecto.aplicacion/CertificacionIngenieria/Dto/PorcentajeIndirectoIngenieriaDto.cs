using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using com.cpp.calypso.proyecto.dominio.CertificacionIngenieria;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace com.cpp.calypso.proyecto.aplicacion.CertificacionIngenieria.Dto
{
    [AutoMap(typeof(PorcentajeIndirectoIngenieria))]
    [Serializable]
    public class PorcentajeIndirectoIngenieriaDto : EntityDto
    {
        public int DetalleIndirectosIngenieriaId { get; set; }

        public int ContratoId { get; set; }

        public string ContratoNombre { get; set; }

        public decimal PorcentajeIndirecto { get; set; }

        public decimal Horas { get; set; }

        public bool IsDeleted { get; set; }

        public long? CreatorUserId { get; set; }

        public DateTime CreationTime { get; set; }

        public long? LastModifierUserId { get; set; }

        public DateTime? LastModificationTime { get; set; }

        public long? DeleterUserId { get; set; }

        public DateTime? DeletionTime { get; set; }
    }
}

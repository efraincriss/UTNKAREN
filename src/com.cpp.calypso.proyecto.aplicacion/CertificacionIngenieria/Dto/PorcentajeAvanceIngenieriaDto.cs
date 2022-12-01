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
    [AutoMap(typeof(PorcentajeAvanceIngenieria))]
    [Serializable]
    public class PorcentajeAvanceIngenieriaDto : EntityDto
    {
        public int ProyectoId { get; set; }

        public string ProyectoNombre { get; set; }

        public string ProyectoCodigo { get; set; }

        public DateTime FechaAvance { get; set; }

        public int CatalogoProcentajeId { get; set; }

        public string PorcentajeNombre { get; set; }

        public decimal ValorPorcentaje { get; set; }

        public bool IsDeleted { get; set; }

        public long? CreatorUserId { get; set; }

        public DateTime CreationTime { get; set; }

        public long? LastModifierUserId { get; set; }

        public DateTime? LastModificationTime { get; set; }

        public long? DeleterUserId { get; set; }

        public DateTime? DeletionTime { get; set; }
    }
}

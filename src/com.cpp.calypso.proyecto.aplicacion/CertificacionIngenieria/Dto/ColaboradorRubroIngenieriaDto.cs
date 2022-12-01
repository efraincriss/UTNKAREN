using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using com.cpp.calypso.proyecto.dominio.CertificacionIngenieria;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace com.cpp.calypso.proyecto.aplicacion.CertificacionIngenieria.Dto
{
    [AutoMap(typeof(ColaboradorRubroIngenieria))]
    [Serializable]
    public class ColaboradorRubroIngenieriaDto : EntityDto
    {
        [Required]
        public int ContratoId { get; set; }

        [Required]
        public int ColaboradorId { get; set; }

        public string Identificacion { get; set; }

        public string Nombres { get; set; }
        public string Estado { get; set; }

        public string RubroNombre { get; set; }

        public string NombreContrato { get; set; }

        [Required]
        public int RubroId { get; set; }

        public decimal Tarifa { get; set; }

        public DateTime FechaInicio { get; set; }

        public DateTime? FechaFin { get; set; }

        public int ItemId { get; set; }

        public bool IsDeleted { get; set; }

        public long? CreatorUserId { get; set; }

        public DateTime CreationTime { get; set; }

        public long? LastModifierUserId { get; set; }

        public DateTime? LastModificationTime { get; set; }

        public long? DeleterUserId { get; set; }

        public DateTime? DeletionTime { get; set; }
    }
}

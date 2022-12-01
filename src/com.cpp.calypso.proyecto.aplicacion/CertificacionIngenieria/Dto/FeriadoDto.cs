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
    [AutoMap(typeof(Feriado))]
    [Serializable]
    public class FeriadoDto : EntityDto
    {
        public DateTime FechaInicio { get; set; }

        public DateTime FechaFin { get; set; }

        public int Horas { get; set; }

        public int Anio { get; set; }

        public string Descripcion { get; set; }

        public bool IsDeleted { get; set; }

        public long? CreatorUserId { get; set; }

        public DateTime CreationTime { get; set; }

        public long? LastModifierUserId { get; set; }

        public DateTime? LastModificationTime { get; set; }

        public long? DeleterUserId { get; set; }

        public DateTime? DeletionTime { get; set; }
    }
}

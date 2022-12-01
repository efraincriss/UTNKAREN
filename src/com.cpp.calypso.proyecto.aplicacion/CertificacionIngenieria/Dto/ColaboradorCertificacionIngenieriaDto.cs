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
    [AutoMap(typeof(ColaboradorCertificacionIngenieria))]
    [Serializable]
    public class ColaboradorCertificacionIngenieriaDto : EntityDto
    {
        public int ColaboradorId { get; set; }

        public DateTime FechaDesde { get; set; }

        public DateTime? FechaHasta { get; set; }

        public int? DisciplinaId { get; set; }

        public string DisciplinaNombre { get; set; }

        public int ModalidadId { get; set; }

        public string ModalidadNombre { get; set; }

        public int UbicacionId { get; set; }

        public string UbicacionNombre { get; set; }

        public string CategoriaID { get; set; }

        public int HorasPorDia { get; set; } = 8;

        public Area Area { get; set; }

        public bool AplicaViatico { get; set; } = false;
        public bool AplicaViaticoDirecto { get; set; } = false;

        public bool EsJornal { get; set; } = false;

        public bool EsGastoDirecto { get; set; } = false;


        public bool IsDeleted { get; set; }

        public long? CreatorUserId { get; set; }

        public DateTime CreationTime { get; set; }

        public long? LastModifierUserId { get; set; }

        public DateTime? LastModificationTime { get; set; }

        public long? DeleterUserId { get; set; }

        public DateTime? DeletionTime { get; set; }
    }
}

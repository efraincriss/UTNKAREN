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
    [AutoMap(typeof(DetalleIndirectosIngenieria))]
    [Serializable]
    public class DetalleIndirectosIngenieriaDto : EntityDto
    {
        public DateTime FechaDesde { get; set; }

        public DateTime FechaHasta { get; set; }

        public string ColaboradorNombres { get; set; }

        public string ColaboradorIdentificacion { get; set; }

        public int ColaboradorId { get; set; }

        public string ItemNombre { get; set; }

        public int ColaboradorRubroId { get; set; }

        public decimal DiasLaborados { get; set; }

        public decimal HorasLaboradas { get; set; }

        public bool EsViatico { get; set; }

        public bool Certificado { get; set; }

        public string CertificadoNombre { get; set; }

        public string distribucion_proyectos { get; set; }

        public decimal saldo { get; set; }

        public int? CertificadoId { get; set; }


        public bool IsDeleted { get; set; }

        public long? CreatorUserId { get; set; }

        public DateTime CreationTime { get; set; }

        public long? LastModifierUserId { get; set; }

        public DateTime? LastModificationTime { get; set; }

        public long? DeleterUserId { get; set; }

        public DateTime? DeletionTime { get; set; }

        public virtual string FechaDesdeString { get; set; }
        public virtual string FechaHastaString { get; set; }
        public virtual string ProyectosCodigosString { get; set; }


        /*Colaborador Rubro Valores*/
        public virtual decimal tarifa { get; set; }
        public virtual decimal monto { get; set; }
        public virtual decimal contratoId { get; set; }
        public virtual string mes { get; set; }
    }
}

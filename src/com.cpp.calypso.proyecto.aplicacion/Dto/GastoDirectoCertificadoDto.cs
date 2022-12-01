using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using com.cpp.calypso.proyecto.dominio;
using com.cpp.calypso.proyecto.dominio.CertificacionIngenieria;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace com.cpp.calypso.proyecto.aplicacion.Dto

{
    [AutoMap(typeof(GastoDirectoCertificado))]
    [Serializable]
    public class GastoDirectoCertificadoDto : EntityDto
    {

        public int CertificadoIngenieriaProyectoId { get; set; }
        public TipoGasto TipoGastoId { get; set; }

        public int ColaboradorId { get; set; }
        public int? RubroId { get; set; }
        public int? UnidadId { get; set; }
        public decimal TotalHoras { get; set; }
        public decimal TarifaHoras { get; set; }

        public bool EsDistribucionE500 { get; set; }
        public decimal Tarifa { get; set; }
        public bool migrado { get; set; }

        public int? EspecialidadId { get; set; }
        public Catalogo Especialidad { get; set; }

        public int? UbicacionId { get; set; }
        public Catalogo Ubicacion { get; set; }

        public string UbicacionTrabajo { get; set; } //UIO u OIT

        public string TipoIndirecto { get; set; } //Ingenieria o PyCP

        public string NombreEtapa { get; set; } //ETAP ID IB AB

        public bool EsViatico { get; set; } = false;
        public bool AplicaViatico { get; set; } = false;


        public bool IsDeleted { get; set; }

        public long? CreatorUserId { get; set; }

        public DateTime CreationTime { get; set; }

        public long? LastModifierUserId { get; set; }

        public DateTime? LastModificationTime { get; set; }

        public long? DeleterUserId { get; set; }

        public DateTime? DeletionTime { get; set; }

        public virtual string CertificadoIngenieriaProyectoString { get; set; }
        public virtual string TipoGastoString { get; set; }
        public virtual string ColaboradoresIdentificacion { get; set; }
        public virtual string ColaboradoresNombresCompletos { get; set; }
        public virtual string RubroCodigoString { get; set; }
        public virtual string RubroString { get; set; }
        public virtual string UnidadString { get; set; }
        public virtual decimal MontoTotal { get; set; }
        public virtual string es500String { get; set; }
    }


}


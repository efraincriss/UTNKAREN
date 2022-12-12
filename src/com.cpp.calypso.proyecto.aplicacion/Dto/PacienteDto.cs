﻿using Abp.Application.Services.Dto;
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

    [AutoMap(typeof(Paciente))]
    [Serializable]
    public class PacienteDto : EntityDto
    {

        public string Identificacion { get; set; }
        public string NombresApellidos { get; set; }
        public decimal Peso { get; set; }

        public decimal Edad { get; set; }
        public int SexoId { get; set; }

        public Catalogo Sexo { get; set; }
        public decimal Talla { get; set; }

        public int CentroId { get; set; }
 
        public string GrupoEdad { get; set; }
        public string NivelEducativo { get; set; }
        public string EstadoCivil { get; set; }
        public string ViveSolo { get; set; }
        public string ConsumeAlcohol { get; set; }
        public string ConsumeCigarillo { get; set; }
        public string AutoReporteSalud { get; set; }
        public string Hospitalizacion { get; set; }
        public string Emergencia { get; set; }
        public string HipertencionArterial { get; set; }
        public string InsuficienciaArterial { get; set; }
        public string InsuficienciaCardicaCongestiva { get; set; }
        public string Epoc { get; set; }
        public string EnfermedadCerebroVascular { get; set; }


        public virtual string  sexoString { get; set;}

        public virtual string centroString { get; set; }
        public bool IsDeleted { get; set; }

        public long? CreatorUserId { get; set; }

        public DateTime CreationTime { get; set; }

        public long? LastModifierUserId { get; set; }

        public DateTime? LastModificationTime { get; set; }

        public long? DeleterUserId { get; set; }

        public DateTime? DeletionTime { get; set; }
    }
}

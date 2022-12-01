using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using com.cpp.calypso.proyecto.dominio;
using JetBrains.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace com.cpp.calypso.proyecto.aplicacion.Dto
{
    [AutoMap(typeof(Colaboradores))]
    [Serializable]
    public class ColaboradorReporteDto : EntityDto
    {
        public int? tipo_identificacion { get; set; }
        [CanBeNull]
        public string numero_identificacion { get; set; }
        [CanBeNull]
        public string nombres_apellidos { get; set; }
        public int? id_sap { get; set; }
        [CanBeNull]
        public string posicion { get; set; }
        public DateTime? fecha_ingreso_desde { get; set; }
        public DateTime? fecha_ingreso_hasta { get; set; }
        [CanBeNull]
        public string estado { get; set; }
        public int? grupo_personal { get; set; }
        public int? encargado_personal { get; set; }
        public DateTime? fecha_baja_desde { get; set; }
        public DateTime? fecha_baja_hasta { get; set; }
        [CanBeNull]
        public string motivo_baja { get; set; }
        public int? tipo_ausentismo { get; set; }
        public DateTime? fecha_inicio_desde { get; set; }
        public DateTime? fecha_inicio_hasta { get; set; }
        public DateTime? fecha_fin_desde { get; set; }
        public DateTime? fecha_fin_hasta { get; set; }
    }
}

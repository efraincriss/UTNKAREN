using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using com.cpp.calypso.comun.dominio;
using com.cpp.calypso.proyecto.dominio;
using JetBrains.Annotations;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace com.cpp.calypso.proyecto.aplicacion.Dto
{
    [AutoMap(typeof(ColaboradorRequisitoHistorico))]
    [Serializable]
    public class ColaboradorRequisitoHistoricoDto : EntityDto
    {
        [DisplayName("ID Colaboradores Requisito")]
        public int ColaboradorRequisitoId { get; set; }

        [DisplayName("Colaborador")]
        public int ColaboradoresId { get; set; }

        [DisplayName("Requisito")]
        public int RequisitosId { get; set; }

        [DisplayName("Archivo")]
        public int? ArchivoId { get; set; }

        [DisplayName("Cumple")]
        public bool cumple { get; set; }

        [DataType(DataType.Date)]
        [DisplayName("Fecha Emision")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime? fecha_emision { get; set; }

        [DataType(DataType.Date)]
        [DisplayName("Fecha Caducidad")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime? fecha_caducidad { get; set; }

        [CanBeNull]
        [DisplayName("Observacion")]
        public string observacion { get; set; }

        [Obligado]
        [DisplayName("Vigente")]
        public bool vigente { get; set; } = true;
        
        [DisplayName("Acción")]
        public int catalogo_accion_id { get; set; }

        public static implicit operator ColaboradorRequisitoHistoricoDto(ColaboradorRequisitoDto c)
        {
            ColaboradorRequisitoHistoricoDto r = new ColaboradorRequisitoHistoricoDto();
            r.ColaboradorRequisitoId = c.Id;
            r.ColaboradoresId = c.ColaboradoresId;
            r.RequisitosId = c.RequisitosId;
            r.ArchivoId = c.ArchivoId;
            r.cumple = c.cumple;
            r.fecha_emision = c.fecha_emision;
            r.fecha_caducidad = c.fecha_caducidad;
            r.observacion = c.observacion;
            r.vigente = c.vigente;
           /* r.catalogo_accion_id = c.catalogo_accion_id;*/

            return r;
        }
    }
}

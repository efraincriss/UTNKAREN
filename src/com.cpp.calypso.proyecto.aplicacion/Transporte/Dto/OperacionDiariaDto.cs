using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using com.cpp.calypso.comun.dominio;
using com.cpp.calypso.proyecto.aplicacion.Dto;
using com.cpp.calypso.proyecto.dominio.Transporte;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace com.cpp.calypso.proyecto.aplicacion.Transporte.Dto
{
    [AutoMap(typeof(OperacionDiaria))]
    [Serializable]
    public class OperacionDiariaDto : EntityDto
    {
        public int? VehiculoId { get; set; }

        public Vehiculo Vehiculo { get; set; }

        public int? ChoferId { get; set; }

        public DateTime? FechaInicio { get; set; }

        public DateTime? FechaFin { get; set; }

        public int KilometrajeInicio { get; set; }

        public int KilometrajeFinal { get; set; }

        public string Observacion { get; set; }

        public string Estado { get; set; }

        public DateTime? fs { get; set; }

        public DateTime? fr { get; set; }

        public bool IsDeleted { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using com.cpp.calypso.proyecto.dominio.RecursosHumanos;

namespace com.cpp.calypso.proyecto.aplicacion.RecursosHumanos.Dto
{
    [AutoMap(typeof(ActualizacionSueldo))]
    [Serializable]
    public class ActualizacionSueldoDto : EntityDto
    {
        public DateTime FechaCarga { get; set; }

        public string Observaciones { get; set; }

        public string UrlArchivo { get; set; }

        public int NumeroRegistros { get; set; }

        public DateTime FechaInicio { get; set; }

        public DateTime? FechaFin { get; set; }

        public virtual List<DetalleActualizacionSueldoDto> DetalleActualizacionSueldos { get; set; }
    }
}

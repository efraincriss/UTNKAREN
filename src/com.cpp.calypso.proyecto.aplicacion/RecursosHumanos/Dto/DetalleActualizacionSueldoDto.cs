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
    [AutoMap(typeof(DetalleActualizacionSueldo))]
    [Serializable]
    public class DetalleActualizacionSueldoDto : EntityDto
    {
        public int CatalogoGrupoId { get; set; }

        public string NombreCategoriaEncargado { get; set; }

        public int ActualizacionSueldoId { get; set; }

        public decimal ValorSueldo { get; set; }
    }
}

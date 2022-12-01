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
    [AutoMap(typeof(Capacitacion))]
    [Serializable]
    public class CapacitacionDto : EntityDto
    {
        public int ColaboradoresId { get; set; }

        public string ColaboradorNombre { get; set; }

        public string ColaboradorSap { get; set; }

        public string ColaboradorIdentificacion { get; set; }

        public int CatalogoTipoCapacitacionId { get; set; }

        public string TipoCapacitacionNombre { get; set; }

        public decimal Horas { get; set; }

        public int CatalogoNombreCapacitacionId { get; set; }

        public string NombreCapacitacion { get; set; }

        public string Observaciones { get; set; }

        public string Fuente { get; set; }

        public DateTime Fecha { get; set; }

        /** Utilizado unicamente para estilos en la descarga de plantilla */
        public string Tipo { get; set; }
    }
}

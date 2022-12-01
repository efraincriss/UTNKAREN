using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using com.cpp.calypso.comun.dominio;
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
    [AutoMap(typeof(RutaHorarioVehiculo))]
    [Serializable]
    public class RutaHorarioVehiculoDto : EntityDto
    {

        [Obligado]
        [DisplayName("Ruta")]
            public int RutaHorarioId { get; set; }

        [Obligado]
        [DisplayName("Vehiculo")]
        public int VehiculoId { get; set; }

        [DataType(DataType.Date)]
        [DisplayName("Fecha Desde")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime FechaDesde { get; set; } = DateTime.Now;

        [DataType(DataType.Date)]
        [DisplayName("Fecha Hasta")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime FechaHasta { get; set; } = DateTime.Now;

        [Obligado]
        [DisplayName("Horario")]
        public TimeSpan HorarioSalida { get; set; }

        [Obligado]
        [DisplayName("Duración")]
        public int Duracion { get; set; }

        [Obligado]
        [DisplayName("Horario")]
        public TimeSpan HoraLlegada { get; set; }


        [DisplayName("Observacion")]
        [StringLength(100)]
        public string Observacion { get; set; }

        [Obligado]
        [DisplayName("Estado")]
        public EstadoAsignacionVehiculo Estado { get; set; }


        public virtual string NombreEstado {get;set;}
        public virtual string TipoVehiculo { get; set; }
        public virtual string PlacaVehiculo { get; set; }
        public virtual string CodigoVehiculo { get; set;}
        public virtual int CapacidadVehiculo { get; set; }

        public virtual string FechaDesdeTexto { get; set; }
        public virtual string FechaHastaTexto { get; set; }

    }
}

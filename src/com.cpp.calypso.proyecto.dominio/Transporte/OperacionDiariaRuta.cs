using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using com.cpp.calypso.comun.dominio;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace com.cpp.calypso.proyecto.dominio.Transporte
{
    public class OperacionDiariaRuta : Entity, ISoftDelete
    {
        public int RutaHorarioVehiculoId { get; set; }

        public RutaHorarioVehiculo RutaHorarioVehiculo { get; set; }

        public int OperacionDiariaId { get; set; }

        public OperacionDiaria OperacionDiaria { get; set; }

        public string RutaHorarioVehiculoRef { get; set; }

        public string OperacionDiariaRef { get; set; }

        public DateTime? FechaInicio { get; set; }

        public DateTime? FechaFin { get; set; }

        public DateTime? fs { get; set; }

        public DateTime? fr { get; set; }

        public bool IsDeleted { get; set; }

        public string uid { get; set; }
    }
}

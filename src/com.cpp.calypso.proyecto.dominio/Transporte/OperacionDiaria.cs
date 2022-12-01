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
    public class OperacionDiaria : Entity, ISoftDelete
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

        public string uid { get; set; }
    }
}

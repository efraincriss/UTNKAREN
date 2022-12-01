using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using com.cpp.calypso.comun.dominio;
using com.cpp.calypso.proyecto.dominio;
using com.cpp.calypso.proyecto.dominio.Proveedor;
using JetBrains.Annotations;

namespace com.cpp.calypso.proyecto.aplicacion.Proveedor.Dto
{
    [AutoMap(typeof(TarifaHotel))]
    [Serializable]
    public class TarifaHotelDto : EntityDto
    {
        [Obligado]
        [DisplayName("Tipo Habitación")]
        public int TipoHabitacionId { get; set; }


        [Obligado]
        [DisplayName("Contrato Proveedor")]
        public int ContratoProveedorId { get; set; }

        [Obligado]
        [DisplayName("Capacidad")]
        public int capacidad { get; set; }

        [Obligado]
        [DisplayName("Costo por Persona")]
        public decimal costo_persona { get; set; }

        public string estado_nombre { get; set; }

        public decimal total { get; set; }

        [Obligado] [DisplayName("Estado")] public bool estado { get; set; } = true;

        public string tipo_habitacion_nombre { get; set; }

        public int secuencial { get; set; }
    }
}

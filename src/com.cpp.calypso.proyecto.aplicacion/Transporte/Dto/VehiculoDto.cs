using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using com.cpp.calypso.comun.dominio;
using com.cpp.calypso.proyecto.dominio;
using com.cpp.calypso.proyecto.dominio.Transporte;

namespace com.cpp.calypso.proyecto.aplicacion.Transporte.Dto
{
    [AutoMap(typeof(Vehiculo))]
    [Serializable]
    public class VehiculoDto : EntityDto
    {
        [Obligado]
        [DisplayName("Proveedor")]
        public int ProveedorId { get; set; }

        [Obligado]
        [DisplayName("Código")]
        [StringLength(20)]
        public string Codigo { get; set; }

        [DisplayName("Código Equipo Inventario")]
        [StringLength(10)]
        public string CodigoEquipoInventario { get; set; }

        [Obligado]
        [DisplayName("Tipo Vehículo")]
        public int TipoVehiculoId { get; set; }

        [Obligado]
        [DisplayName("Número Placa")]
        [StringLength(20)]
        public string NumeroPlaca { get; set; }

        [Obligado]
        [DisplayName("Marca")]
        [StringLength(60)]
        public string Marca { get; set; }

        [Obligado]
        [DisplayName("Capacidad")]
        public int Capacidad { get; set; }

        [Obligado]
        [DisplayName("Año Facricación")]
        public int AnioFabricacion { get; set; }

        [Obligado]
        [DisplayName("Color")]
        [StringLength(20)]
        public string Color { get; set; }


        [DataType(DataType.Date)]
        [DisplayName("Fecha Vencimiento Matrícula")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime FechaVencimientoMatricula { get; set; }

        [Obligado]
        [DisplayName("Estado")]
        [StringLength(3)]
        public string Estado { get; set; } = "ACT";

        [DataType(DataType.Date)]
        [DisplayName("Fecha Estado")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime FechaEstado { get; set; }

        public string ProveedorRazonSocial { get; set; }

        public string TipoVehiculo { get; set; }

        public string EstadoNombre { get; set; }

        public int Secuencial { get; set; }

        public string FechaVencimiento { get; set; }
    }
}

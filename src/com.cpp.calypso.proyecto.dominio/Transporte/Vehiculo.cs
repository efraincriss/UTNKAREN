using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using com.cpp.calypso.comun.dominio;

namespace com.cpp.calypso.proyecto.dominio.Transporte
{
    [Serializable]
    public class Vehiculo : Entity, IFullAudited
    {
        [Obligado]
        [DisplayName("Proveedor")]
        public int ProveedorId { get; set; }
        public Proveedor.Proveedor Proveedor { get; set; }

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
        public Catalogo TipoVehiculo { get; set; }

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

        public string GetEstado()
        {
            switch (Estado)
            {
                case "ACT":
                    return "ACTIVO";
                case "INA":
                    return "INACTIVO";
                case "MAN":
                    return "MANTENIMIENTO";
                default:
                    return "ACTIVO";
            }
        }

        public bool IsDeleted { get; set; }

        public DateTime CreationTime { get; set; }

        public long? CreatorUserId { get; set; }

        public DateTime? LastModificationTime { get; set; }

        public long? LastModifierUserId { get; set; }

        public DateTime? DeletionTime { get; set; }

        public long? DeleterUserId { get; set; }
    }
}

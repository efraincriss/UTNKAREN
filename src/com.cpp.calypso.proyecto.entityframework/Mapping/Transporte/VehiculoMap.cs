using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using com.cpp.calypso.proyecto.dominio.Transporte;

namespace com.cpp.calypso.proyecto.entityframework.Mapping.Transporte
{
    public class VehiculoMap : EntityTypeConfiguration<Vehiculo>
    {
        public VehiculoMap()
        {
            ToTable("vehiculos", "SCH_TRANSPORTES");

            HasKey(d => d.Id);
            Property(d => d.Id)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            Property(d => d.IsDeleted).HasColumnName("eliminado");
            Property(d => d.CreationTime).HasColumnName("fecha_creacion");
            Property(d => d.CreatorUserId).HasColumnName("usuario_creador");
            Property(d => d.LastModificationTime).HasColumnName("ultima_modificacion");
            Property(d => d.LastModifierUserId).HasColumnName("usuario_ultima_modificacion");
            Property(d => d.DeletionTime).HasColumnName("fecha_eliminacion");
            Property(d => d.DeleterUserId).HasColumnName("usuario_eliminacion");
            Property(d => d.Codigo).HasColumnName("codigo");
            Property(d => d.ProveedorId).HasColumnName("proveedor_id");
            Property(d => d.Codigo).HasColumnName("codigo");
            Property(d => d.TipoVehiculoId).HasColumnName("catalogo_tipo_vehiculo_id");
            Property(d => d.NumeroPlaca).HasColumnName("numero_placa");
            Property(d => d.Marca).HasColumnName("marca");
            Property(d => d.AnioFabricacion).HasColumnName("anio_fabricacion");
            Property(d => d.FechaVencimientoMatricula).HasColumnName("fecha_vencimiento_matricula");
            Property(d => d.Estado).HasColumnName("estado");
            Property(d => d.FechaEstado).HasColumnName("fecha_estado");
            Property(d => d.Color).HasColumnName("color");
            Property(d => d.Capacidad).HasColumnName("capacidad");
            Property(d => d.CodigoEquipoInventario).HasColumnName("codigo_equipo_inventario");
        }
    }
}

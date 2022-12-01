using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using com.cpp.calypso.proyecto.dominio.Proveedor;

namespace com.cpp.calypso.proyecto.entityframework.Mapping.Proveedor
{
    public class HabitacionMap : EntityTypeConfiguration<Habitacion>
    {
        public HabitacionMap()
        {

            ToTable("habitaciones", "SCH_SERVICIOS");

            HasKey(d => d.Id);
            Property(d => d.Id)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            HasRequired(p => p.Proveedor)
                .WithMany()
                .HasForeignKey(s => s.ProveedorId)
                .WillCascadeOnDelete(false);

            HasRequired(p => p.TipoHabitacion)
                .WithMany()
                .HasForeignKey(s => s.TipoHabitacionId)
                .WillCascadeOnDelete(false);

            Property(d => d.ProveedorId).HasColumnName("proveedor_id");
            Property(d => d.TipoHabitacionId).HasColumnName("tipo_habitacion_id");
            Property(d => d.IsDeleted).HasColumnName("vigente");
        }
    }
}

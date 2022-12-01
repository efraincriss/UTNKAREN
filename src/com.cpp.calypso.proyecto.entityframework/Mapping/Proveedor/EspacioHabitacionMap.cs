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
    public class EspacioHabitacionMap : EntityTypeConfiguration<EspacioHabitacion>
    {
        public EspacioHabitacionMap()
        {
            ToTable("espacios_habitaciones", "SCH_SERVICIOS");

            HasKey(d => d.Id);
            Property(d => d.Id)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            Property(d => d.IsDeleted).HasColumnName("vigente");
            Property(d => d.HabitacionId).HasColumnName("habitacion_id");
        }
    }
}

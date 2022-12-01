using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using com.cpp.calypso.proyecto.dominio;

namespace com.cpp.calypso.proyecto.entityframework
{
    public class OrdenServicioMap : EntityTypeConfiguration<OrdenServicio>
    {
        public OrdenServicioMap()
        {
            ToTable("ordenes_servicio", "SCH_PROYECTOS");

            HasKey(d => d.Id);
            Property(d => d.Id)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            Property(c => c.fecha_orden_servicio).IsOptional();

            Property(c => c.monto_aprobado_os).HasPrecision(20, 2);
            Property(c => c.monto_aprobado_construccion).HasPrecision(20, 2);
            Property(c => c.monto_aprobado_suministros).HasPrecision(20, 2);
            Property(c => c.monto_aprobado_ingeniería).HasPrecision(20, 2);
        }
    }
}

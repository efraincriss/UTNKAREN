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
    public class AvanceObraMap : EntityTypeConfiguration<AvanceObra>
    {
        public AvanceObraMap()
        {
            ToTable("avances_obra", "SCH_PROYECTOS");

            HasKey(d => d.Id);
            Property(d => d.Id)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            Property(c => c.monto_construccion).HasPrecision(20, 2);
            Property(c => c.monto_ingenieria).HasPrecision(20, 2);
            Property(c => c.monto_suministros).HasPrecision(20, 2);
            Property(c => c.monto_total).HasPrecision(20, 2);

            Property(c => c.fecha_desde).IsOptional();
            Property(c => c.fecha_hasta).IsOptional();
            Property(c => c.fecha_presentacion).IsOptional();
        }
    }
}

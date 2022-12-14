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
    public class ComputoPresupuestoMap : EntityTypeConfiguration<ComputoPresupuesto>
    {
        public ComputoPresupuestoMap()
        {
            ToTable("computos_presupuesto", "SCH_PROYECTOS");

            HasKey(d => d.Id);
            Property(d => d.Id)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);


            Property(c => c.cantidad).HasPrecision(20, 4);

            Property(c => c.cantidad_eac).HasPrecision(20, 4);
            Property(c => c.costo_total).HasPrecision(20, 4);
        }
    }
}

using com.cpp.calypso.proyecto.dominio;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace com.cpp.calypso.proyecto.entityframework.Mapping
{
    public class ComputoComercialMap : EntityTypeConfiguration<ComputoComercial>
    {

        public ComputoComercialMap()
        {
            ToTable("computo_comercial", "SCH_PROYECTOS");

            HasKey(d => d.Id);
            Property(d => d.Id)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            Property(c => c.cantidad).HasPrecision(20, 4);

            Property(c => c.cantidad_eac).HasPrecision(20, 4);
            Property(c => c.costo_total).HasPrecision(20, 4);

        }
    }
}

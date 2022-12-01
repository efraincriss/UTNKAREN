using com.cpp.calypso.proyecto.dominio;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace com.cpp.calypso.proyecto.entityframework
{
    public class RsoCabeceraMap : EntityTypeConfiguration<RsoCabecera>
    {
        public RsoCabeceraMap()
        {
            ToTable("rso_cabeceras", "SCH_PROYECTOS");

            HasKey(d => d.Id);
            Property(d => d.Id)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);


            Property(c => c.avance_real_acumulado).HasPrecision(18, 6); //

        }
    }
}
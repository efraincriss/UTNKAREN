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
    public class CurvaRSOmap : EntityTypeConfiguration<CurvaProyectoRSO>
    {
        public CurvaRSOmap()
        {
            ToTable("curvas_rso", "SCH_PROYECTOS");

            HasKey(d => d.Id);
            Property(d => d.Id)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            Property(c => c.valor_previsto).HasPrecision(20, 6);
            Property(c => c.valor_previsto_acumulado).HasPrecision(20, 6);
            Property(c => c.valor_real).HasPrecision(20, 6);
            Property(c => c.valor_real_acumulado).HasPrecision(20, 6);

        }
    }
}

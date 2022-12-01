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
    public class ObraDisruptivoMap : EntityTypeConfiguration<ObraDisruptivo>
    {
        public ObraDisruptivoMap()
        {
            ToTable("obras_disruptivo", "SCH_PROYECTOS");

            HasKey(d => d.Id);
            Property(d => d.Id)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            Property(c => c.numero_horas).HasPrecision(10, 2);
            Property(c => c.numero_horas_hombres).HasPrecision(10, 2);
        }
    }
}

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
    public class NovedadMap : EntityTypeConfiguration<Novedad>
    {
        public NovedadMap()
        {
            ToTable("novedades_requerimientos", "SCH_PROYECTOS");

            HasKey(d => d.Id);
            Property(d => d.Id)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            HasRequired(a => a.Requerimiento)
                .WithMany(f => f.Novedades)
                .HasForeignKey(u => u.RequerimientoId);
        }
    }
}

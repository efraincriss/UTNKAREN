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
    public class JustificacionViandaMap : EntityTypeConfiguration<JustificacionVianda>
    {
        public JustificacionViandaMap()
        {
            ToTable("justificaciones_viandas", "SCH_SERVICIOS");

            HasKey(d => d.Id);
            Property(d => d.Id)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            HasRequired(p => p.SolicitudVianda)
             .WithMany()
             .HasForeignKey(s => s.SolicitudViandaId)
             .WillCascadeOnDelete(false);
        }
    }
}

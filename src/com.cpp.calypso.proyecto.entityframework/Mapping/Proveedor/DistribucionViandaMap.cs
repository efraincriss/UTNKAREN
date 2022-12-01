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
    public class DistribucionViandaMap : EntityTypeConfiguration<DistribucionVianda>
    {
        public DistribucionViandaMap()
        {
            ToTable("distribuciones_viandas", "SCH_SERVICIOS");

            HasKey(d => d.Id);
            Property(d => d.Id)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);


            HasRequired(p => p.tipo_comida)
            .WithMany()
            .HasForeignKey(s => s.tipo_comida_id)
            .WillCascadeOnDelete(false);

            HasOptional(p => p.conductor_asignado)
            .WithMany()
            .HasForeignKey(c => c.conductor_asignado_id)
            .WillCascadeOnDelete(false);
        }
    }
}

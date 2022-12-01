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
    public class ComputosTemporalMap : EntityTypeConfiguration<ComputosTemporal>
    {
        public ComputosTemporalMap()
        {
            ToTable("computos_temporal", "SCH_PROYECTOS");

            HasKey(d => d.Id);
            Property(d => d.Id)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            //Add evitar error:
            //Introducing FOREIGN KEY constraint 'FK_SCH_PROYECTOS.computos_temporal_SCH_PROYECTOS.wbs_WbsId' on table 'computos_temporal' may cause cycles or multiple cascade paths. Specify ON DELETE NO ACTION 
            HasRequired(a => a.Wbs)
             .WithMany()
             .HasForeignKey(u => u.WbsId)
             .WillCascadeOnDelete(false);


        }
    }
}

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
    public class GRMap : EntityTypeConfiguration<GR>
    {
        public GRMap()
        {
            ToTable("grs", "SCH_PROYECTOS");

            HasKey(d => d.Id);
            Property(d => d.Id)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);


            //Add evitar error:
            //Introducing FOREIGN KEY constraint 'FK_SCH_PROYECTOS.grs_SCH_PROYECTOS.proyectos_ProyectoId' on table 'grs' may cause cycles or multiple cascade paths. Specify ON DELETE NO ACTION or ON UPDATE NO ACTION, or modify other FOREIGN KEY constraints.
            HasRequired(a => a.Proyecto)
             .WithMany()
             .HasForeignKey(u => u.ProyectoId)
             .WillCascadeOnDelete(false);
        }
    }
}

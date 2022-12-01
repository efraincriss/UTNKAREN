using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using com.cpp.calypso.proyecto.dominio;

namespace com.cpp.calypso.proyecto.entityframework.Mapping
{
    public class ArchivoContratoMap : EntityTypeConfiguration<ArchivosContrato>
    {
        public ArchivoContratoMap()
        {
            ToTable("archivos_contratos", "SCH_PROYECTOS");

            HasKey(d => d.Id);
            Property(d => d.Id)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);


            //Add evitar error:
            //Introducing FOREIGN KEY constraint 'FK_SCH_PROYECTOS.archivos_contratos_SCH_PROYECTOS.contratos_ContratoId' on table 'archivos_contratos' may cause cycles or multiple cascade paths. Specify ON DELETE NO ACTION or ON UPDATE NO ACTION, or modify other FOREIGN KEY constraints.
            HasRequired(p => p.Contratos)
              .WithMany()
              .HasForeignKey(s => s.ContratoId)
              .WillCascadeOnDelete(false);

        }
    }
}

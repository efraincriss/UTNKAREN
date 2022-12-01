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
    public class DetalleAvanceIngenieriaMap : EntityTypeConfiguration<DetalleAvanceIngenieria>
    {
        public DetalleAvanceIngenieriaMap()
        {
            ToTable("detalles_avance_ingenieria", "SCH_PROYECTOS");
            HasKey(d => d.Id);
            Property(d => d.Id)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            Property(c => c.cantidad_horas).HasPrecision(20, 2);

            //Add evitar error:
            //'FK_SCH_PROYECTOS.detalles_avance_ingenieria_SCH_PROYECTOS.computos_ComputoId' on table 'detalles_avance_ingenieria' may cause cycles or multiple cascade paths. Specify ON DELETE NO ACTION or ON UPDATE NO ACTION, or modify other FOREIGN KEY constraints.
            HasRequired(a => a.Computo)
             .WithMany()
             .HasForeignKey(u => u.ComputoId)
             .WillCascadeOnDelete(false);
        }
    }
}

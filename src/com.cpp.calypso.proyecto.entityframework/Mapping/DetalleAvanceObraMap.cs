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
    public class DetalleAvanceObraMap : EntityTypeConfiguration<DetalleAvanceObra>
    {
        public DetalleAvanceObraMap()
        {
            ToTable("detalles_avance_obra", "SCH_PROYECTOS");

            HasKey(d => d.Id);
            Property(d => d.Id)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            Property(c => c.cantidad_diaria).HasPrecision(18, 2);
            Property(c => c.cantidad_acumulada_anterior).HasPrecision(18, 6);

            Property(c => c.cantidad_diaria).HasPrecision(18, 6);
            Property(c => c.total).HasPrecision(18, 6);
            Property(c => c.precio_unitario).HasPrecision(10, 2);
      

            Property(c => c.fecha_registro).IsOptional();

            //Add evitar error:
            //'FK_SCH_PROYECTOS.detalles_avance_obra_SCH_PROYECTOS.computos_ComputoId' on table 'detalles_avance_obra' may cause cycles or multiple cascade paths. Specify ON DELETE NO ACTION or ON UPDATE NO ACTION, or modify other FOREIGN KEY constraints.
            HasRequired(a => a.Computo)
             .WithMany()
             .HasForeignKey(u => u.ComputoId)
             .WillCascadeOnDelete(false);
        }
    }
}

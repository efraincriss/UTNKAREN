
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
    class ObraAdicionalMap : EntityTypeConfiguration<ObraAdicional>
    {
        public ObraAdicionalMap()
        {
            ToTable("obras_adicionales", "SCH_PROYECTOS");

            HasKey(d => d.Id);
            Property(d => d.Id)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            Property(c => c.cantidad).HasPrecision(10, 2);
            Property(c => c.precio_unitario).HasPrecision(10, 2);
            Property(c => c.costo_total).HasPrecision(10, 2);
            Property(c => c.precio_base).HasPrecision(10, 2);
            Property(c => c.precio_incrementado).HasPrecision(10, 2);
            Property(c => c.precio_ajustado).HasPrecision(10, 2);

            //Add evitar error:
            //Introducing FOREIGN KEY constraint 'FK_SCH_PROYECTOS.obras_adicionales_SCH_PROYECTOS.wbs_WbsId' on table 'obras_adicionales' may cause cycles or multiple cascade paths. Specify ON DELETE NO ACTION or ON UPDATE NO ACTION, or modify other FOREIGN KEY constraints.
            HasRequired(a => a.Wbs)
             .WithMany()
             .HasForeignKey(u => u.WbsId)
             .WillCascadeOnDelete(false);
        }
    }
}

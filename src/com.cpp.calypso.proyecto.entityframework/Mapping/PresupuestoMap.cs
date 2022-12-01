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
    public class PresupuestoMap : EntityTypeConfiguration<Presupuesto>
    {
        public PresupuestoMap()
        {
            ToTable("presupuestos", "SCH_PROYECTOS");

            HasKey(d => d.Id);
            Property(d => d.Id)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            Property(c => c.monto_ingenieria).HasPrecision(20, 2);
            Property(c => c.monto_construccion).HasPrecision(20, 2);
            Property(c => c.monto_suministros).HasPrecision(20, 2);

            //Add evitar error:
            //'FK_SCH_PROYECTOS.presupuestos_SCH_CATALOGOS.catalogos_OrigenDeDatosId' on table 'presupuestos' may cause cycles or multiple cascade paths. Specify ON DELETE NO ACTION or ON UPDATE NO ACTION, or modify other FOREIGN KEY constraints.
            HasRequired(p => p.OrigenDatos)
              .WithMany()
              .HasForeignKey(s => s.OrigenDeDatosId)
              .WillCascadeOnDelete(false);
        }
    }
}

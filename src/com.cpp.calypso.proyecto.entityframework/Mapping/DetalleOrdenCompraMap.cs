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
    public class DetalleOrdenCompraMap : EntityTypeConfiguration<DetalleOrdenCompra>
    {

        public DetalleOrdenCompraMap()
        {
            ToTable("detalles_orden_compra", "SCH_PROYECTOS");

            HasKey(d => d.Id);
            Property(d => d.Id)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            //'FK_SCH_PROYECTOS.detalles_orden_compra_SCH_PROYECTOS.computos_ComputoId' on table 'detalles_orden_compra' may cause cycles or multiple cascade paths. Specify ON DELETE NO ACTION or ON UPDATE NO ACTION, or modify other FOREIGN KEY constraints.
            HasRequired(p => p.Computo)
             .WithMany()
             .HasForeignKey(s => s.ComputoId)
             .WillCascadeOnDelete(false);

            HasRequired(p => p.OrdenCompra)
           .WithMany()
           .HasForeignKey(s => s.OrdenCompraId)
           .WillCascadeOnDelete(false);

            Property(c => c.valor).HasPrecision(18, 8); //

        }
    }
}

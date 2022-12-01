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
    public class RetencionFacturaMap : EntityTypeConfiguration<RetencionFactura>
    {
        public RetencionFacturaMap()
        {
            ToTable("retenciones_facturas", "SCH_PROYECTOS");

            HasKey(d => d.Id);
            Property(d => d.Id)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            //'FK_SCH_PROYECTOS.retenciones_facturas_SCH_PROYECTOS.facturas_FacturaId' on table 'retenciones_facturas' may cause cycles or multiple cascade paths. Specify ON DELETE NO ACTION or ON UPDATE NO ACTION, or modify other FOREIGN KEY constraints.
            HasRequired(p => p.Factura)
            .WithMany()
            .HasForeignKey(s => s.FacturaId)
            .WillCascadeOnDelete(false);
        }
    }
}

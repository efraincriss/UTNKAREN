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
   public class FacturaMap : EntityTypeConfiguration<Factura>
    {
        public FacturaMap()
        {
            ToTable("facturas", "SCH_PROYECTOS");

            HasKey(d => d.Id);
            Property(d => d.Id)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);


            //Add evitar error:
            //Introducing FOREIGN KEY constraint 'FK_SCH_PROYECTOS.facturas_SCH_PROYECTOS.clientes_ClienteId' on table 'facturas' may cause cycles or multiple cascade paths. Specify ON DELETE NO ACTION or ON UPDATE NO ACTION, or modify other FOREIGN KEY constraints.
            HasRequired(a => a.Cliente)
            .WithMany()
            .HasForeignKey(u => u.ClienteId)
            .WillCascadeOnDelete(false);

            //Add evitar error:
            //Introducing FOREIGN KEY constraint 'FK_SCH_PROYECTOS.facturas_SCH_PROYECTOS.empresas_EmpresaId' on table 'facturas' may cause cycles or multiple cascade paths. Specify ON DELETE NO ACTION or ON UPDATE NO ACTION, or modify other FOREIGN KEY constraints.
            HasRequired(a => a.Empresa)
           .WithMany()
           .HasForeignKey(u => u.EmpresaId)
           .WillCascadeOnDelete(false);
        }
    }
    
}

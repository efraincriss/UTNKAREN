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
	public class NovedadProveedorMap : EntityTypeConfiguration<NovedadProveedor>
    {
        public NovedadProveedorMap()
        {
            ToTable("novedades_proveedor", "SCH_PROVEEDORES");

            Property(d => d.Id)
               .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);


            // Relationships
            this.HasRequired(t => t.Proveedor)
                .WithMany(t => t.novedades)
                .HasForeignKey(d => d.ProveedorId);

            HasOptional(p => p.documentacion)
          .WithMany()
          .HasForeignKey(s => s.documentacion_id)
          .WillCascadeOnDelete(false);
        }
    }
}

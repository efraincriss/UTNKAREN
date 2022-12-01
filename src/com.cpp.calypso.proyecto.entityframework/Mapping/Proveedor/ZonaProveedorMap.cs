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
    public class ZonaProveedorMap : EntityTypeConfiguration<ZonaProveedor>
    {
        public ZonaProveedorMap()
        {
            ToTable("zonas_proveedor", "SCH_PROVEEDORES");

            HasKey(d => d.Id);
            Property(d => d.Id)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            // Relationships
            this.HasRequired(t => t.Proveedor)
                .WithMany(t => t.zonas)
                .HasForeignKey(d => d.ProveedorId);

        }
    }
}

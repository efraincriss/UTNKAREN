using com.cpp.calypso.proyecto.dominio;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace com.cpp.calypso.proyecto.entityframework.Mapping
{
    public class MenuProveedorMap : EntityTypeConfiguration<MenuProveedor>
    {
        public MenuProveedorMap()
        {
            ToTable("menus_proveedor", "SCH_PROVEEDORES");

            Property(d => d.Id)
               .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
 
            // Relationships
            this.HasRequired(t => t.Proveedor)
                .WithMany(t => t.menus)
                .HasForeignKey(d => d.ProveedorId);

          
        }
    }
}

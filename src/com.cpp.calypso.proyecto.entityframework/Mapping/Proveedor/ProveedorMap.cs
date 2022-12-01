using com.cpp.calypso.proyecto.dominio;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace com.cpp.calypso.proyecto.entityframework.Mapping
{
    public class ProveedorMap : EntityTypeConfiguration<dominio.Proveedor.Proveedor>
    {
        public ProveedorMap()
        {
            ToTable("proveedores", "SCH_PROVEEDORES");

            Property(d => d.Id)
               .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);


            HasRequired(p => p.contacto)
               .WithMany()
               .HasForeignKey(s => s.contacto_id)
               .WillCascadeOnDelete(false);


            HasRequired(p => p.tipo_proveedor)
            .WithMany()
            .HasForeignKey(s => s.tipo_proveedor_id)
            .WillCascadeOnDelete(false);


            HasOptional(p => p.documentacion)
            .WithMany()
            .HasForeignKey(s => s.documentacion_id)
            .WillCascadeOnDelete(false);

        }
    }
}

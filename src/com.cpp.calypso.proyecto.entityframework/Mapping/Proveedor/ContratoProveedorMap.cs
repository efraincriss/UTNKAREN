using com.cpp.calypso.proyecto.dominio;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace com.cpp.calypso.proyecto.entityframework.Mapping
{
    public class ContratoProveedorMap : EntityTypeConfiguration<ContratoProveedor>
    {
        public ContratoProveedorMap()
        {
            ToTable("contratos_proveedores", "SCH_PROVEEDORES");

            HasKey(d => d.Id);
            Property(d => d.Id)
               .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);


            // Relationships
            this.HasRequired(t => t.Proveedor)
                .WithMany(t => t.contratos)
                .HasForeignKey(d => d.ProveedorId);


            HasOptional(p => p.documentacion)
          .WithMany()
          .HasForeignKey(s => s.documentacion_id)
          .WillCascadeOnDelete(false);


        }
    }
}

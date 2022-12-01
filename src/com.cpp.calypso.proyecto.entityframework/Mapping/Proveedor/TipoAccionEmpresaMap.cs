using com.cpp.calypso.proyecto.dominio;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace com.cpp.calypso.proyecto.entityframework.Mapping
{
    public class TipoAccionEmpresaMap : EntityTypeConfiguration<TipoAccionEmpresa>
    {
        public TipoAccionEmpresaMap()
        {
            ToTable("tipos_acciones_empresas", "SCH_SERVICIOS");

            HasKey(d => d.Id);
            Property(d => d.Id)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            HasRequired(p => p.tipo_comida)
           .WithMany()
           .HasForeignKey(s => s.tipo_comida_id)
           .WillCascadeOnDelete(false);

            HasRequired(p => p.Accion)
           .WithMany()
           .HasForeignKey(s => s.AccionId)
           .WillCascadeOnDelete(false);

        }
    }
}

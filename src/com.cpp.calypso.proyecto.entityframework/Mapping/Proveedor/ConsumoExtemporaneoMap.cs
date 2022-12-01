using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using com.cpp.calypso.proyecto.dominio.Proveedor;

namespace com.cpp.calypso.proyecto.entityframework.Mapping.Proveedor
{
    public class ConsumoExtemporaneoMap : EntityTypeConfiguration<ConsumoExtemporaneo>
    {
        public ConsumoExtemporaneoMap()
        {
            ToTable("consumos_extemporaneos", "SCH_SERVICIOS");

            HasKey(d => d.Id);
            Property(d => d.Id)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            HasRequired(p => p.Proveedor)
                .WithMany()
                .HasForeignKey(s => s.ProveedorId)
                .WillCascadeOnDelete(false);

            HasRequired(p => p.TipoComida)
                .WithMany()
                .HasForeignKey(s => s.TipoComidaId)
                .WillCascadeOnDelete(false);

            Property(d => d.ProveedorId).HasColumnName("proveedor_id");
            Property(d => d.Fecha).HasColumnName("fecha");
            Property(d => d.TipoComidaId).HasColumnName("tipo_comida_id");
            Property(d => d.DocumentoRespaldoId).HasColumnName("documento_respaldo_id");

            Property(d => d.IsDeleted).HasColumnName("eliminado");
            Property(d => d.CreationTime).HasColumnName("fecha_creacion");
            Property(d => d.CreatorUserId).HasColumnName("usuario_creador");
            Property(d => d.LastModificationTime).HasColumnName("ultima_modificacion");
            Property(d => d.LastModifierUserId).HasColumnName("usuario_ultima_modificacion");
            Property(d => d.DeletionTime).HasColumnName("fecha_eliminacion");
            Property(d => d.DeleterUserId).HasColumnName("usuario_eliminacion");
        }
    }
}

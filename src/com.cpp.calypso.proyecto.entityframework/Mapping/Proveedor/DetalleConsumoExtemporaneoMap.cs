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
    public class DetalleConsumoExtemporaneoMap : EntityTypeConfiguration<DetalleConsumoExtemporaneo>
    {
        public DetalleConsumoExtemporaneoMap()
        {
            ToTable("detalles_consumos_extemporaneos", "SCH_SERVICIOS");

            HasKey(d => d.Id);
            Property(d => d.Id)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            HasRequired(p => p.ConsumoExtemporaneo)
                .WithMany()
                .HasForeignKey(s => s.ConsumoExtemporaneoId)
                .WillCascadeOnDelete(false);


            HasRequired(p => p.Colaborador)
                .WithMany()
                .HasForeignKey(s => s.ColaboradorId)
                .WillCascadeOnDelete(false);

            Property(d => d.ConsumoExtemporaneoId).HasColumnName("consumo_extemporaneo_id");
            Property(d => d.ColaboradorId).HasColumnName("colaborador_id");
            Property(d => d.Observaciones).HasColumnName("observaciones");
            Property(d => d.Liquidado).HasColumnName("liquidado");
            Property(d => d.LiquidacionDetalleId).HasColumnName("liquidacion_detalle_id");

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

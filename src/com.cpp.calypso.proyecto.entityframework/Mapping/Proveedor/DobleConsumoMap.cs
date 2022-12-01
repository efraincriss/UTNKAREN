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
    public class DobleConsumoMap : EntityTypeConfiguration<DobleConsumo>
    {
        public DobleConsumoMap()
        {
            ToTable("dobles_consumos", "SCH_SERVICIOS");

            HasKey(d => d.Id);
            Property(d => d.Id)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            Property(d => d.Id).HasColumnName("id");
            Property(d => d.TipoComidaId).HasColumnName("tipo_comida_id");
            Property(d => d.ColaboradorId).HasColumnName("colaborador_id");
            Property(d => d.ProveedorId).HasColumnName("proveedor_id");
            Property(d => d.Fecha).HasColumnName("fecha");
            Property(d => d.OrigenConsumoId).HasColumnName("origen_consumo_id");
            Property(d => d.Identificador).HasColumnName("identificador");
            Property(d => d.IsDeleted).HasColumnName("vigente");
            Property(d => d.Version).HasColumnName("m_version");


            this.HasRequired(t => t.Proveedor)
                .WithMany()
                .HasForeignKey(d => d.ProveedorId)
                .WillCascadeOnDelete(false);

            this.HasRequired(t => t.Colaborador)
                .WithMany()
                .HasForeignKey(d => d.ColaboradorId)
                .WillCascadeOnDelete(false);

            this.HasRequired(t => t.TipoComida)
                .WithMany()
                .HasForeignKey(d => d.TipoComidaId)
                .WillCascadeOnDelete(false);

        }
    }
}

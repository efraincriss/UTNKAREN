using com.cpp.calypso.proyecto.dominio.CertificacionIngenieria;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace com.cpp.calypso.proyecto.entityframework.Mapping.CertificacionIngenieria
{
    public class ColaboradorRubroIngenieriaMap : EntityTypeConfiguration<ColaboradorRubroIngenieria>
    {
        public ColaboradorRubroIngenieriaMap()
        {
            ToTable("colaboradores_rubro_ingenieria", "SCH_INGENIERIA");

            HasKey(d => d.Id);
            Property(d => d.Id)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            Property(d => d.Id).HasColumnName("id");
            Property(d => d.ContratoId).HasColumnName("contrato_id");
            Property(d => d.ColaboradorId).HasColumnName("colaborador_id");
            Property(d => d.RubroId).HasColumnName("rubro_id");
            Property(d => d.Tarifa).HasColumnName("tarifa");
            Property(d => d.FechaInicio).HasColumnName("fecha_inicio");
            Property(d => d.FechaInicio).HasColumnName("fecha_fin");

            Property(d => d.IsDeleted).HasColumnName("vigente");
            Property(d => d.CreationTime).HasColumnName("fecha_creacion");
            Property(d => d.CreatorUserId).HasColumnName("usuario_creacion");
            Property(d => d.LastModificationTime).HasColumnName("fecha_actualizacion");
            Property(d => d.LastModifierUserId).HasColumnName("usuario_actualizacion");
            Property(d => d.DeletionTime).HasColumnName("fecha_eliminacion");
            Property(d => d.DeleterUserId).HasColumnName("usuario_eliminacion");
        }
    }
}

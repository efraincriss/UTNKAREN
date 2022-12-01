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
    public class PorcentajeIndirectoIngenieriaMap : EntityTypeConfiguration<PorcentajeIndirectoIngenieria>
    {
        public PorcentajeIndirectoIngenieriaMap()
        {
            ToTable("porcentajes_indirectos_ingenieria", "SCH_INGENIERIA");

            HasKey(d => d.Id);
            Property(d => d.Id)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            Property(d => d.Id).HasColumnName("id");
            Property(d => d.ContratoId).HasColumnName("contrato_id");
            Property(d => d.DetalleIndirectosIngenieriaId).HasColumnName("detalle_indirectos_ingenieria");
            Property(d => d.PorcentajeIndirecto).HasColumnName("porcentaje_indirecto");
            Property(d => d.Horas).HasColumnName("horas");

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

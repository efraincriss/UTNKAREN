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
    public class PorcentajeAvanceIngenieriaMap : EntityTypeConfiguration<PorcentajeAvanceIngenieria>
    {
        public PorcentajeAvanceIngenieriaMap()
        {
            ToTable("porcentajes_avances_ingenieria", "SCH_INGENIERIA");

            HasKey(d => d.Id);
            Property(d => d.Id)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            Property(d => d.Id).HasColumnName("id");
            Property(d => d.ProyectoId).HasColumnName("proyecto_id");
            Property(d => d.FechaAvance).HasColumnName("fecha_avance");
            Property(d => d.CatalogoProcentajeId).HasColumnName("catalogo_porcentaje_id");
            Property(d => d.ValorPorcentaje).HasColumnName("valor_porcentaje");

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

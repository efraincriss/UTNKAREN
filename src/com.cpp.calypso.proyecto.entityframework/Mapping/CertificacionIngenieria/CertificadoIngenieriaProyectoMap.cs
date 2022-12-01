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

    public class CertificadoIngenieriaProyectoMap : EntityTypeConfiguration<CertificadoIngenieriaProyecto>
    {
        public CertificadoIngenieriaProyectoMap()
        {
            ToTable("certificados_ingenieria", "SCH_INGENIERIA");

            HasKey(d => d.Id);
            Property(d => d.Id)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            Property(d => d.Id).HasColumnName("id");
            Property(d => d.PorcentajeParticipacionDirectos).HasColumnName("porcentaje_participacion_directos");

            Property(d => d.IsDeleted).HasColumnName("vigente");
            Property(d => d.CreationTime).HasColumnName("fecha_creacion");
            Property(d => d.CreatorUserId).HasColumnName("usuario_creacion");
            Property(d => d.LastModificationTime).HasColumnName("fecha_actualizacion");
            Property(d => d.LastModifierUserId).HasColumnName("usuario_actualizacion");
            Property(d => d.DeletionTime).HasColumnName("fecha_eliminacion");
            Property(d => d.DeleterUserId).HasColumnName("usuario_eliminacion");
            Property(c => c.HorasActualCertificadas).HasPrecision(24, 16); //
            Property(c => c.HorasAnteriorCertificadas).HasPrecision(24, 16); //
            Property(c => c.MontoAnteriorCertificado).HasPrecision(24, 16); //

            Property(c => c.MontoActualCertificado).HasPrecision(24, 16); //

            Property(c => c.HorasPresupuestadas).HasPrecision(24, 16); //

        }
    }
}



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

    public class AvancePorcentajeProyectoMap : EntityTypeConfiguration<AvancePorcentajeProyecto>
    {
        public AvancePorcentajeProyectoMap()
        {
            ToTable("avances_porcentaje_proyecto", "SCH_INGENIERIA");

            HasKey(d => d.Id);
            Property(d => d.Id)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            Property(d => d.Id).HasColumnName("id");
            Property(d => d.CertificadoId).HasColumnName("certificado_id");
            Property(d => d.FechaCertificado).HasColumnName("fecha_certificado");
            Property(d => d.ProyectoId).HasColumnName("proyecto_id");
            Property(d => d.AvancePrevistoAnteriorIB).HasColumnName("avance_previsto_anterior_IB");
            Property(d => d.AvancePrevistoActualIB).HasColumnName("avance_previsto_actual_IB");
            Property(d => d.AvanceRealAnteriorIB).HasColumnName("avance_real_anterior_IB");
            Property(d => d.AvanceRealActualIB).HasColumnName("avance_real_actual_IB");
            Property(d => d.AvancePrevistoAnteriorID).HasColumnName("avance_previsto_anterior_ID");
            Property(d => d.AvancePrevistoActualID).HasColumnName("avance_previsto_actual_ID");
            Property(d => d.AvanceRealAnteriorID).HasColumnName("avance_real_anterior_ID");
            Property(d => d.AvanceRealActualID).HasColumnName("avance_real_actual_ID");
            Property(d => d.AsbuiltActual).HasColumnName("asbuilt_actual");
            Property(d => d.AsbuiltAnterior).HasColumnName("asbuilt_anterior");
            Property(d => d.Justificacion).HasColumnName("justificacion");
           

            Property(d => d.IsDeleted).HasColumnName("vigente");
            Property(d => d.CreationTime).HasColumnName("fecha_creacion");
            Property(d => d.CreatorUserId).HasColumnName("usuario_creacion");
            Property(d => d.LastModificationTime).HasColumnName("fecha_actualizacion");
            Property(d => d.LastModifierUserId).HasColumnName("usuario_actualizacion");
            Property(d => d.DeletionTime).HasColumnName("fecha_eliminacion");
            Property(d => d.DeleterUserId).HasColumnName("usuario_eliminacion");



            Property(c => c.AvancePrevistoAnteriorIB).HasPrecision(18, 4); //
            Property(c => c.AvancePrevistoActualIB).HasPrecision(18, 4); //
            Property(c => c.AvanceRealAnteriorIB).HasPrecision(18, 4); //
            Property(c => c.AvanceRealActualIB).HasPrecision(18, 4); //
            Property(c => c.AvancePrevistoAnteriorID).HasPrecision(18, 4); //
            Property(c => c.AvancePrevistoActualID).HasPrecision(18, 4); //
            Property(c => c.AvanceRealAnteriorID).HasPrecision(18, 4); //
            Property(c => c.AvanceRealActualID).HasPrecision(18, 4); //
            Property(c => c.AsbuiltActual).HasPrecision(18, 4); //            
            Property(c => c.AsbuiltAnterior).HasPrecision(18, 4); //      

        }
    }
}




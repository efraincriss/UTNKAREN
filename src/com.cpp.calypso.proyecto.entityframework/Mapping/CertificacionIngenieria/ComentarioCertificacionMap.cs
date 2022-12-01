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
 

public class ComentarioCertificacionMap : EntityTypeConfiguration<ComentarioCertificado>
{
    public ComentarioCertificacionMap()
    {
        ToTable("comentarios_certificado_ingenieria", "SCH_INGENIERIA");

        HasKey(d => d.Id);
        Property(d => d.Id)
            .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

        Property(d => d.Id).HasColumnName("id");
        Property(d => d.CertificadoId).HasColumnName("certificado_id");
        Property(d => d.FechaCarga).HasColumnName("fecha_carga");
        Property(d => d.ProyectoId).HasColumnName("proyecto_id");
  
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





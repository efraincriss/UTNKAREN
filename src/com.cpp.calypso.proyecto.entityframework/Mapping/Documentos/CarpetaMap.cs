using com.cpp.calypso.proyecto.dominio.Documentos;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace com.cpp.calypso.proyecto.entityframework.Mapping.Documentos
{
    public class CarpetaMap : EntityTypeConfiguration<Carpeta>
    {
        public CarpetaMap()
        {
            ToTable("carpetas", "SCH_DOCUMENTOS");

            HasKey(d => d.Id);
            Property(d => d.Id)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            Property(d => d.Id).HasColumnName("id");
            Property(d => d.Codigo).HasColumnName("codigo");
            Property(d => d.NombreCorto).HasColumnName("nombre_corto");
            Property(d => d.NombreCompleto).HasColumnName("nombre_completo");
            Property(d => d.Descripcion).HasColumnName("descripcion");
            Property(d => d.EstadoId).HasColumnName("catalogo_estado_id");
            Property(d => d.Publicado).HasColumnName("publicado");

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

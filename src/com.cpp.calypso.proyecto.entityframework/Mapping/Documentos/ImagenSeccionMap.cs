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
    public class ImagenSeccionMap : EntityTypeConfiguration<ImagenSeccion>
    {
        public ImagenSeccionMap()
        {
            ToTable("imagenes_seccion", "SCH_DOCUMENTOS");

            HasKey(d => d.Id);
            Property(d => d.Id)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            Property(d => d.Id).HasColumnName("id");
            Property(d => d.SeccionId).HasColumnName("seccion_id");
            Property(d => d.NombreImagen).HasColumnName("nombre_imagen");
            Property(d => d.ImagenBase64).HasColumnName("imagen_base_64");

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

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
    public class SeccionMap : EntityTypeConfiguration<Seccion>
    {
        public SeccionMap()
        {
            ToTable("secciones", "SCH_DOCUMENTOS");

            HasKey(d => d.Id);
            Property(d => d.Id)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            Property(d => d.Id).HasColumnName("id");
            Property(d => d.NombreSeccion).HasColumnName("seccion");
            Property(d => d.Contenido).HasColumnName("contenido");
            Property(d => d.Contenido_Plano).HasColumnName("contenido_plano");
            Property(d => d.DocumentoId).HasColumnName("documento_id");
            Property(d => d.SeccionPadreId).HasColumnName("seccion_id");
            Property(d => d.NumeroPagina).HasColumnName("numero_pagina");
            Property(d => d.Ordinal).HasColumnName("ordinal");

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

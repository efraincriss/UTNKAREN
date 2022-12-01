﻿using System.Data.Entity.ModelConfiguration;
using System.ComponentModel.DataAnnotations.Schema;
 
using System.Data.Entity.Infrastructure.Annotations;
 
using com.cpp.calypso.comun.dominio;

namespace com.cpp.calypso.seguridad.entityframework
{
    public class RolMap :
         EntityTypeConfiguration<Rol>
    {
        public RolMap()
        {
            ToTable("roles", "SCH_USUARIOS");

            HasKey(d => d.Id);
            Property(d => d.Id)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);


        
            Property(d => d.Codigo)
                   .HasColumnAnnotation("Index",
                    new IndexAnnotation(new IndexAttribute() { IsUnique = true }));

      
        }
    }
}

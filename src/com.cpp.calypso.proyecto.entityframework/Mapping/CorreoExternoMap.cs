﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using com.cpp.calypso.proyecto.dominio;

namespace com.cpp.calypso.proyecto.entityframework
{
    public class CorreoExternoMap : EntityTypeConfiguration<CorreoExterno>
    {
        public CorreoExternoMap()
        {
            ToTable("correos_externos", "SCH_PROYECTOS");

            HasKey(d => d.Id);
            Property(d => d.Id)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
        }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using com.cpp.calypso.proyecto.dominio;

namespace com.cpp.calypso.proyecto.entityframework
{
    public class AvanceIngenieriaMap : EntityTypeConfiguration<AvanceIngenieria>
    {
        public AvanceIngenieriaMap()
        {
            ToTable("avances_ingenieria", "SCH_PROYECTOS");
            HasKey(d => d.Id);


            Property(d => d.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);


            Property(c => c.monto_ingenieria).HasPrecision(20, 2);
            

        }
    }
}

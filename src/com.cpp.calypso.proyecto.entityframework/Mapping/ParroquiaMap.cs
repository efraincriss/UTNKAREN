using com.cpp.calypso.proyecto.dominio;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace com.cpp.calypso.proyecto.entityframework.Mapping
{
    public class ParroquiaMap : EntityTypeConfiguration<Parroquia>
    {
        public ParroquiaMap()
        {
            ToTable("parroquias", "SCH_CATALOGOS");

            Property(d => d.Id)
               .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
        }
    }
}

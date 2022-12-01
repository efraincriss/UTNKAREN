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
    public class ColaboradorResponsabilidadMap : EntityTypeConfiguration<ColaboradorResponsabilidad>
    {
        public ColaboradorResponsabilidadMap()
        {
            ToTable("colaboradores_responsabilidades", "SCH_RRHH");

            Property(d => d.Id)
               .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            
        }
    }
}

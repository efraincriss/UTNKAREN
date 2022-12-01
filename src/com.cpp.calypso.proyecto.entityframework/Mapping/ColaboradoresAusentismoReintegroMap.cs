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
    class ColaboradoresAusentismoReintegroMap : EntityTypeConfiguration<ColaboradoresAusentismoReintegros>
    {
        public ColaboradoresAusentismoReintegroMap()
        {
            ToTable("colaboradores_ausentismos_reintegros", "SCH_RRHH");

            HasKey(d => d.Id);
            Property(d => d.Id)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

        }
    }
}

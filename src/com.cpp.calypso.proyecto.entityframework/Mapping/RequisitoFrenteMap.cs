using com.cpp.calypso.proyecto.dominio;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace com.cpp.calypso.proyecto.entityframework.Mapping
{
    public class RequisitoFrenteMap : EntityTypeConfiguration<RequisitoFrente>
    {

        public RequisitoFrenteMap()
        {
            ToTable("requisitos_grupos_personales_frentes", "SCH_RRHH");

            Property(d => d.Id)
               .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

			Property(d => d.RequisitoColaboradorId).HasColumnName("grupo_personal_requisito_id");
			Property(d => d.ZonaFrenteId).HasColumnName("zona_id");

		}

    }
}

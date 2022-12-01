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
	public class ColaboradorFormacionEducativaMap : EntityTypeConfiguration<ColaboradorFormacionEducativa>
	{
		public ColaboradorFormacionEducativaMap()
		{
			ToTable("colaboradores_formaciones_educativas", "SCH_RRHH");

			Property(d => d.Id)
			   .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);


			Property(d => d.ColaboradoresId).HasColumnName("colaboradores_id");

		}
	}
}

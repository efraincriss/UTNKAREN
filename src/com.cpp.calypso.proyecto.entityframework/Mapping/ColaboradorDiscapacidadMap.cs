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
	public class ColaboradorDiscapacidadMap : EntityTypeConfiguration<ColaboradorDiscapacidad>
	{
		public ColaboradorDiscapacidadMap()
		{
			ToTable("colaboradores_discapacidades", "SCH_RRHH");

			Property(d => d.Id)
			   .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);


			Property(d => d.ColaboradoresId).HasColumnName("colaborador_id");
			Property(d => d.ColaboradorCargaSocialId).HasColumnName("colaborador_carga_social_id");

		}
	}
}

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
	public class ColaboradoresComidaMap : EntityTypeConfiguration<ColaboradoresComida>
	{
		public ColaboradoresComidaMap()
		{
			ToTable("alimentaciones", "SCH_RRHH");

			HasKey(d => d.Id);
			Property(d => d.Id)
			   .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            Property(d => d.ColaboradorServicioId).HasColumnName("colaborador_servicio_id");
        }
	}
}

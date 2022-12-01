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
	public class ServicioDestinoComidaMap : EntityTypeConfiguration<ServicioDestinoComida>
	{
		public ServicioDestinoComidaMap()
		{
			ToTable("servicios_destinos_tipos_alimentaciones", "SCH_RRHH");

			HasKey(d => d.Id);
			Property(d => d.Id)
			   .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

			Property(d => d.ServicioDestinoId).HasColumnName("servicio_destino_id");
			Property(d => d.tipo_comida).HasColumnName("catalogo_tipo_alimentacion_id");
		}
	}
}

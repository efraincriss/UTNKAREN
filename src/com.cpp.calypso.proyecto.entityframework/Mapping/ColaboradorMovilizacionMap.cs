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
    public class ColaboradorMovilizacionMap : EntityTypeConfiguration<ColaboradorMovilizacion>
    {
        public ColaboradorMovilizacionMap()
        {
            ToTable("movilizaciones", "SCH_RRHH");

            Property(d => d.Id)
               .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);


            Property(d => d.ColaboradorServicioId).HasColumnName("colaborador_servicio_id");
            Property(d => d.ParroquiaId).HasColumnName("parroquia_id");
            Property(d => d.ComunidadId).HasColumnName("comunidad_id");

        }
    }
}

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
    public class OfertaComercialMap : EntityTypeConfiguration<OfertaComercial>
    {
        public OfertaComercialMap()
        {
            ToTable("oferta_comercial", "SCH_PROYECTOS");

            HasKey(d => d.Id);
            Property(d => d.Id)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            Property(c => c.monto_ofertado_migracion_actual).HasPrecision(24, 16); // 
            Property(c => c.monto_so_aprobado_migracion_anterior).HasPrecision(24, 16); //
            Property(c => c.monto_so_aprobado_migracion_actual).HasPrecision(24, 16); //
        }
    }
}
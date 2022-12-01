using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using com.cpp.calypso.proyecto.dominio;

namespace com.cpp.calypso.proyecto.entityframework.Mapping
{
    public class OrdenServicioTempMap : EntityTypeConfiguration<OrdenServicioTemp>
    {
        public OrdenServicioTempMap()
        {
            ToTable("ordenes_servicio_temp", "SCH_PROYECTOS");

            HasKey(d => d.Id);
            Property(d => d.Id)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            Property(c => c.Ingenieria).HasPrecision(20, 2);
            Property(c => c.Construccion).HasPrecision(20, 2);
            Property(c => c.Compras).HasPrecision(20, 2);
            Property(c => c.Total).HasPrecision(20, 2);
        }
    }
}

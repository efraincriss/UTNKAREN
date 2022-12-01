using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using com.cpp.calypso.proyecto.dominio;

namespace com.cpp.calypso.proyecto.entityframework
{
    public class DetalleOrdenServicioMap : EntityTypeConfiguration<DetalleOrdenServicio>
    {
        public DetalleOrdenServicioMap()
        {
            ToTable("detalles_orden_servicio", "SCH_PROYECTOS");

            HasKey(d => d.Id);
            Property(d => d.Id)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            Property(c => c.valor_os).HasPrecision(20, 2);
        }
    }
}

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
    public class DetalleAvanceProcuraMap : EntityTypeConfiguration<DetalleAvanceProcura>
    {

        public DetalleAvanceProcuraMap()
        {
            ToTable("detalles_avance_procura", "SCH_PROYECTOS");

            HasKey(d => d.Id);
            Property(d => d.Id)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            Property(c => c.valor_real).HasPrecision(18, 8); //
            Property(c => c.ingreso_acumulado).HasPrecision(18, 8); //
            Property(c => c.calculo_diario).HasPrecision(18, 8); //
            Property(c => c.calculo_anterior).HasPrecision(18, 8); //
        }
    }
}

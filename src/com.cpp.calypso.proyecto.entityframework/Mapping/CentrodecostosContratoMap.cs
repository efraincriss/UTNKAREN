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
    public class CentrodecostosContratoMap : EntityTypeConfiguration<CentrodecostosContrato>
    {
        public CentrodecostosContratoMap()
        {
            ToTable("centro_costos_contratos", "SCH_PROYECTOS");

            HasKey(d => d.Id);
            Property(d => d.Id)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

         

            // Property(d => d.IsDeleted).HasColumnName("vigente");

        }
    }
}
using com.cpp.calypso.proyecto.dominio.WebService;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace com.cpp.calypso.proyecto.entityframework.Mapping.WebService
{
    public class ParametroServicioWebMap : EntityTypeConfiguration<ParametroServicioWeb>
    {
        public ParametroServicioWebMap()
        {
            ToTable("parametros_servicios_web", "SCH_CATALOGOS");

            HasKey(d => d.Id);
            Property(d => d.Id)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);


        }
    }
}

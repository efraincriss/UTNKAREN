using com.cpp.calypso.proyecto.dominio;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace com.cpp.calypso.proyecto.entityframework.Mapping
{
    public class RequisitoServicioMap : EntityTypeConfiguration<RequisitoServicio>
    {

        public RequisitoServicioMap()
        {
            ToTable("requisito_servicios", "SCH_RRHH");

            Property(d => d.Id)
               .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

        }

    }
}

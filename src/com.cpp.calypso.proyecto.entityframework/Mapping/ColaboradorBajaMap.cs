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
    public class ColaboradorBajaMap : EntityTypeConfiguration<ColaboradorBaja>
    {
        public ColaboradorBajaMap()
        {
            ToTable("colaboradores_bajas", "SCH_RRHH");

            Property(d => d.Id)
               .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);


            Property(d => d.ColaboradoresId).HasColumnName("colaborador_id");
            Property(d => d.ArchivoId).HasColumnName("archivo_id");

        }
    }
}

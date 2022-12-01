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
    public class ColaboradoresMap : EntityTypeConfiguration<Colaboradores>
    {

        public ColaboradoresMap()
        {
            ToTable("colaboradores", "SCH_RRHH");

            Property(d => d.Id)
               .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            Property(d => d.ContactoId).HasColumnName("contacto_id");
            Property(d => d.AdminRotacionId).HasColumnName("rotacion_id");
            Property(d => d.PaisId).HasColumnName("pais_id");
            Property(d => d.ContratoId).HasColumnName("contrato_id");
            Property(d => d.pais_pais_nacimiento_id).HasColumnName("catalogo_nacionalidad_id");
        }
    }
}

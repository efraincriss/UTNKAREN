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
    public class RequisitoColaboradorMap : EntityTypeConfiguration<RequisitoColaborador>
    {
        public RequisitoColaboradorMap()
        {
            ToTable("requisitos_grupos_personales", "SCH_RRHH");

            Property(d => d.Id)
               .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

			Property(d => d.tipo_usuarioId).HasColumnName("catalogo_grupos_personal_id");
			Property(d => d.RequisitosId).HasColumnName("requisitos_id");
			Property(d => d.rolId).HasColumnName("catalogo_accion_id");


            this.HasRequired(t => t.Requisitos)
                .WithMany(t => t.RequisitosColaboradores)
                .HasForeignKey(d => d.RequisitosId);
		}

    }
}

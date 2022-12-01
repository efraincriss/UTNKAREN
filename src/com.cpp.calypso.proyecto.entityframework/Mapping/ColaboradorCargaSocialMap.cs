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
    public class ColaboradorCargaSocialMap : EntityTypeConfiguration<ColaboradorCargaSocial>
    {
        public ColaboradorCargaSocialMap()
        {
            ToTable("colaboradores_cargas_sociales", "SCH_RRHH");

            HasKey(d => d.Id);
            Property(d => d.Id)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

			Property(d => d.ColaboradoresId).HasColumnName("colaborador_id");
			Property(d => d.idTipoIdentificacion).HasColumnName("catalogo_tipo_identificacion_id");
			Property(d => d.idGenero).HasColumnName("catalogo_genero_id");
			Property(d => d.parentesco_id).HasColumnName("catalogo_parentesco_id");
			Property(d => d.estado_civil).HasColumnName("catalogo_estado_civil_id");
			Property(d => d.PaisId).HasColumnName("pais_pais_nacimiento_id");
			Property(d => d.pais_nacimiento).HasColumnName("catalogo_nacionalidades_id");
			Property(d => d.nro_identificacion).HasColumnName("numero_identificacion");


		}
    }
}

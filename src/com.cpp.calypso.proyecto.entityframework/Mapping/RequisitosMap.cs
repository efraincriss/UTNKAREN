using com.cpp.calypso.proyecto.dominio;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace com.cpp.calypso.proyecto.entityframework
{
    public class RequisitosMap : EntityTypeConfiguration<Requisitos>
    {

        public RequisitosMap()
        {
            ToTable("requisito", "SCH_RRHH");

            HasKey(d => d.Id);
            Property(d => d.Id)
               .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

			Property(d => d.requisitoId).HasColumnName("catalogo_requisito_id");
			Property(d => d.responsableId).HasColumnName("catalogo_responsable_id");
            Property(d => d.CatalogoFrecuenciaNotificacionId).HasColumnName("catalogo_frecuencia_notificacion_id");
        }

    }
}

using com.cpp.calypso.proyecto.dominio.RecursosHumanos;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace com.cpp.calypso.proyecto.entityframework.Mapping.RecursosHumanos
{
    public class ListaDistribucionResponsableMap : EntityTypeConfiguration<ListaDistribucionResponsable>
    {
        public ListaDistribucionResponsableMap()
        {
            ToTable("listas_distribucion_responsables", "SCH_RRHH");

            HasKey(d => d.Id);
            Property(d => d.Id)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);


            Property(d => d.ListaDistribucionId).HasColumnName("lista_distribucion_id");
            Property(d => d.ResponsableId).HasColumnName("catalogo_responsable_id");
            Property(d => d.ProcesoNotificacion).HasColumnName("proceso_notificacion");

        }
    }
}
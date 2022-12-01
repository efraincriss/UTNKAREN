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
    public class ColaboradorNotificacionMap : EntityTypeConfiguration<ColaboradorNotificacion>
    {
        public ColaboradorNotificacionMap()
        {
            ToTable("colaboradores_notificaciones", "SCH_RRHH");

            HasKey(d => d.Id);
            Property(d => d.Id)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);


            Property(d => d.ColaboradorId).HasColumnName("colaborador_id");
            Property(d => d.FechaNotificacion).HasColumnName("fecha_notificacion");
            Property(d => d.ProcesoNotificacion).HasColumnName("proceso_notificacion");
            Property(d => d.ListaDistribucionResponsableId).HasColumnName("lista_distribucion_responsable_id");

        }
    }
}
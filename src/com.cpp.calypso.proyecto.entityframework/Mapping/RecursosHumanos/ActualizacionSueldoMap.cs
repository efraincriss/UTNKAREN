using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using com.cpp.calypso.proyecto.dominio.RecursosHumanos;

namespace com.cpp.calypso.proyecto.entityframework.Mapping.RecursosHumanos
{
    public class ActualizacionSueldoMap : EntityTypeConfiguration<ActualizacionSueldo>
    {
        public ActualizacionSueldoMap()
        {
            ToTable("actualizacion_sueldos", "SCH_RRHH");

            HasKey(d => d.Id);
            Property(d => d.Id)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);


            Property(d => d.FechaCarga).HasColumnName("fecha_carga");
            Property(d => d.FechaInicio).HasColumnName("fecha_inicio");
            Property(d => d.FechaFin).HasColumnName("fecha_fin");
            Property(d => d.UrlArchivo).HasColumnName("url_archivo_respaldo");
            Property(d => d.NumeroRegistros).HasColumnName("numero_registros");
            Property(d => d.Observaciones).HasColumnName("observacion");
            Property(d => d.IsDeleted).HasColumnName("eliminado");
            Property(d => d.CreationTime).HasColumnName("fecha_creacion");
            Property(d => d.CreatorUserId).HasColumnName("usuario_creador");
            Property(d => d.LastModificationTime).HasColumnName("ultima_modificacion");
            Property(d => d.LastModifierUserId).HasColumnName("usuario_ultima_modificacion");
            Property(d => d.DeletionTime).HasColumnName("fecha_eliminacion");
            Property(d => d.DeleterUserId).HasColumnName("usuario_eliminacion");
        }
    }
}

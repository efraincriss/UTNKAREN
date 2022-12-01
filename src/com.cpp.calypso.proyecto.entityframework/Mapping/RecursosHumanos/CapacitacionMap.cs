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
    public class CapacitacionMap : EntityTypeConfiguration<Capacitacion>
    {
        public CapacitacionMap()
        {
            ToTable("capacitaciones", "SCH_RRHH");

            HasKey(d => d.Id);
            Property(d => d.Id)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);


            Property(d => d.ColaboradoresId).HasColumnName("colaborador_id");
            Property(d => d.CatalogoTipoCapacitacionId).HasColumnName("catalogo_tipo_capacitacion_id");
            Property(d => d.Horas).HasColumnName("horas");
            Property(d => d.CatalogoNombreCapacitacionId).HasColumnName("catalogo_nombre_capacitacion_id");
            Property(d => d.Observaciones).HasColumnName("observacion");
            Property(d => d.Fuente).HasColumnName("fuente");
            Property(d => d.Fecha).HasColumnName("fecha");
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

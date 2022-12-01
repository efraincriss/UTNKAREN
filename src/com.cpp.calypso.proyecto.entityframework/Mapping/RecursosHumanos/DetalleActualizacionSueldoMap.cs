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
    public class DetalleActualizacionSueldoMap : EntityTypeConfiguration<DetalleActualizacionSueldo>
    {
        public DetalleActualizacionSueldoMap()
        {
            ToTable("detalle_actualizacion_sueldos", "SCH_RRHH");

            HasKey(d => d.Id);
            Property(d => d.Id)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);


            Property(d => d.CategoriaEncargadoId).HasColumnName("categoria_encargado_id");
            Property(d => d.ActualizacionSueldoId).HasColumnName("actualizacion_sueldo_id");
            Property(d => d.ValorSueldo).HasColumnName("valor_sueldo");
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

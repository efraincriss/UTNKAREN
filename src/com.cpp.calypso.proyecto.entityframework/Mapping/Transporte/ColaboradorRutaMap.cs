using com.cpp.calypso.proyecto.dominio.Transporte;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace com.cpp.calypso.proyecto.entityframework.Mapping.Transporte
{
    class ColaboradorRutaMap : EntityTypeConfiguration<ColaboradorRuta>
    {
        public ColaboradorRutaMap()
        {
            ToTable("colaboradores_rutas", "SCH_TRANSPORTES");

            HasKey(d => d.Id);
            Property(d => d.Id)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            Property(d => d.IsDeleted).HasColumnName("eliminado");
            Property(d => d.CreationTime).HasColumnName("fecha_creacion");
            Property(d => d.CreatorUserId).HasColumnName("usuario_creador");
            Property(d => d.LastModificationTime).HasColumnName("ultima_modificacion");
            Property(d => d.LastModifierUserId).HasColumnName("usuario_ultima_modificacion");
            Property(d => d.DeletionTime).HasColumnName("fecha_eliminacion");
            Property(d => d.DeleterUserId).HasColumnName("usuario_eliminacion");

            Property(d => d.ColaboradorId).HasColumnName("colaborador_id");
            Property(d => d.RutaHorarioId).HasColumnName("ruta_horario_id");

            Property(d => d.Estado).HasColumnName("estado");
            Property(d => d.Observacion).HasColumnName("observacion");
            Property(d => d.UsuarioAsignacion).HasColumnName("usuario_asignacion");
            Property(d => d.UsuarioDesAsignacion).HasColumnName("usuario_desasignacion");
            Property(d => d.FechaAsignacion).HasColumnName("fecha_asignacion");
            Property(d => d.FechaDesAsignacion).HasColumnName("fecha_desasignacion");
        }
    }
}

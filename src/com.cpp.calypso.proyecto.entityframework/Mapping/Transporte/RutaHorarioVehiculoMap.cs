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
    class RutaHorarioVehiculoMap : EntityTypeConfiguration<RutaHorarioVehiculo>
    {
        public RutaHorarioVehiculoMap()
        {
            ToTable("rutas_horarios_vehiculos", "SCH_TRANSPORTES");

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

            Property(d => d.RutaHorarioId).HasColumnName("ruta_horario_id");
            Property(d => d.VehiculoId).HasColumnName("vehiculo_id");
            Property(d => d.HoraLlegada).HasColumnName("hora_llegada");
            Property(d => d.HorarioSalida).HasColumnName("hora_salida");
            Property(d => d.FechaDesde).HasColumnName("fecha_desde");
            Property(d => d.FechaHasta).HasColumnName("fecha_hasta");
            Property(d => d.Duracion).HasColumnName("duracion");
            Property(d => d.Observacion).HasColumnName("observacion");
            Property(d => d.Estado).HasColumnName("estado");
           

        }
    }
}
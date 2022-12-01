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
    public class OperacionDiariaRutaMap : EntityTypeConfiguration<OperacionDiariaRuta>
    {
        public OperacionDiariaRutaMap()
        {
            ToTable("operaciones_diarias_rutas", "SCH_TRANSPORTES");

            HasKey(d => d.Id);
            Property(d => d.Id)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);


            Property(d => d.RutaHorarioVehiculoId).HasColumnName("ruta_horario_vehiculo_id");
            Property(d => d.RutaHorarioVehiculoRef).HasColumnName("ruta_horario_vehiculo_ref");
            Property(d => d.OperacionDiariaId).HasColumnName("operacion_diaria_id");
            Property(d => d.OperacionDiariaRef).HasColumnName("operacion_diaria_ref");
            Property(d => d.FechaInicio).HasColumnName("fecha_inicio");
            Property(d => d.FechaFin).HasColumnName("fecha_fin");
            Property(d => d.IsDeleted).HasColumnName("vigente");

        }
    }
}

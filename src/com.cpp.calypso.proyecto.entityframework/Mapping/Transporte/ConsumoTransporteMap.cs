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
    public class ConsumoTransporteMap : EntityTypeConfiguration<ConsumoTransporte>
    {
        public ConsumoTransporteMap()
        {
            ToTable("consumos_transporte", "SCH_TRANSPORTES");

            HasKey(d => d.Id);
            Property(d => d.Id)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            Property(d => d.TipoConsumo).HasColumnName("tipo_consumo");
            Property(d => d.OperacionDiariaRutaId).HasColumnName("operacion_diaria_ruta_id");
            Property(d => d.OperacionDiariaRutaRef).HasColumnName("operacion_diaria_ruta_ref");
            Property(d => d.FechaEmbarque).HasColumnName("fecha_embarque");
            Property(d => d.FechaDesembarque).HasColumnName("fecha_desembarque");
            Property(d => d.CoordenadaXEmbarque).HasColumnName("coordenada_x_embarque");
            Property(d => d.CoordenadaYEmbarque).HasColumnName("coordenada_y_embarque");
            Property(d => d.CoordenadaXDesembarque).HasColumnName("coordenada_x_desembarque");
            Property(d => d.CoordenadaYDesembarque).HasColumnName("coordenada_y_desembarque");
            Property(d => d.ColaboradorId).HasColumnName("colaborador_id");
            Property(d => d.Huella).HasColumnName("huella");
            Property(d => d.IsDeleted).HasColumnName("vigente");

        }
    }
}

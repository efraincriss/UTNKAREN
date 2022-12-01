using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using com.cpp.calypso.proyecto.dominio.Transporte;

namespace com.cpp.calypso.proyecto.entityframework.Mapping.Transporte
{
    public class VehiculoHistoricoMap : EntityTypeConfiguration<VehiculoHistorico>
    {
        public VehiculoHistoricoMap()
        {
            ToTable("vehiculos_historicos", "SCH_TRANSPORTES");

            HasKey(d => d.Id);
            Property(d => d.Id)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            Property(d => d.IsDeleted).HasColumnName("eliminado");
            Property(d => d.CreationTime).HasColumnName("fecha");
            Property(d => d.CreatorUserId).HasColumnName("usuario");
            Property(d => d.VehiculoId).HasColumnName("vehiculo_id");
            Property(d => d.Estado).HasColumnName("estado");
            Property(d => d.FechaEstado).HasColumnName("fecha_estado");
        }
    }
}

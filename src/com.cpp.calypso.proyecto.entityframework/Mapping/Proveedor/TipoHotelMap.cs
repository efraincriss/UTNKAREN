using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using com.cpp.calypso.proyecto.dominio.Proveedor;

namespace com.cpp.calypso.proyecto.entityframework.Mapping.Proveedor
{
    public class TipoHotelMap : EntityTypeConfiguration<TarifaHotel>
    {
        public TipoHotelMap()
        {
            ToTable("tarifas_hoteles", "SCH_SERVICIOS");

            HasKey(d => d.Id);
            Property(d => d.Id)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            HasRequired(p => p.TipoHabitacion)
                .WithMany()
                .HasForeignKey(s => s.TipoHabitacionId)
                .WillCascadeOnDelete(false);

            HasRequired(p => p.ContratoProveedor)
                .WithMany()
                .HasForeignKey(s => s.ContratoProveedorId)
                .WillCascadeOnDelete(false);

            Property(d => d.TipoHabitacionId).HasColumnName("tipo_habitacion_id");
            Property(d => d.ContratoProveedorId).HasColumnName("contrato_proveedor_id");
            Property(d => d.IsDeleted).HasColumnName("eliminado");
        }
    }
}

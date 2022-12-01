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

    public class DetalleReservaMap : EntityTypeConfiguration<DetalleReserva>
    {
        public DetalleReservaMap()
        {
            ToTable("detalles_reservas", "SCH_SERVICIOS");

            HasKey(d => d.Id);
            Property(d => d.Id)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            HasRequired(p => p.ReservaHotel)
                .WithMany()
                .HasForeignKey(s => s.ReservaHotelId)
                .WillCascadeOnDelete(false);

            Property(d => d.ReservaHotelId).HasColumnName("reserva_hotel_id");
            Property(d => d.IsDeleted).HasColumnName("eliminado");
        }
    }
}

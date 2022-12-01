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
    public class ReservaHotelMap : EntityTypeConfiguration<ReservaHotel>
    {
        public ReservaHotelMap()
        {
            ToTable("reservas_hoteles", "SCH_SERVICIOS");

            HasKey(d => d.Id);
            Property(d => d.Id)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);


            HasRequired(p => p.EspacioHabitacion)
                .WithMany()
                .HasForeignKey(s => s.EspacioHabitacionId)
                .WillCascadeOnDelete(false);

            HasRequired(p => p.Colaborador)
                .WithMany()
                .HasForeignKey(s => s.ColaboradorId)
                .WillCascadeOnDelete(false);

            Property(d => d.IsDeleted).HasColumnName("eliminado");
            Property(d => d.EspacioHabitacionId).HasColumnName("espacio_habitacion_id");
            Property(d => d.ColaboradorId).HasColumnName("colaborador_id");
            Property(d => d.DocumentoId).HasColumnName("documentacion_extemporanea_id");

            Property(d => d.TipoHabitacionId).HasColumnName("tipo_habitacion_id");
            Property(d => d.Costo).HasColumnName("costo");
            Property(d => d.NombreTipoHabitacion).HasColumnName("nombre_tipo_habitacion");
            Property(d => d.NumeroHabitacion).HasColumnName("numero_habitacion");
            Property(d => d.CodigoEspacio).HasColumnName("codigo_espacio");



    }
    }
}

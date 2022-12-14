using com.cpp.calypso.proyecto.dominio.Proveedor;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace com.cpp.calypso.proyecto.entityframework.Mapping.Proveedor
{
    public class TarifaLavanderiaMap : EntityTypeConfiguration<TarifaLavanderia>
    {
        public TarifaLavanderiaMap()
        {
            ToTable("tarifas_lavanderia", "SCH_SERVICIOS");

            HasKey(d => d.Id);
            Property(d => d.Id)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            HasRequired(p => p.TipoServicio)
                .WithMany()
                .HasForeignKey(s => s.TipoServicioId)
                .WillCascadeOnDelete(false);

            HasRequired(p => p.ContratoProveedor)
                .WithMany()
                .HasForeignKey(s => s.ContratoProveedorId)
                .WillCascadeOnDelete(false);

            Property(d => d.TipoServicioId).HasColumnName("tipo_servicio_id");
            Property(d => d.ContratoProveedorId).HasColumnName("contrato_proveedor_id");
            Property(d => d.IsDeleted).HasColumnName("eliminado");
        }
    }
}

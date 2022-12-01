using com.cpp.calypso.proyecto.dominio;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace com.cpp.calypso.proyecto.entityframework.Mapping.Proveedor
{
   
    public class LiquidacionServicioMap : EntityTypeConfiguration<LiquidacionServicio>
    {
        public LiquidacionServicioMap()
        {
            ToTable("liquidacion_servicios", "SCH_PROVEEDORES");

            Property(d => d.Id)
               .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            Property(d => d.IsDeleted).HasColumnName("eliminado");
            Property(d => d.CreationTime).HasColumnName("fecha_creacion");
            Property(d => d.CreatorUserId).HasColumnName("usuario_creador");
            Property(d => d.LastModificationTime).HasColumnName("ultima_modificacion");
            Property(d => d.LastModifierUserId).HasColumnName("usuario_ultima_modificacion");
            Property(d => d.DeletionTime).HasColumnName("fecha_eliminacion");
            Property(d => d.DeleterUserId).HasColumnName("usuario_eliminacion");
            Property(d => d.Codigo).HasColumnName("codigo");
            Property(d => d.ContratoProveedorId).HasColumnName("contrato_proveedor_id");
            Property(d => d.Estado).HasColumnName("estado");
            Property(d => d.TipoServicioId).HasColumnName("tipo_servicio_id");
            Property(d => d.MontoConsumido).HasColumnName("monto_consumido");
            Property(d => d.FechaDesde).HasColumnName("fecha_desde");
            Property(d => d.FechaHasta).HasColumnName("fecha_hasta");
            Property(d => d.FechaPago).HasColumnName("fecha_pago");
           
        }
    }

}

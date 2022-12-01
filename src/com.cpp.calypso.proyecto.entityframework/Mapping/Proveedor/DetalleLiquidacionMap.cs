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
    public class DetalleLiquidacionMap : EntityTypeConfiguration<DetalleLiquidacion>
    {
        public DetalleLiquidacionMap()
        {
            ToTable("detalles_liquidacioness", "SCH_PROVEEDORES");


            Property(d => d.Id)
               .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            Property(d => d.IsDeleted).HasColumnName("eliminado");
            Property(d => d.CreationTime).HasColumnName("fecha_creacion");
            Property(d => d.CreatorUserId).HasColumnName("usuario_creador");
            Property(d => d.LastModificationTime).HasColumnName("ultima_modificacion");
            Property(d => d.LastModifierUserId).HasColumnName("usuario_ultima_modificacion");
            Property(d => d.DeletionTime).HasColumnName("fecha_eliminacion");
            Property(d => d.DeleterUserId).HasColumnName("usuario_eliminacion");
            Property(d => d.Descripcion).HasColumnName("descripcion");
            Property(d => d.Fecha).HasColumnName("fecha");
            Property(d => d.LiquidacionId).HasColumnName("liquidacion_id");
            Property(d => d.Valor).HasColumnName("valor");


        }
    }

}

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
    class RutaMap : EntityTypeConfiguration<Ruta>
    {
        public RutaMap()
        {
            ToTable("rutas", "SCH_TRANSPORTES");

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

            Property(d => d.Codigo).HasColumnName("codigo");
            Property(d => d.Nombre).HasColumnName("nombre");
            Property(d => d.Descripcion).HasColumnName("descripcion");
            Property(d => d.Duracion).HasColumnName("duracion");
            Property(d => d.OrigenId).HasColumnName("lugar_origen_id");
            Property(d => d.DestinoId).HasColumnName("lugar_destino_id");
            Property(d => d.Distancia).HasColumnName("distancia");
            Property(d => d.SectorId).HasColumnName("catalogo_sector_id");
        }
    }
}

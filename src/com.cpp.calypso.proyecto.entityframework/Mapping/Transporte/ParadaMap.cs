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
    public class ParadaMap : EntityTypeConfiguration<Parada>
    {
        public ParadaMap()
        {
            ToTable("paradas", "SCH_TRANSPORTES");

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
            Property(d => d.Latitud).HasColumnName("latitud");
            Property(d => d.Longitud).HasColumnName("longitud");
            Property(d => d.Referencia).HasColumnName("referencia");
            Property(d => d.Latitud).HasPrecision(9, 6);
            Property(d => d.Longitud).HasPrecision(9, 6);
        }
    }
}

using com.cpp.calypso.proyecto.dominio;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace com.cpp.calypso.proyecto.entityframework.Mapping.Transporte
{
    
    public class ChoferMap : EntityTypeConfiguration<Chofer>
    {
        public ChoferMap()
        {
            ToTable("choferes", "SCH_TRANSPORTES");

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
            Property(d => d.ProveedorId).HasColumnName("proveedor_id");
            Property(d => d.TipoIdentificacionId).HasColumnName("catalogo_tipo_identificacion");
            Property(d => d.NumeroIdentificacion).HasColumnName("numero_identificacion");
            Property(d => d.ApellidosNombres).HasColumnName("apellidos_nombres");
            Property(d => d.Nombres).HasColumnName("nombres");
            Property(d => d.Apellidos).HasColumnName("apellidos");
            Property(d => d.GeneroId).HasColumnName("catalogo_genero_id");
            Property(d => d.FechaNacimiento).HasColumnName("fecha_nacimiento");
            Property(d => d.Celular).HasColumnName("celular");
            Property(d => d.Mail).HasColumnName("mail");
            Property(d => d.Estado).HasColumnName("estado");
            Property(d => d.FechaEstado).HasColumnName("fecha_estado");
        }
    }
}

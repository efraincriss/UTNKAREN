using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using com.cpp.calypso.proyecto.dominio;

namespace com.cpp.calypso.proyecto.entityframework.Mapping
{
    public class ContactoEmergenciaMap : EntityTypeConfiguration<ContactoEmergencia>
    {
        public ContactoEmergenciaMap()
        {
            ToTable("contactos_emergencia", "SCH_RRHH");

            HasKey(d => d.Id);
            Property(d => d.Id)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            HasRequired(p => p.Colaborador)
                .WithMany()
                .HasForeignKey(s => s.ColaboradorId)
                .WillCascadeOnDelete(false);

            Property(d => d.IsDeleted).HasColumnName("eliminado");
            Property(d => d.ColaboradorId).HasColumnName("colaborador_id");
            Property(d => d.Nombres).HasColumnName("nombres");
            Property(d => d.Identificacion).HasColumnName("cedula");
            Property(d => d.Relacion).HasColumnName("catalogo_relacion_id");
            Property(d => d.UrbanizacionComuna).HasColumnName("urbanizacion_comuna");
            Property(d => d.Direccion).HasColumnName("direccion");
            Property(d => d.Telefono).HasColumnName("telefono");
            Property(d => d.Celular).HasColumnName("celular");
            Property(d => d.CreationTime).HasColumnName("fecha_creacion");
            Property(d => d.CreatorUserId).HasColumnName("usuario_creador");
            Property(d => d.LastModificationTime).HasColumnName("ultima_modificacion");
            Property(d => d.LastModifierUserId).HasColumnName("usuario_ultima_modificacion");
            Property(d => d.DeletionTime).HasColumnName("fecha_eliminacion");
            Property(d => d.DeleterUserId).HasColumnName("usuario_eliminacion");
        }
    }
}

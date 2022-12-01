using com.cpp.calypso.proyecto.dominio;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace com.cpp.calypso.proyecto.entityframework.Mapping
{
    public class ContactoMap : EntityTypeConfiguration<Contacto>
    {
        public ContactoMap()
        {
            ToTable("contactos", "SCH_RRHH");

            HasKey(d => d.Id);
            Property(d => d.Id)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            Property(d => d.ParroquiaId).HasColumnName("parroquia_id");
            Property(d => d.ComunidadId).HasColumnName("comunidad_id");


            HasOptional(p => p.Parroquia)
                .WithMany()
                .HasForeignKey(s => s.ParroquiaId)
                .WillCascadeOnDelete(false);



        }
    }
}

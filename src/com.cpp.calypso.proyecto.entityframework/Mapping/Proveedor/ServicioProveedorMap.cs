using com.cpp.calypso.proyecto.dominio;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Infrastructure.Annotations;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace com.cpp.calypso.proyecto.entityframework.Mapping
{
	public class ServicioProveedorMap : EntityTypeConfiguration<ServicioProveedor>
	{
		public ServicioProveedorMap()
		{
			ToTable("servicios_proveedor", "SCH_SERVICIOS");

			HasKey(d => d.Id);
			Property(d => d.Id)
				.HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);


            string uniqueIndex = "UX_SPRO_PRO_ID_SER_ID";

		    /*Property(d => d.ProveedorId).HasColumnAnnotation("Index",
		        new IndexAnnotation(new IndexAttribute(uniqueIndex) { IsUnique = true, Order = 1 }));

		    Property(d => d.ServicioId).HasColumnAnnotation("Index",
		        new IndexAnnotation(new IndexAttribute(uniqueIndex) { IsUnique = true, Order = 2 }));
                */

            // Relationships
            this.HasRequired(t => t.Proveedor)
                .WithMany(t => t.servicios)
                .HasForeignKey(d => d.ProveedorId);



		    HasRequired(p => p.Servicio)
		        .WithMany()
		        .HasForeignKey(s => s.ServicioId)
		        .WillCascadeOnDelete(false);

            Property(d => d.ServicioId).HasColumnName("servicio_id");
            Property(d => d.ProveedorId).HasColumnName("proveedor_id");
            Property(d => d.IsDeleted).HasColumnName("vigente");

        }
	}
}

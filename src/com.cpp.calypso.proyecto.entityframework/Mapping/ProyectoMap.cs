using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Infrastructure.Annotations;
using System.Data.Entity.ModelConfiguration;
using com.cpp.calypso.proyecto.dominio;

namespace com.cpp.calypso.proyecto.entityframework
{
    public class ProyectoMap :
        EntityTypeConfiguration<Proyecto>
    {
         public ProyectoMap()
        {
            ToTable("proyectos", "SCH_PROYECTOS");


            /*Property(d => d.Codigo)
               .HasColumnAnnotation("IX_CODIGO",
                new IndexAnnotation(new IndexAttribute() { IsUnique = true }));*/


            HasKey(d => d.Id);
            Property(d => d.Id)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            HasRequired(a => a.Contrato)
            .WithMany(a=>a.Proyectos)
            .HasForeignKey(u => u.contratoId);

            Property(c => c.monto_aprobado_os).HasPrecision(20, 2);
            Property(c => c.monto_aprobado_os_ingenieria).HasPrecision(20, 2);
            Property(c => c.monto_aprobado_os_construccion).HasPrecision(20, 2);
            Property(c => c.monto_aprobado_os_suministros).HasPrecision(20, 2);

        }
    }  
}

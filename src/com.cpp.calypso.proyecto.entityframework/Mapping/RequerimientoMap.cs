using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Infrastructure.Annotations;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using com.cpp.calypso.proyecto.dominio;

namespace com.cpp.calypso.proyecto.entityframework
{
    public class RequerimientoMap : EntityTypeConfiguration<Requerimiento>
    {
        public RequerimientoMap()
        {
            ToTable("requerimientos", "SCH_PROYECTOS");

            HasKey(d => d.Id);
            Property(d => d.Id)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            HasRequired(a => a.Proyecto)
                .WithMany(f => f.Requerimientos)
                .HasForeignKey(u => u.ProyectoId)
                //Add evitar error:
                //'FK_SCH_PROYECTOS.requerimientos_SCH_PROYECTOS.proyectos_ProyectoId' on table 'requerimientos' may cause cycles or multiple cascade paths. Specify ON DELETE NO ACTION or ON UPDATE NO ACTION, or modify other FOREIGN KEY constraints.
                .WillCascadeOnDelete(false);

            string uniqueIndex = "UX_REQUERIMIENTO_CODIGO";

            Property(c => c.monto_ingenieria).HasPrecision(20, 2);
            Property(c => c.monto_construccion).HasPrecision(20, 2);
            Property(c => c.monto_procura).HasPrecision(20, 2);
            Property(c => c.monto_total ).HasPrecision(20, 2);



        }
    }
}

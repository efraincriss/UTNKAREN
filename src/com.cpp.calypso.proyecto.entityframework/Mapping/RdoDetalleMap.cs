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
   public class RdoDetalleMap: EntityTypeConfiguration<RdoDetalle>
    {
        public RdoDetalleMap()
        {
            ToTable("rdo_detalles", "SCH_PROYECTOS");

            HasKey(d => d.Id);
            Property(d => d.Id)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            //Add evitar error:
            //Introducing FOREIGN KEY constraint 'FK_SCH_PROYECTOS.rdo_detalles_SCH_PROYECTOS.items_ItemId' on table 'rdo_detalles' may cause cycles or multiple cascade paths. Specify ON DELETE NO ACTION or ON UPDATE NO ACTION, or modify other FOREIGN KEY constraints.
            HasRequired(a => a.Item)
             .WithMany()
             .HasForeignKey(u => u.ItemId)
             .WillCascadeOnDelete(false);

            //Add evitar error:
            //'FK_SCH_PROYECTOS.rdo_detalles_SCH_PROYECTOS.rdo_cabeceras_RdoCabeceraId' on table 'rdo_detalles' may cause cycles or multiple cascade paths. Specify ON DELETE NO ACTION or ON UPDATE NO ACTION, or modify other FOREIGN KEY constraints.
            HasRequired(a => a.RdoCabecera)
            .WithMany(c => c.RdoDetalles)
            .HasForeignKey(u => u.RdoCabeceraId)
            .WillCascadeOnDelete(false);



            Property(c => c.porcentaje_avance_anterior).HasPrecision(20, 4); //

            Property(c => c.porcentaje_avance_diario).HasPrecision(20, 4); //

            Property(c => c.porcentaje_avance_actual_acumulado).HasPrecision(20, 4); //

            Property(c => c.porcentaje_avance_previsto_acumulado).HasPrecision(20, 4); //
        }
    }
}

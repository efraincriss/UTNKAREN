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
    public class RsoDetalleEacMap : EntityTypeConfiguration<RsoDetalleEac>
    {
        public RsoDetalleEacMap()
        {
            ToTable("rso_detalles_eac", "SCH_PROYECTOS");

            HasKey(d => d.Id);
            Property(d => d.Id)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);


            //Introducing FOREIGN KEY constraint 'FK_SCH_PROYECTOS.rdo_detalles_eac_SCH_PROYECTOS.items_ItemId' on table 'rdo_detalles_eac' may cause cycles or multiple cascade paths. Specify ON DELETE NO ACTION or ON UPDATE NO ACTION, or modify other FOREIGN KEY constraints.
            HasRequired(p => p.Item)
                .WithMany()
                .HasForeignKey(s => s.ItemId)
                .WillCascadeOnDelete(false);

            //'FK_SCH_PROYECTOS.rdo_detalles_eac_SCH_PROYECTOS.rdo_cabeceras_RdoCabeceraId' on table 'rdo_detalles_eac' may cause cycles or multiple cascade paths. Specify ON DELETE NO ACTION or ON UPDATE NO ACTION, or modify other FOREIGN KEY constraints.
            HasRequired(p => p.RsoCabecera)
              .WithMany()
              .HasForeignKey(s => s.RsoCabeceraId)
              .WillCascadeOnDelete(false);

            Property(c => c.ganancia).HasPrecision(20, 4); //
            Property(c => c.porcentaje_costo_eac_total).HasPrecision(24, 20); //
            Property(c => c.porcentaje_presupuesto_total).HasPrecision(24, 20); //
            Property(c => c.porcentaje_avance_anterior).HasPrecision(24, 20); //
            Property(c => c.porcentaje_avance_diario).HasPrecision(24, 20); //
            Property(c => c.porcentaje_avance_actual_acumulado).HasPrecision(24, 20); //
            Property(c => c.porcentaje_avance_previsto_acumulado).HasPrecision(24, 20); //
            Property(c => c.costo_eac).HasPrecision(38, 20); //
            Property(c => c.costo_presupuesto).HasPrecision(38, 20); //
            Property(c => c.costo_budget_version_anterior).HasPrecision(38, 20); //
            Property(c => c.ev_actual_version_anterior).HasPrecision(38, 20); //
            Property(c => c.ac_actual).HasPrecision(38, 20); //
            Property(c => c.ac_anterior).HasPrecision(38, 20); //
            Property(c => c.ac_diario).HasPrecision(38, 20); //
            Property(c => c.cantidad_acumulada).HasPrecision(38, 20); //
            Property(c => c.cantidad_anterior).HasPrecision(38, 20); //
            Property(c => c.cantidad_diaria).HasPrecision(38, 20); //
            Property(c => c.cantidad_eac).HasPrecision(38, 20); //
            Property(c => c.cantidad_planificada).HasPrecision(38, 20); //
            Property(c => c.pv_costo_planificado).HasPrecision(38, 20); //
            Property(c => c.ern_value).HasPrecision(38, 20); //
            Property(c => c.presupuesto_total).HasPrecision(38, 20); //
            Property(c => c.ev_anterior).HasPrecision(38, 20); //
            Property(c => c.ev_diario).HasPrecision(38, 20); //
            Property(c => c.ev_actual).HasPrecision(38, 20); //

        }
    }
}

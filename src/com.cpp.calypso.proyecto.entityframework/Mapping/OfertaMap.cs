using com.cpp.calypso.proyecto.dominio;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace com.cpp.calypso.proyecto.entityframework
{
    public class OfertaMap : EntityTypeConfiguration<Oferta>
    {
        public OfertaMap()
        {
            ToTable("bases_rdo", "SCH_PROYECTOS");

            HasKey(d => d.Id);
            Property(d => d.Id)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);


            /*HasOptional(o => o.OfertaPadre)
                .WithMany()
                .HasForeignKey(o => o.OfertaPadre_Id);*/

            Property(c => c.fecha_oferta).IsOptional();
            Property(c => c.fecha_pliego).IsOptional();
            Property(c => c.fecha_primer_envio).IsOptional();
            Property(c => c.fecha_recepcion_so).IsOptional();
            Property(c => c.fecha_ultima_modificacion).IsOptional();
            Property(c => c.fecha_ultimo_envio).IsOptional();

            Property(c => c.monto_certificado_aprobado_acumulado).HasPrecision(20, 2);
            Property(c => c.monto_ofertado ).HasPrecision(20, 2);
            Property(c => c.monto_ofertado_pendiente_aprobacion ).HasPrecision(20, 2);
            Property(c => c.monto_so_aprobado ).HasPrecision(20, 2);
            Property(c => c.monto_so_referencia_total ).HasPrecision(20, 2);

            Property(c => c.monto_ingenieria).HasPrecision(20, 2);
            Property(c => c.monto_construccion).HasPrecision(20, 2);
            Property(c => c.monto_suministros).HasPrecision(20, 2);


            //Add evitar error:
            //'FK_SCH_PROYECTOS.presupuestos_SCH_CATALOGOS.catalogos_OrigenDatosId' on table 'presupuestos' may cause cycles or multiple cascade paths. Specify ON DELETE NO ACTION or ON UPDATE NO ACTION, or modify other FOREIGN KEY constraints.
            HasRequired(p => p.OrigenDatos)
              .WithMany()
              .HasForeignKey(s => s.OrigenDatosId)
              .WillCascadeOnDelete(false);

            
        }
    }
}

using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using com.cpp.calypso.proyecto.dominio;

namespace com.cpp.calypso.proyecto.entityframework.Mapping
{
    public class DetalleCertificadoMap : EntityTypeConfiguration<DetalleCertificado>
    {
        public DetalleCertificadoMap()
        {
            ToTable("detalles_certificado", "SCH_PROYECTOS");
            HasKey(d => d.Id);

            //'FK_SCH_PROYECTOS.detalles_certificado_SCH_PROYECTOS.computos_ComputoId' on table 'detalles_certificado' may cause cycles or multiple cascade paths. Specify ON DELETE NO ACTION or ON UPDATE NO ACTION, or modify other FOREIGN KEY constraints
            HasRequired(p => p.Computo)
             .WithMany()
             .HasForeignKey(s => s.ComputoId)
             .WillCascadeOnDelete(false);

        }
    }
}
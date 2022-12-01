using com.cpp.calypso.proyecto.dominio.CertificacionIngenieria;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace com.cpp.calypso.proyecto.entityframework.Mapping.CertificacionIngenieria
{

    public class DistribucionProyectoCertificacionMap : EntityTypeConfiguration<DistribucionCertificadoIngenieria>
    {
        public DistribucionProyectoCertificacionMap()
        {
            ToTable("distribucion_certificado_ingenieria", "SCH_INGENIERIA");

            HasKey(d => d.Id);
            Property(d => d.Id)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            Property(d => d.Id).HasColumnName("id");
           
            Property(d => d.IsDeleted).HasColumnName("eliminado");

        }
    }
}



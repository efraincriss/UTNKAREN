using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using com.cpp.calypso.proyecto.dominio;

namespace com.cpp.calypso.proyecto.entityframework.Mapping
{
    public class CertificadoMap : EntityTypeConfiguration<Certificado>
    {
        public CertificadoMap()
        {
            ToTable("certificados", "SCH_PROYECTOS");
            HasKey(d => d.Id);


        }
    }
}
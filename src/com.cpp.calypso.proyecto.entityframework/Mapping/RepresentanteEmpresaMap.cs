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
    public class RepresentanteEmpresaMap : EntityTypeConfiguration<RepresentanteEmpresa>
    {
        public RepresentanteEmpresaMap()
        {
            ToTable("representantes_empresas", "SCH_PROYECTOS");

            HasKey(d => d.Id);
            Property(d => d.Id)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            HasRequired(a => a.Empresa)
              .WithMany(r => r.RepresentantesEmpresa)
              .HasForeignKey(u => u.EmpresaId);
        }
    }
}

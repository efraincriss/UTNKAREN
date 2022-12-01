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
    public class CuentaEmpresaMap : EntityTypeConfiguration<CuentaEmpresa>
    {
        public CuentaEmpresaMap()
        {
            ToTable("cuentas_empresa", "SCH_PROYECTOS");

            HasKey(d => d.Id);
            Property(d => d.Id)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);


            HasRequired(a => a.Empresa)
                .WithMany()
                .HasForeignKey(u => u.EmpresaId);

            HasRequired(a => a.InstitucionFinanciera)
                .WithMany()
                .HasForeignKey(u => u.InstitucionFinancieraId);
            //Property(d => d.IsDeleted).HasColumnName("vigente");

        }
    }
}

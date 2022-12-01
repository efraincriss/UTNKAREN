using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace com.cpp.calypso.proyecto.entityframework.Mapping.Proveedor
{

    public class RegistroSincronizacionMap : EntityTypeConfiguration<dominio.Proveedor.RegistroSincronizacion>
    {
        public  RegistroSincronizacionMap ()
        {
            ToTable("registro_sincronizaciones", "SCH_RESPALDOS");

            Property(d => d.Id)
               .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            Property(d => d.UsuarioId).HasColumnName("usuario_id");
            Property(d => d.Identificador).HasColumnName("identificador");
            Property(d => d.FechaSincronizacion).HasColumnName("fecha_sincronizacion");
        }
    }
}

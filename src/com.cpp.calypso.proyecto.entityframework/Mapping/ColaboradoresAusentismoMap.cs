using com.cpp.calypso.proyecto.dominio;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace com.cpp.calypso.proyecto.entityframework.Mapping
{
    public class ColaboradoresAusentismoMap : EntityTypeConfiguration<ColaboradoresAusentismo>
    {
        public ColaboradoresAusentismoMap()
        {
            ToTable("colaboradores_ausentismos", "SCH_RRHH");

            HasKey(d => d.Id);
            Property(d => d.Id)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            HasRequired(a => a.Colaborador)
              .WithMany()
              .HasForeignKey(u => u.colaborador_id)
              .WillCascadeOnDelete(false);

            HasRequired(a => a.TipoAusentismo)
              .WithMany()
              .HasForeignKey(u => u.catalogo_tipo_ausentismo_id)
              .WillCascadeOnDelete(false);

        }
    }
}

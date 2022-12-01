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
    class ColaboradoresAusentismoRequisitoMap : EntityTypeConfiguration<ColaboradoresAusentismoRequisitos>
    {
        public ColaboradoresAusentismoRequisitoMap()
        {
            ToTable("colaboradores_ausentismos_requisitos", "SCH_RRHH");

            HasKey(d => d.Id);
            Property(d => d.Id)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);


            //Add evitar error:

            HasRequired(p => p.ColaboradorAusentismo)
              .WithMany()
              .HasForeignKey(s => s.colaborador_ausentismo_id)
              .WillCascadeOnDelete(false);
        }
    }
}

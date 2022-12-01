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
    public class ConsumoMap : EntityTypeConfiguration<Consumo>
    {
        public ConsumoMap()
        {
            ToTable("consumos", "SCH_SERVICIOS");

            HasKey(d => d.Id);
            Property(d => d.Id)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            //Add evitar error:
            //'FK_SCH_SERVICIOS.consumos_SCH_SERVICIOS.tipos_opciones_comidas_TipoOpcionComidaId' on table 'consumos' may cause cycles or multiple cascade paths. Specify ON DELETE NO ACTION or ON UPDATE NO ACTION, or modify other FOREIGN KEY constraints.
            HasRequired(a => a.TipoComida)
           .WithMany()
           .HasForeignKey(u => u.Tipo_Comida_Id)
           .WillCascadeOnDelete(false);

            HasRequired(a => a.OpcionComida)
          .WithMany()
          .HasForeignKey(u => u.Opcion_Comida_Id)
          .WillCascadeOnDelete(false);

            HasRequired(a => a.colaborador)
        .WithMany()
        .HasForeignKey(u => u.colaborador_id)
        .WillCascadeOnDelete(false);

        }
    }
}

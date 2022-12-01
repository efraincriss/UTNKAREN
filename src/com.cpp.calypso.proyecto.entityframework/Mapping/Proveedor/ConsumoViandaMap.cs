using com.cpp.calypso.proyecto.dominio;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace com.cpp.calypso.proyecto.entityframework.Mapping
{
    public class ConsumoViandaMap : EntityTypeConfiguration<ConsumoVianda>
    {
        public ConsumoViandaMap()
        {
            ToTable("consumos_viandas", "SCH_SERVICIOS");

            HasKey(d => d.Id);
            Property(d => d.Id)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);


            /// <summary>
            /// TODO: Revision por el  Cliente. 
            /// 1. Opcion Comida, no debe ser tabla, se esta utilizando un catalogo. Codigo: OPCIONCOMIDA
            /// 2. Tiene un enlace a tipoOpcionComida, que ya posee opcion de comida. (Esta entidad esta atada al contrato del proveedor)
            /// </summary>
            HasRequired(p => p.OpcionComida)
             .WithMany()
             .HasForeignKey(s => s.OpcionComidaId)
             .WillCascadeOnDelete(false);

            HasRequired(p => p.SolicitudVianda)
              .WithMany()
              .HasForeignKey(s => s.SolicitudViandaId)
              .WillCascadeOnDelete(false);

            HasRequired(p => p.TipoComida)
              .WithMany()
              .HasForeignKey(s => s.TipoOpcionComidaId)
              .WillCascadeOnDelete(false);

            HasRequired(p => p.colaborador)
            .WithMany()
            .HasForeignKey(s => s.colaborador_id)
            .WillCascadeOnDelete(false);

        }
    }
}

using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using com.cpp.calypso.proyecto.dominio;


namespace com.cpp.calypso.proyecto.entityframework
{
    public class ConsultaPublicaMap : EntityTypeConfiguration<ConsultaPublica>
    {
        public ConsultaPublicaMap()
        {
            ToTable("consultas_publicas", "SCH_ACCESOS");

            HasKey(d => d.Id);
            Property(d => d.Id)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            HasRequired(p => p.CiudadTrabajo)
                .WithMany()
                .HasForeignKey(s => s.CiudadTrabajoId)
                .WillCascadeOnDelete(false);

            HasRequired(p => p.TipoIdentificacion)
                .WithMany()
                .HasForeignKey(s => s.TipoIdentificacionId)
                .WillCascadeOnDelete(false);

            Property(d => d.CiudadTrabajoId).HasColumnName("ciudad_trabajo_id");
            Property(d => d.TipoIdentificacionId).HasColumnName("tipo_identificacion_id");
            Property(d => d.IsDeleted).HasColumnName("eliminado");
        }
    }
}


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
    public class ColaboradorCertificadoIngenieriaMap : EntityTypeConfiguration<ColaboradorCertificacionIngenieria>
    {
        public ColaboradorCertificadoIngenieriaMap()
        {
            ToTable("colaborador_certificacion_ingenieria", "SCH_INGENIERIA");

            HasKey(d => d.Id);
            Property(d => d.Id)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            Property(d => d.Id).HasColumnName("id");
            Property(d => d.UbicacionId).HasColumnName("catalogo_ubicacion_id");
            Property(d => d.ColaboradorId).HasColumnName("colaborador_id");
            Property(d => d.ModalidadId).HasColumnName("catalogo_modalidad_id");
            Property(d => d.DisciplinaId).HasColumnName("catalogo_disciplina_id");
            Property(d => d.FechaDesde).HasColumnName("fecha_desde");
            Property(d => d.FechaHasta).HasColumnName("fecha_hasta");
            Property(d => d.CategoriaID).HasColumnName("categoria_i_d");
            Property(d => d.HorasPorDia).HasColumnName("horas_laboradas_dia");
            Property(d => d.EsJornal).HasColumnName("es_jornal");
            Property(d => d.EsGastoDirecto).HasColumnName("es_gasto_directo");

            

        Property(d => d.IsDeleted).HasColumnName("vigente");
            Property(d => d.CreationTime).HasColumnName("fecha_creacion");
            Property(d => d.CreatorUserId).HasColumnName("usuario_creacion");
            Property(d => d.LastModificationTime).HasColumnName("fecha_actualizacion");
            Property(d => d.LastModifierUserId).HasColumnName("usuario_actualizacion");
            Property(d => d.DeletionTime).HasColumnName("fecha_eliminacion");
            Property(d => d.DeleterUserId).HasColumnName("usuario_eliminacion");
        }
    }
}

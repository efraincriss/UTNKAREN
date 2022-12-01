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
     public class DetallesDirectosIngenieriaMap : EntityTypeConfiguration<DetallesDirectosIngenieria>
    {
        public DetallesDirectosIngenieriaMap()
        {
            ToTable("detalles_directos_ingenieria", "SCH_INGENIERIA");

            HasKey(d => d.Id);
            Property(d => d.Id)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            Property(d => d.Id).HasColumnName("id");
            Property(d => d.ColaboradorId).HasColumnName("colaborador_id");
            Property(d => d.ColaboradorId).HasColumnName("colaborador_id");
            Property(d => d.CertificadoId).HasColumnName("certificad_id");
            Property(d => d.ProyectoId).HasColumnName("proyecto_id");
            Property(d => d.Identificacion).HasColumnName("identificacion");
            Property(d => d.FechaTrabajo).HasColumnName("fecha_trabajo");
            Property(d => d.CodigoProyecto).HasColumnName("codigo_proyecto");
            Property(d => d.NumeroHoras).HasColumnName("numero_horas");
            Property(d => d.NombreEjecutante).HasColumnName("nombre_ejecutante");
            Property(d => d.Observaciones).HasColumnName("observaciones");
            Property(d => d.EtapaId).HasColumnName("etapa_id");
            Property(d => d.EspecialidadId).HasColumnName("especialidad_id");
            Property(d => d.EstadoRegistroId).HasColumnName("estado_registro_id");
            Property(d => d.TipoRegistroId).HasColumnName("tipo_registro_id");
            Property(d => d.LocacionId).HasColumnName("locacion");
            Property(d => d.ModalidadId).HasColumnName("modalidad_id");
            Property(d => d.EsDirecto).HasColumnName("es_directo");
            Property(d => d.CertificadoId).HasColumnName("certificado_id");
            Property(d => d.JustificacionActualizacion).HasColumnName("justificacion_actualizacion");
            Property(d => d.FechaCarga).HasColumnName("fecha_carga");
            Property(d => d.CargaAutomatica).HasColumnName("carga_automatica");
            Property(d => d.Secuencial).HasColumnName("secuencial");

            Property(d => d.IsDeleted).HasColumnName("vigente");
            Property(d => d.CreationTime).HasColumnName("fecha_creacion");
            Property(d => d.CreatorUserId).HasColumnName("usuario_creacion");
            Property(d => d.LastModificationTime).HasColumnName("fecha_actualizacion");
            Property(d => d.LastModifierUserId).HasColumnName("usuario_actualizacion");
            Property(d => d.DeletionTime).HasColumnName("fecha_eliminacion");
            Property(d => d.DeleterUserId).HasColumnName("usuario_eliminacion");

            Property(c => c.NumeroHoras).HasPrecision(24, 16); //
            Property(c => c.tarifa_migracion).HasPrecision(24, 16); //
            Property(c => c.total_migracion).HasPrecision(24, 16); //
        }
    }
}



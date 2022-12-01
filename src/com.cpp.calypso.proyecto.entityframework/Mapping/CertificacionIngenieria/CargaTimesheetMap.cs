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
    class CargaTimesheetMap
    {
    }
}


public class CargaTimesheetMap : EntityTypeConfiguration<CargaTimesheet>
{
    public CargaTimesheetMap()
    {
        ToTable("carga_timesheet", "SCH_INGENIERIA");

        HasKey(d => d.Id);
        Property(d => d.Id)
            .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

        Property(d => d.Id).HasColumnName("id");
        Property(d => d.FechaInicial).HasColumnName("fecha_inicial");
        Property(d => d.FechaFinal).HasColumnName("fecha_final");
        Property(d => d.NombreArchivo).HasColumnName("nombre_archivo");
        Property(d => d.NumeroRegistros).HasColumnName("numero_registros");
        Property(d => d.ValidacionIngenieria).HasColumnName("validacion_ingenieria");
        Property(d => d.FechaValidacionIngenieria).HasColumnName("fecha_validacion_ingenieria");
        Property(d => d.IsDeleted).HasColumnName("vigente");
        Property(d => d.CreationTime).HasColumnName("fecha_creacion");
        Property(d => d.CreatorUserId).HasColumnName("usuario_creacion");
        Property(d => d.LastModificationTime).HasColumnName("fecha_actualizacion");
        Property(d => d.LastModifierUserId).HasColumnName("usuario_actualizacion");
        Property(d => d.DeletionTime).HasColumnName("fecha_eliminacion");
        Property(d => d.DeleterUserId).HasColumnName("usuario_eliminacion");
    }
}



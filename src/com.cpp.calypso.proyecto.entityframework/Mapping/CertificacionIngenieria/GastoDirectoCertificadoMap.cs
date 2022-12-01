﻿using com.cpp.calypso.proyecto.dominio.CertificacionIngenieria;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace com.cpp.calypso.proyecto.entityframework.Mapping.CertificacionIngenieria
{

    public class GastoDirectoCertificadoMap : EntityTypeConfiguration<GastoDirectoCertificado>
    {
        public GastoDirectoCertificadoMap()
        {
            ToTable("gastos_directos_certificado", "SCH_INGENIERIA");

            HasKey(d => d.Id);
            Property(d => d.Id)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            Property(d => d.Id).HasColumnName("id");


            Property(d => d.IsDeleted).HasColumnName("vigente");
            Property(d => d.CreationTime).HasColumnName("fecha_creacion");
            Property(d => d.CreatorUserId).HasColumnName("usuario_creacion");
            Property(d => d.LastModificationTime).HasColumnName("fecha_actualizacion");
            Property(d => d.LastModifierUserId).HasColumnName("usuario_actualizacion");
            Property(d => d.DeletionTime).HasColumnName("fecha_eliminacion");
            Property(d => d.DeleterUserId).HasColumnName("usuario_eliminacion");
            Property(d => d.UbicacionId).HasColumnName("ubicacion_id");
            Property(d => d.EspecialidadId).HasColumnName("especialidad_id");
            Property(d => d.Area).HasColumnName("tipo_colaborador");
            Property(d => d.UbicacionTrabajo).HasColumnName("ubicacion");

            Property(c => c.TotalHoras).HasPrecision(24, 16); //
            Property(c => c.Tarifa).HasPrecision(24, 16); //
            Property(c => c.TarifaHoras).HasPrecision(24, 16); //
           
        }
    }
}



using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using com.cpp.calypso.proyecto.dominio.Seguridades;

namespace com.cpp.calypso.proyecto.entityframework.Mapping.Seguridades
{
    public class ProblemaSincronizacionMap : EntityTypeConfiguration<ProblemaSincronizacion>
    {
        public ProblemaSincronizacionMap()
        {
            ToTable("problemas_sincronizacion", "SCH_MONITOREOS");

            HasKey(d => d.Id);
            Property(d => d.Id)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            Property(d => d.Id).HasColumnName("id");
            Property(d => d.Fecha).HasColumnName("fecha");
            Property(d => d.FechaSolucion).HasColumnName("fecha_solucion");
            Property(d => d.Fuente).HasColumnName("fuente");
            Property(d => d.Entidad).HasColumnName("entidad");
            Property(d => d.Problema).HasColumnName("problema");
            Property(d => d.Solucionado).HasColumnName("solucionado");
            Property(d => d.UsuarioId).HasColumnName("usuario_id");
            Property(d => d.Observaciones).HasColumnName("observaciones");
            Property(d => d.Uid).HasColumnName("uid");
            Property(d => d.Resumen).HasColumnName("resumen");
        }
    }
}

using com.cpp.calypso.proyecto.dominio;
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

    public class ResumenCertificacionMap : EntityTypeConfiguration<ResumenCertificacion>
    {
        public ResumenCertificacionMap()
        {
            ToTable("resumen_certificado_ingenieria", "SCH_INGENIERIA");

            HasKey(d => d.Id);
            Property(d => d.Id)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            Property(d => d.Id).HasColumnName("id");
            Property(d => d.ProyectoId).HasColumnName("proyecto_id");
            Property(d => d.Descripcion).HasColumnName("descripcion");

            Property(d => d.TotalEjecutadoHH).HasColumnName("total_ejecutado_hh");
            Property(d => d.TotalEjecutadoUSD).HasColumnName("total_ejecutado_usd");

            Property(d => d.CLASE).HasColumnName("clase");
            Property(d => d.USD_BDG).HasColumnName("usd_bdg");
            Property(d => d.HH_BDG).HasColumnName("hh_bdg");

            Property(d => d.N_Oferta).HasColumnName("numero_oferta");


            Property(d => d.USD_AB).HasColumnName("usd_ab");
            Property(d => d.HH_AB).HasColumnName("hh_ab");



            Property(d => d.EAC_USD).HasColumnName("eac_usd");
            Property(d => d.EAC_HH).HasColumnName("eac_hh");


            Property(d => d.TOTAL_PREVISTO).HasColumnName("total_previsto");
            Property(d => d.IB_PREVISTO).HasColumnName("ib_previsto");
            Property(d => d.ID_PREVISTO).HasColumnName("id_previsto");

            Property(d => d.TOTAL_REAL).HasColumnName("total_real");
            Property(d => d.IB_REAL).HasColumnName("ib_real");

            Property(d => d.ID_REAL).HasColumnName("id_real");
            Property(d => d.AB_REAL).HasColumnName("ab_real");



            Property(d => d.PORCENTAJE_AVANCE_FÍSICO_PREVISTO_IB_ID_AB).HasColumnName("porcentaje_avance_fisico_previsto_ib_id_ab");
            Property(d => d.PORCENTAJE_AVANCE_FÍSICO_REAL_IB_ID_AB).HasColumnName("porcentaje_avance_fisico_real_ib_id_ab");

            Property(d => d.DESVIO_BDG_EAC_USD).HasColumnName("desvio_bdg_eac_usd");
            Property(d => d.HH_DISPONIBLES).HasColumnName("hh_disponibles");

            Property(d => d.PLANNED_VALUE_PV_).HasColumnName("planned_value_pv");
            Property(d => d.EARN_VALUE_EV).HasColumnName("earn_value_ev");


            Property(d => d.ACTUAL_COST_AC).HasColumnName("actual_cost_ac");
            Property(d => d.SPI).HasColumnName("spi");
            Property(d => d.CPI).HasColumnName("CPI");
            Property(d => d.Comentarios).HasColumnName("comentarios");

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


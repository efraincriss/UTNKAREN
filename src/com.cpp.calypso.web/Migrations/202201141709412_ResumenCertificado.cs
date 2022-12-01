namespace com.cpp.calypso.web.Migrations
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity.Infrastructure.Annotations;
    using System.Data.Entity.Migrations;
    
    public partial class ResumenCertificado : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "SCH_INGENIERIA.resumen_certificado_ingenieria",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        proyecto_id = c.Int(nullable: false),
                        descripcion = c.String(),
                        total_ejecutado_hh = c.Decimal(nullable: false, precision: 18, scale: 2),
                        total_ejecutado_usd = c.Decimal(nullable: false, precision: 18, scale: 2),
                        clase = c.String(),
                        usd_bdg = c.Decimal(nullable: false, precision: 18, scale: 2),
                        hh_bdg = c.Decimal(nullable: false, precision: 18, scale: 2),
                        numero_oferta = c.String(),
                        usd_ab = c.Decimal(nullable: false, precision: 18, scale: 2),
                        hh_ab = c.Decimal(nullable: false, precision: 18, scale: 2),
                        eac_usd = c.Decimal(nullable: false, precision: 18, scale: 2),
                        eac_hh = c.Decimal(nullable: false, precision: 18, scale: 2),
                        total_previsto = c.Decimal(nullable: false, precision: 18, scale: 2),
                        ib_previsto = c.Decimal(nullable: false, precision: 18, scale: 2),
                        id_previsto = c.Decimal(nullable: false, precision: 18, scale: 2),
                        total_real = c.Decimal(nullable: false, precision: 18, scale: 2),
                        ib_real = c.Decimal(nullable: false, precision: 18, scale: 2),
                        id_real = c.Decimal(nullable: false, precision: 18, scale: 2),
                        ab_real = c.Decimal(nullable: false, precision: 18, scale: 2),
                        porcentaje_avance_fisico_previsto_ib_id_ab = c.Decimal(nullable: false, precision: 18, scale: 2),
                        porcentaje_avance_fisico_real_ib_id_ab = c.Decimal(nullable: false, precision: 18, scale: 2),
                        desvio_bdg_eac_usd = c.Decimal(nullable: false, precision: 18, scale: 2),
                        hh_disponibles = c.Decimal(nullable: false, precision: 18, scale: 2),
                        planned_value_pv = c.Decimal(nullable: false, precision: 18, scale: 2),
                        earn_value_ev = c.Decimal(nullable: false, precision: 18, scale: 2),
                        actual_cost_ac = c.Decimal(nullable: false, precision: 18, scale: 2),
                        spi = c.Decimal(nullable: false, precision: 18, scale: 2),
                        CPI = c.Decimal(nullable: false, precision: 18, scale: 2),
                        comentarios = c.String(),
                        vigente = c.Boolean(nullable: false),
                        usuario_creacion = c.Long(),
                        fecha_creacion = c.DateTime(nullable: false),
                        usuario_actualizacion = c.Long(),
                        fecha_actualizacion = c.DateTime(),
                        usuario_eliminacion = c.Long(),
                        fecha_eliminacion = c.DateTime(),
                    },
                annotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_ResumenCertificacion_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                })
                .PrimaryKey(t => t.id)
                .ForeignKey("SCH_PROYECTOS.proyectos", t => t.proyecto_id)
                .Index(t => t.proyecto_id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("SCH_INGENIERIA.resumen_certificado_ingenieria", "proyecto_id", "SCH_PROYECTOS.proyectos");
            DropIndex("SCH_INGENIERIA.resumen_certificado_ingenieria", new[] { "proyecto_id" });
            DropTable("SCH_INGENIERIA.resumen_certificado_ingenieria",
                removedAnnotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_ResumenCertificacion_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                });
        }
    }
}

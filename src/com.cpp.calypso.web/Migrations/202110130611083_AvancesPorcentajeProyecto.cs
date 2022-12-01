namespace com.cpp.calypso.web.Migrations
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity.Infrastructure.Annotations;
    using System.Data.Entity.Migrations;
    
    public partial class AvancesPorcentajeProyecto : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "SCH_INGENIERIA.avances_porcentaje_proyecto",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        proyecto_id = c.Int(nullable: false),
                        certificado_id = c.Int(),
                        fecha_certificado = c.DateTime(),
                        avance_previsto_anterior = c.Decimal(nullable: false, precision: 18, scale: 2),
                        avance_real_anterior = c.Decimal(nullable: false, precision: 18, scale: 2),
                        avance_previsto_actual = c.Decimal(nullable: false, precision: 18, scale: 2),
                        avance_real_actual = c.Decimal(nullable: false, precision: 18, scale: 2),
                        justificacion = c.String(),
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
                    { "DynamicFilter_AvancePorcentajeProyecto_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                })
                .PrimaryKey(t => t.id)
                .ForeignKey("SCH_PROYECTOS.proyectos", t => t.proyecto_id)
                .Index(t => t.proyecto_id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("SCH_INGENIERIA.avances_porcentaje_proyecto", "proyecto_id", "SCH_PROYECTOS.proyectos");
            DropIndex("SCH_INGENIERIA.avances_porcentaje_proyecto", new[] { "proyecto_id" });
            DropTable("SCH_INGENIERIA.avances_porcentaje_proyecto",
                removedAnnotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_AvancePorcentajeProyecto_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                });
        }
    }
}

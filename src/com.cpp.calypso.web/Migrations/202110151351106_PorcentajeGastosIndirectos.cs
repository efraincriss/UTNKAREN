namespace com.cpp.calypso.web.Migrations
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity.Infrastructure.Annotations;
    using System.Data.Entity.Migrations;
    
    public partial class PorcentajeGastosIndirectos : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "SCH_INGENIERIA.porcentajes_indirectos_ingenieria",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        detalle_indirectos_ingenieria = c.Int(nullable: false),
                        contrato_id = c.Int(nullable: false),
                        porcentaje_indirecto = c.Decimal(nullable: false, precision: 18, scale: 2),
                        horas = c.Decimal(nullable: false, precision: 18, scale: 2),
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
                    { "DynamicFilter_PorcentajeIndirectoIngenieria_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                })
                .PrimaryKey(t => t.id)
                .ForeignKey("SCH_PROYECTOS.contratos", t => t.contrato_id)
                .ForeignKey("SCH_INGENIERIA.detalles_indirectos_ingenieria", t => t.detalle_indirectos_ingenieria)
                .Index(t => t.detalle_indirectos_ingenieria)
                .Index(t => t.contrato_id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("SCH_INGENIERIA.porcentajes_indirectos_ingenieria", "detalle_indirectos_ingenieria", "SCH_INGENIERIA.detalles_indirectos_ingenieria");
            DropForeignKey("SCH_INGENIERIA.porcentajes_indirectos_ingenieria", "contrato_id", "SCH_PROYECTOS.contratos");
            DropIndex("SCH_INGENIERIA.porcentajes_indirectos_ingenieria", new[] { "contrato_id" });
            DropIndex("SCH_INGENIERIA.porcentajes_indirectos_ingenieria", new[] { "detalle_indirectos_ingenieria" });
            DropTable("SCH_INGENIERIA.porcentajes_indirectos_ingenieria",
                removedAnnotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_PorcentajeIndirectoIngenieria_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                });
        }
    }
}

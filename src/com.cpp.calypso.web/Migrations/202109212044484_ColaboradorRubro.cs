namespace com.cpp.calypso.web.Migrations
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity.Infrastructure.Annotations;
    using System.Data.Entity.Migrations;
    
    public partial class ColaboradorRubro : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "SCH_INGENIERIA.colaboradores_rubro_ingenieria",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        contrato_id = c.Int(nullable: false),
                        colaborador_id = c.Int(nullable: false),
                        rubro_id = c.Int(nullable: false),
                        tarifa = c.Decimal(nullable: false, precision: 18, scale: 2),
                        fecha_fin = c.DateTime(nullable: false),
                        FechaFin = c.DateTime(),
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
                    { "DynamicFilter_ColaboradorRubroIngenieria_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                })
                .PrimaryKey(t => t.id)
                .ForeignKey("SCH_RRHH.colaboradores", t => t.colaborador_id)
                .ForeignKey("SCH_PROYECTOS.contratos", t => t.contrato_id)
                .ForeignKey("SCH_PROYECTOS.detalle_preciarios", t => t.rubro_id)
                .Index(t => t.contrato_id)
                .Index(t => t.colaborador_id)
                .Index(t => t.rubro_id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("SCH_INGENIERIA.colaboradores_rubro_ingenieria", "rubro_id", "SCH_PROYECTOS.detalle_preciarios");
            DropForeignKey("SCH_INGENIERIA.colaboradores_rubro_ingenieria", "contrato_id", "SCH_PROYECTOS.contratos");
            DropForeignKey("SCH_INGENIERIA.colaboradores_rubro_ingenieria", "colaborador_id", "SCH_RRHH.colaboradores");
            DropIndex("SCH_INGENIERIA.colaboradores_rubro_ingenieria", new[] { "rubro_id" });
            DropIndex("SCH_INGENIERIA.colaboradores_rubro_ingenieria", new[] { "colaborador_id" });
            DropIndex("SCH_INGENIERIA.colaboradores_rubro_ingenieria", new[] { "contrato_id" });
            DropTable("SCH_INGENIERIA.colaboradores_rubro_ingenieria",
                removedAnnotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_ColaboradorRubroIngenieria_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                });
        }
    }
}

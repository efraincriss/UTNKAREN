namespace com.cpp.calypso.web.Migrations
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity.Infrastructure.Annotations;
    using System.Data.Entity.Migrations;
    
    public partial class PorcentajeAvanceIngenieria : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "SCH_INGENIERIA.porcentajes_avances_ingenieria",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        proyecto_id = c.Int(nullable: false),
                        fecha_avance = c.DateTime(nullable: false),
                        catalogo_porcentaje_id = c.Int(nullable: false),
                        valor_porcentaje = c.Decimal(nullable: false, precision: 18, scale: 2),
                        vigente = c.Boolean(nullable: false),
                        usuario_creacion = c.Long(),
                        fecha_creacion = c.DateTime(nullable: false),
                        usuario_actualizacion = c.Long(),
                        fecha_actualizacion = c.DateTime(),
                        usuario_eliminacion = c.Long(),
                        fecha_eliminacion = c.DateTime(),
                        CatalogoPorcentaje_Id = c.Int(),
                    },
                annotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_PorcentajeAvanceIngenieria_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                })
                .PrimaryKey(t => t.id)
                .ForeignKey("SCH_CATALOGOS.catalogos", t => t.CatalogoPorcentaje_Id)
                .ForeignKey("SCH_PROYECTOS.proyectos", t => t.proyecto_id)
                .Index(t => t.proyecto_id)
                .Index(t => t.CatalogoPorcentaje_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("SCH_INGENIERIA.porcentajes_avances_ingenieria", "proyecto_id", "SCH_PROYECTOS.proyectos");
            DropForeignKey("SCH_INGENIERIA.porcentajes_avances_ingenieria", "CatalogoPorcentaje_Id", "SCH_CATALOGOS.catalogos");
            DropIndex("SCH_INGENIERIA.porcentajes_avances_ingenieria", new[] { "CatalogoPorcentaje_Id" });
            DropIndex("SCH_INGENIERIA.porcentajes_avances_ingenieria", new[] { "proyecto_id" });
            DropTable("SCH_INGENIERIA.porcentajes_avances_ingenieria",
                removedAnnotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_PorcentajeAvanceIngenieria_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                });
        }
    }
}

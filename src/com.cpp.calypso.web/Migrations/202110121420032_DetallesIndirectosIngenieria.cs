namespace com.cpp.calypso.web.Migrations
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity.Infrastructure.Annotations;
    using System.Data.Entity.Migrations;
    
    public partial class DetallesIndirectosIngenieria : DbMigration
    {
        public override void Up()
        {
            
            
            CreateTable(
                "SCH_INGENIERIA.detalles_indirectos_ingenieria",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        FechaDesde = c.DateTime(nullable: false),
                        FechaHasta = c.DateTime(nullable: false),
                        colaborador_rubro_id = c.Int(nullable: false),
                        dias_laborados = c.Int(nullable: false),
                        horas_laboradas = c.Int(nullable: false),
                        es_viatico = c.Boolean(nullable: false),
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
                    { "DynamicFilter_DetalleIndirectosIngenieria_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                })
                .PrimaryKey(t => t.id)
                .ForeignKey("SCH_INGENIERIA.colaboradores_rubro_ingenieria", t => t.colaborador_rubro_id)
                .Index(t => t.colaborador_rubro_id);
            
        }
        
        public override void Down()
        {

            DropForeignKey("SCH_INGENIERIA.detalles_indirectos_ingenieria", "colaborador_rubro_id", "SCH_INGENIERIA.colaboradores_rubro_ingenieria");
            
            DropIndex("SCH_INGENIERIA.detalles_indirectos_ingenieria", new[] { "colaborador_rubro_id" });
            
            DropTable("SCH_INGENIERIA.detalles_indirectos_ingenieria",
                removedAnnotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_DetalleIndirectosIngenieria_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                });
        }
    }
}

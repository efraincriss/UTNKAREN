namespace com.cpp.calypso.web.Migrations
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity.Infrastructure.Annotations;
    using System.Data.Entity.Migrations;
    
    public partial class TarifaLavanderia : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "SCH_SERVICIOS.tarifas_lavaderia",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        tipo_servicio_id = c.Int(nullable: false),
                        contrato_proveedor_id = c.Int(nullable: false),
                        valor_servicio = c.Decimal(nullable: false, precision: 18, scale: 2),
                        estado = c.Boolean(nullable: false),
                        eliminado = c.Boolean(nullable: false),
                    },
                annotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_TarifaLavanderia_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("SCH_PROVEEDORES.contratos_proveedores", t => t.contrato_proveedor_id)
                .ForeignKey("SCH_CATALOGOS.catalogos", t => t.tipo_servicio_id)
                .Index(t => t.tipo_servicio_id)
                .Index(t => t.contrato_proveedor_id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("SCH_SERVICIOS.tarifas_lavaderia", "tipo_servicio_id", "SCH_CATALOGOS.catalogos");
            DropForeignKey("SCH_SERVICIOS.tarifas_lavaderia", "contrato_proveedor_id", "SCH_PROVEEDORES.contratos_proveedores");
            DropIndex("SCH_SERVICIOS.tarifas_lavaderia", new[] { "contrato_proveedor_id" });
            DropIndex("SCH_SERVICIOS.tarifas_lavaderia", new[] { "tipo_servicio_id" });
            DropTable("SCH_SERVICIOS.tarifas_lavaderia",
                removedAnnotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_TarifaLavanderia_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                });
        }
    }
}

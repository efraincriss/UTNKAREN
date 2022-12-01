namespace com.cpp.calypso.web.Migrations
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity.Infrastructure.Annotations;
    using System.Data.Entity.Migrations;
    
    public partial class DoblesConsumos : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "SCH_SERVICIOS.dobles_consumos",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        fecha = c.DateTime(nullable: false),
                        tipo_comida_id = c.Int(nullable: false),
                        origen_consumo_id = c.Int(),
                        proveedor_id = c.Int(nullable: false),
                        colaborador_id = c.Int(nullable: false),
                        identificador = c.String(),
                        fs = c.DateTime(),
                        fr = c.DateTime(),
                        m_version = c.Int(nullable: false),
                        uid = c.String(),
                        vigente = c.Boolean(nullable: false),
                    },
                annotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_DobleConsumo_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                })
                .PrimaryKey(t => t.id)
                .ForeignKey("SCH_RRHH.colaboradores", t => t.colaborador_id)
                .ForeignKey("SCH_PROVEEDORES.proveedores", t => t.proveedor_id)
                .ForeignKey("SCH_CATALOGOS.catalogos", t => t.tipo_comida_id)
                .Index(t => t.tipo_comida_id)
                .Index(t => t.proveedor_id)
                .Index(t => t.colaborador_id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("SCH_SERVICIOS.dobles_consumos", "tipo_comida_id", "SCH_CATALOGOS.catalogos");
            DropForeignKey("SCH_SERVICIOS.dobles_consumos", "proveedor_id", "SCH_PROVEEDORES.proveedores");
            DropForeignKey("SCH_SERVICIOS.dobles_consumos", "colaborador_id", "SCH_RRHH.colaboradores");
            DropIndex("SCH_SERVICIOS.dobles_consumos", new[] { "colaborador_id" });
            DropIndex("SCH_SERVICIOS.dobles_consumos", new[] { "proveedor_id" });
            DropIndex("SCH_SERVICIOS.dobles_consumos", new[] { "tipo_comida_id" });
            DropTable("SCH_SERVICIOS.dobles_consumos",
                removedAnnotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_DobleConsumo_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                });
        }
    }
}

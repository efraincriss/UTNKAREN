namespace com.cpp.calypso.web.Migrations
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity.Infrastructure.Annotations;
    using System.Data.Entity.Migrations;
    
    public partial class Estructura_Inicial : DbMigration
    {
        public override void Up()
        {
           /* AlterTableAnnotations(
                "SCH_SERVICIOS.servicios_proveedor",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        servicio_id = c.Int(nullable: false),
                        proveedor_id = c.Int(nullable: false),
                        estado = c.Int(nullable: false),
                        vigente = c.Boolean(nullable: false),
                    },
                annotations: new Dictionary<string, AnnotationValues>
                {
                    { 
                        "DynamicFilter_ServicioProveedor_SoftDelete",
                        new AnnotationValues(oldValue: null, newValue: "EntityFramework.DynamicFilters.DynamicFilterDefinition")
                    },
                });
            
          //  AddColumn("SCH_CATALOGOS.catalogos", "visualiza_movil", c => c.Boolean());
            AddColumn("SCH_SERVICIOS.servicios_proveedor", "vigente", c => c.Boolean(nullable: false));
      */
    }
        
        public override void Down()
        {/*
            DropColumn("SCH_SERVICIOS.servicios_proveedor", "vigente");
            DropColumn("SCH_CATALOGOS.catalogos", "visualiza_movil");
            AlterTableAnnotations(
                "SCH_SERVICIOS.servicios_proveedor",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        servicio_id = c.Int(nullable: false),
                        proveedor_id = c.Int(nullable: false),
                        estado = c.Int(nullable: false),
                        vigente = c.Boolean(nullable: false),
                    },
                annotations: new Dictionary<string, AnnotationValues>
                {
                    { 
                        "DynamicFilter_ServicioProveedor_SoftDelete",
                        new AnnotationValues(oldValue: "EntityFramework.DynamicFilters.DynamicFilterDefinition", newValue: null)
                    },
                });
            */
        }
    }
}

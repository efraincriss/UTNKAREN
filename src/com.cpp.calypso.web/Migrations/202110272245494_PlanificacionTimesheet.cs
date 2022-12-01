namespace com.cpp.calypso.web.Migrations
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity.Infrastructure.Annotations;
    using System.Data.Entity.Migrations;
    
    public partial class PlanificacionTimesheet : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "SCH_INGENIERIA.planificaciones_timesheet",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        fecha = c.DateTime(nullable: false),
                        descripcion = c.String(),
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
                    { "DynamicFilter_PlanificacionTimesheet_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                })
                .PrimaryKey(t => t.id);
            
        }
        
        public override void Down()
        {
            DropTable("SCH_INGENIERIA.planificaciones_timesheet",
                removedAnnotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_PlanificacionTimesheet_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                });
        }
    }
}

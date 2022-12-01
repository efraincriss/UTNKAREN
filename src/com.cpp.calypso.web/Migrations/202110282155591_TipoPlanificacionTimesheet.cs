namespace com.cpp.calypso.web.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class TipoPlanificacionTimesheet : DbMigration
    {
        public override void Up()
        {
            AddColumn("SCH_INGENIERIA.planificaciones_timesheet", "tipo_planificacion", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("SCH_INGENIERIA.planificaciones_timesheet", "tipo_planificacion");
        }
    }
}

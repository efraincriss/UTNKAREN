namespace com.cpp.calypso.web.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class OrdenTimesheet : DbMigration
    {
        public override void Up()
        {
            AddColumn("SCH_PROYECTOS.proyectos", "orden_timesheet", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("SCH_PROYECTOS.proyectos", "orden_timesheet");
        }
    }
}

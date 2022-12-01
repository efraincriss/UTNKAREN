namespace com.cpp.calypso.web.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ValidacionIngenieria : DbMigration
    {
        public override void Up()
        {
            AddColumn("SCH_INGENIERIA.carga_timesheet", "fecha_validacion_ingenieria", c => c.DateTime());
            AddColumn("SCH_INGENIERIA.carga_timesheet", "validacion_ingenieria", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("SCH_INGENIERIA.carga_timesheet", "validacion_ingenieria");
            DropColumn("SCH_INGENIERIA.carga_timesheet", "fecha_validacion_ingenieria");
        }
    }
}

namespace com.cpp.calypso.web.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class FechaInicioyFinFeriados : DbMigration
    {
        public override void Up()
        {
            AddColumn("SCH_INGENIERIA.feriados", "fecha_inicio", c => c.DateTime(nullable: false));
            AddColumn("SCH_INGENIERIA.feriados", "fecha_fin", c => c.DateTime(nullable: false));
            DropColumn("SCH_INGENIERIA.feriados", "fecha");
        }
        
        public override void Down()
        {
            AddColumn("SCH_INGENIERIA.feriados", "fecha", c => c.DateTime(nullable: false));
            DropColumn("SCH_INGENIERIA.feriados", "fecha_fin");
            DropColumn("SCH_INGENIERIA.feriados", "fecha_inicio");
        }
    }
}

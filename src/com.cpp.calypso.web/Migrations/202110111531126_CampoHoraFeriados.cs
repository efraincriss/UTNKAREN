namespace com.cpp.calypso.web.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CampoHoraFeriados : DbMigration
    {
        public override void Up()
        {
            AddColumn("SCH_INGENIERIA.feriados", "horas", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("SCH_INGENIERIA.feriados", "horas");
        }
    }
}

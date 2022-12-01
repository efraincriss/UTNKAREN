namespace com.cpp.calypso.web.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CampoREsumenProblemasSincronizacion : DbMigration
    {
        public override void Up()
        {
            AddColumn("SCH_MONITOREOS.problemas_sincronizacion", "resumen", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("SCH_MONITOREOS.problemas_sincronizacion", "resumen");
        }
    }
}

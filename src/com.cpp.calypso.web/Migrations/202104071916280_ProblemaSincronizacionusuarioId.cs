namespace com.cpp.calypso.web.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ProblemaSincronizacionusuarioId : DbMigration
    {
        public override void Up()
        {
            AlterColumn("SCH_MONITOREOS.problemas_sincronizacion", "usuario_id", c => c.Int());
        }
        
        public override void Down()
        {
            AlterColumn("SCH_MONITOREOS.problemas_sincronizacion", "usuario_id", c => c.String());
        }
    }
}

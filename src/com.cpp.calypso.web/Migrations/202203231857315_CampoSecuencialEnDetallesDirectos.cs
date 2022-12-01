namespace com.cpp.calypso.web.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CampoSecuencialEnDetallesDirectos : DbMigration
    {
        public override void Up()
        {
            AddColumn("SCH_INGENIERIA.detalles_directos_ingenieria", "secuencial", c => c.Int());
        }
        
        public override void Down()
        {
            DropColumn("SCH_INGENIERIA.detalles_directos_ingenieria", "secuencial");
        }
    }
}

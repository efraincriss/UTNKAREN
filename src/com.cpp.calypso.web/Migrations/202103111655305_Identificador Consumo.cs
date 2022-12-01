namespace com.cpp.calypso.web.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class IdentificadorConsumo : DbMigration
    {
        public override void Up()
        {
            AddColumn("SCH_SERVICIOS.consumos", "identificador", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("SCH_SERVICIOS.consumos", "identificador");
        }
    }
}

namespace com.cpp.calypso.web.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class FechregistoKat : DbMigration
    {
        public override void Up()
        {
            AddColumn("SCH_MNA.katz", "FechaRegistro", c => c.DateTime(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("SCH_MNA.katz", "FechaRegistro");
        }
    }
}

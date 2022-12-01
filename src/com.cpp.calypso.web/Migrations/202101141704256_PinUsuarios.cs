namespace com.cpp.calypso.web.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class PinUsuarios : DbMigration
    {
        public override void Up()
        {
           // AddColumn("SCH_USUARIOS.usuarios", "pin", c => c.String());
        }
        
        public override void Down()
        {
           // DropColumn("SCH_USUARIOS.usuarios", "pin");
        }
    }
}

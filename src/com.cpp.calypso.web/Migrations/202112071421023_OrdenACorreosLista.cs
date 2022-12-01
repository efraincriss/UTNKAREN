namespace com.cpp.calypso.web.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class OrdenACorreosLista : DbMigration
    {
        public override void Up()
        {
            AddColumn("SCH_PROYECTOS.correos_lista", "orden", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("SCH_PROYECTOS.correos_lista", "orden");
        }
    }
}

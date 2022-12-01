namespace com.cpp.calypso.web.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Seccion_Correos : DbMigration
    {
        public override void Up()
        {
            AddColumn("SCH_PROYECTOS.correos_lista", "seccion", c => c.Int());
        }
        
        public override void Down()
        {
            DropColumn("SCH_PROYECTOS.correos_lista", "seccion");
        }
    }
}

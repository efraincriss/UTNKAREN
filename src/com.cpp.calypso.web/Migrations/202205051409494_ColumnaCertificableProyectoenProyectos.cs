namespace com.cpp.calypso.web.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ColumnaCertificableProyectoenProyectos : DbMigration
    {
        public override void Up()
        {
            AddColumn("SCH_PROYECTOS.proyectos", "certificable_ingenieria", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("SCH_PROYECTOS.proyectos", "certificable_ingenieria");
        }
    }
}

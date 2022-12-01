namespace com.cpp.calypso.web.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class campoesRSOProyecto : DbMigration
    {
        public override void Up()
        {
            AddColumn("SCH_PROYECTOS.proyectos", "es_RSO", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("SCH_PROYECTOS.proyectos", "es_RSO");
        }
    }
}

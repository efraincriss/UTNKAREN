namespace com.cpp.calypso.web.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ordenProceder : DbMigration
    {
        public override void Up()
        {
            AddColumn("SCH_PROYECTOS.oferta_comercial", "link_ordenProceder", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("SCH_PROYECTOS.oferta_comercial", "link_ordenProceder");
        }
    }
}

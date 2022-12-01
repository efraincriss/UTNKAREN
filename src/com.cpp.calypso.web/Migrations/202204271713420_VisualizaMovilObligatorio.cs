namespace com.cpp.calypso.web.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class VisualizaMovilObligatorio : DbMigration
    {
        public override void Up()
        {
            AlterColumn("SCH_CATALOGOS.catalogos", "visualiza_movil", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("SCH_CATALOGOS.catalogos", "visualiza_movil", c => c.Boolean());
        }
    }
}

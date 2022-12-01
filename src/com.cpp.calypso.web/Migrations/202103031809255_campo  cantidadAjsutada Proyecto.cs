namespace com.cpp.calypso.web.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class campocantidadAjsutadaProyecto : DbMigration
    {
        public override void Up()
        {
            AddColumn("SCH_PROYECTOS.computos", "cantidadAjustada", c => c.Boolean(nullable: false));
            AddColumn("SCH_PROYECTOS.computos", "tipo", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("SCH_PROYECTOS.computos", "tipo");
            DropColumn("SCH_PROYECTOS.computos", "cantidadAjustada");
        }
    }
}

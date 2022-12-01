namespace com.cpp.calypso.web.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class codigo_Reporte_RdocolumnaProyectos : DbMigration
    {
        public override void Up()
        {
            AddColumn("SCH_PROYECTOS.proyectos", "codigo_reporte_RDO", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("SCH_PROYECTOS.proyectos", "codigo_reporte_RDO");
        }
    }
}

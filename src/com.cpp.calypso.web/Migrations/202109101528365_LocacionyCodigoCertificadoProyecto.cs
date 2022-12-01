namespace com.cpp.calypso.web.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class LocacionyCodigoCertificadoProyecto : DbMigration
    {
        public override void Up()
        {
            AddColumn("SCH_PROYECTOS.proyectos", "locacion", c => c.String());
            AddColumn("SCH_PROYECTOS.proyectos", "codigo_reporte_certificacion", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("SCH_PROYECTOS.proyectos", "codigo_reporte_certificacion");
            DropColumn("SCH_PROYECTOS.proyectos", "locacion");
        }
    }
}

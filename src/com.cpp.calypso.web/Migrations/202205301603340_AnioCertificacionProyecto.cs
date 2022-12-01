namespace com.cpp.calypso.web.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AnioCertificacionProyecto : DbMigration
    {
        public override void Up()
        {
            AddColumn("SCH_PROYECTOS.proyectos", "anio_certificacion_ingenieria", c => c.Int(nullable: false));
            AddColumn("SCH_INGENIERIA.resumen_certificado_ingenieria", "anioProyecto", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("SCH_INGENIERIA.resumen_certificado_ingenieria", "anioProyecto");
            DropColumn("SCH_PROYECTOS.proyectos", "anio_certificacion_ingenieria");
        }
    }
}

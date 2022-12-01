namespace com.cpp.calypso.web.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CamposPrpyectoJustificacion : DbMigration
    {
        public override void Up()
        {
            AddColumn("SCH_PROYECTOS.proyectos", "codigo_interno", c => c.String());
            AddColumn("SCH_PROYECTOS.proyectos", "codigo_cliente", c => c.String());
            AddColumn("SCH_INGENIERIA.detalles_directos_ingenieria", "fecha_carga", c => c.DateTime(nullable: false));
            AddColumn("SCH_INGENIERIA.detalles_directos_ingenieria", "justificacion_actualizacion", c => c.String());
            AddColumn("SCH_INGENIERIA.detalles_directos_ingenieria", "carga_automatica", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("SCH_INGENIERIA.detalles_directos_ingenieria", "carga_automatica");
            DropColumn("SCH_INGENIERIA.detalles_directos_ingenieria", "justificacion_actualizacion");
            DropColumn("SCH_INGENIERIA.detalles_directos_ingenieria", "fecha_carga");
            DropColumn("SCH_PROYECTOS.proyectos", "codigo_cliente");
            DropColumn("SCH_PROYECTOS.proyectos", "codigo_interno");
        }
    }
}

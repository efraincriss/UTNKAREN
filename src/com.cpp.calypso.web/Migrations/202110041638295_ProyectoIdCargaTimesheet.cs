namespace com.cpp.calypso.web.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ProyectoIdCargaTimesheet : DbMigration
    {
        public override void Up()
        {
            AddColumn("SCH_INGENIERIA.detalles_directos_ingenieria", "identificacion", c => c.String());
            AddColumn("SCH_INGENIERIA.detalles_directos_ingenieria", "proyecto_id", c => c.Int(nullable: false));
            CreateIndex("SCH_INGENIERIA.detalles_directos_ingenieria", "proyecto_id");
            AddForeignKey("SCH_INGENIERIA.detalles_directos_ingenieria", "proyecto_id", "SCH_PROYECTOS.proyectos", "Id");
            DropColumn("SCH_INGENIERIA.detalles_directos_ingenieria", "cedula");
        }
        
        public override void Down()
        {
            AddColumn("SCH_INGENIERIA.detalles_directos_ingenieria", "cedula", c => c.String());
            DropForeignKey("SCH_INGENIERIA.detalles_directos_ingenieria", "proyecto_id", "SCH_PROYECTOS.proyectos");
            DropIndex("SCH_INGENIERIA.detalles_directos_ingenieria", new[] { "proyecto_id" });
            DropColumn("SCH_INGENIERIA.detalles_directos_ingenieria", "proyecto_id");
            DropColumn("SCH_INGENIERIA.detalles_directos_ingenieria", "identificacion");
        }
    }
}

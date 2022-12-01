namespace com.cpp.calypso.web.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DatosIniciales : DbMigration
    {
        public override void Up()
        {
            //RenameTable(name: "SCH_SERVICIOS.tarifas_lavaderia", newName: "tarifas_lavanderia");
          /*  AddColumn("SCH_PROYECTOS.proyectos", "ProyectoCerrado", c => c.Boolean(nullable: false));
            AddColumn("SCH_PROYECTOS.proyectos", "PortafolioId", c => c.Int());
            AddColumn("SCH_PROYECTOS.proyectos", "UbicacionId", c => c.Int());
            AddColumn("SCH_PROYECTOS.proyectos", "certificable_ingenieria", c => c.Boolean(nullable: false));
            CreateIndex("SCH_PROYECTOS.proyectos", "PortafolioId");
            CreateIndex("SCH_PROYECTOS.proyectos", "UbicacionId");
            AddForeignKey("SCH_PROYECTOS.proyectos", "PortafolioId", "SCH_CATALOGOS.catalogos", "Id");
            AddForeignKey("SCH_PROYECTOS.proyectos", "UbicacionId", "SCH_CATALOGOS.catalogos", "Id");*/
        }
        
        public override void Down()
        {
            /*DropForeignKey("SCH_PROYECTOS.proyectos", "UbicacionId", "SCH_CATALOGOS.catalogos");
            DropForeignKey("SCH_PROYECTOS.proyectos", "PortafolioId", "SCH_CATALOGOS.catalogos");
            DropIndex("SCH_PROYECTOS.proyectos", new[] { "UbicacionId" });
            DropIndex("SCH_PROYECTOS.proyectos", new[] { "PortafolioId" });
            DropColumn("SCH_PROYECTOS.proyectos", "certificable_ingenieria");
            DropColumn("SCH_PROYECTOS.proyectos", "UbicacionId");
            DropColumn("SCH_PROYECTOS.proyectos", "PortafolioId");
            DropColumn("SCH_PROYECTOS.proyectos", "ProyectoCerrado");
           // RenameTable(name: "SCH_SERVICIOS.tarifas_lavanderia", newName: "tarifas_lavaderia");*/
        }
    }
}

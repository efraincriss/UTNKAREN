namespace com.cpp.calypso.web.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Merge1 : DbMigration
    {
        public override void Up()
        {
            
            
        }
        
        public override void Down()
        {
            DropForeignKey("SCH_PROYECTOS.rso_detalles_eac", "RsoCabeceraId", "SCH_PROYECTOS.rso_cabeceras");
            DropForeignKey("SCH_PROYECTOS.rso_detalles_eac", "ItemId", "SCH_PROYECTOS.items");
            DropForeignKey("SCH_PROYECTOS.rso_detalles_eac", "ComputoId", "SCH_PROYECTOS.computos");
            DropForeignKey("SCH_PROYECTOS.rso_cabeceras", "ProyectoId", "SCH_PROYECTOS.proyectos");
            DropIndex("SCH_PROYECTOS.rso_detalles_eac", new[] { "ItemId" });
            DropIndex("SCH_PROYECTOS.rso_detalles_eac", new[] { "ComputoId" });
            DropIndex("SCH_PROYECTOS.rso_detalles_eac", new[] { "RsoCabeceraId" });
            DropIndex("SCH_PROYECTOS.rso_cabeceras", new[] { "ProyectoId" });
            DropTable("SCH_PROYECTOS.rso_detalles_eac");
            DropTable("SCH_PROYECTOS.rso_cabeceras");
        }
    }
}

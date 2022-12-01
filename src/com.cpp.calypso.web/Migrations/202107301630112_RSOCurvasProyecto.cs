namespace com.cpp.calypso.web.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RSOCurvasProyecto : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "SCH_PROYECTOS.curvas_rso",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ProyectoId = c.Int(nullable: false),
                        fecha = c.DateTime(nullable: false),
                        valor_previsto = c.Decimal(nullable: false, precision: 20, scale: 6),
                        valor_previsto_acumulado = c.Decimal(nullable: false, precision: 20, scale: 6),
                        valor_real = c.Decimal(nullable: false, precision: 20, scale: 6),
                        valor_real_acumulado = c.Decimal(nullable: false, precision: 20, scale: 6),
                        dato_migrado = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("SCH_PROYECTOS.proyectos", t => t.ProyectoId)
                .Index(t => t.ProyectoId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("SCH_PROYECTOS.curvas_rso", "ProyectoId", "SCH_PROYECTOS.proyectos");
            DropIndex("SCH_PROYECTOS.curvas_rso", new[] { "ProyectoId" });
            DropTable("SCH_PROYECTOS.curvas_rso");
        }
    }
}

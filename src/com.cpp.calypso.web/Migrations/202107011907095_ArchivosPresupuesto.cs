namespace com.cpp.calypso.web.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ArchivosPresupuesto : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "SCH_PROYECTOS.archivos_presupuesto",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        PresupuestoId = c.Int(nullable: false),
                        nombre = c.String(),
                        fecha_registro = c.DateTime(nullable: false),
                        hash = c.Binary(),
                        tipo_contenido = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("SCH_PROYECTOS.presupuestos", t => t.PresupuestoId)
                .Index(t => t.PresupuestoId);
            
            AddColumn("SCH_PROYECTOS.presupuestos", "asuntoCorreo", c => c.String());
            AddColumn("SCH_PROYECTOS.presupuestos", "descripcionCorreo", c => c.String());
        }
        
        public override void Down()
        {
            DropForeignKey("SCH_PROYECTOS.archivos_presupuesto", "PresupuestoId", "SCH_PROYECTOS.presupuestos");
            DropIndex("SCH_PROYECTOS.archivos_presupuesto", new[] { "PresupuestoId" });
            DropColumn("SCH_PROYECTOS.presupuestos", "descripcionCorreo");
            DropColumn("SCH_PROYECTOS.presupuestos", "asuntoCorreo");
            DropTable("SCH_PROYECTOS.archivos_presupuesto");
        }
    }
}

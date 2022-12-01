namespace com.cpp.calypso.web.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ArchivosOrdenProceder : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "SCH_PROYECTOS.archivos_orden_proceder",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ofertaComercialId = c.Int(nullable: false),
                        nombre = c.String(),
                        fecha_registro = c.DateTime(nullable: false),
                        hash = c.Binary(),
                        tipo_contenido = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("SCH_PROYECTOS.oferta_comercial", t => t.ofertaComercialId)
                .Index(t => t.ofertaComercialId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("SCH_PROYECTOS.archivos_orden_proceder", "ofertaComercialId", "SCH_PROYECTOS.oferta_comercial");
            DropIndex("SCH_PROYECTOS.archivos_orden_proceder", new[] { "ofertaComercialId" });
            DropTable("SCH_PROYECTOS.archivos_orden_proceder");
        }
    }
}

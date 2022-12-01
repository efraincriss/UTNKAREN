namespace com.cpp.calypso.web.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CodigoEnSeccionesDocumentos : DbMigration
    {
        public override void Up()
        {
            DropIndex("SCH_DOCUMENTOS.secciones", new[] { "seccion_id" });
            AddColumn("SCH_DOCUMENTOS.secciones", "Codigo", c => c.String());
            AlterColumn("SCH_DOCUMENTOS.secciones", "seccion_id", c => c.Int());
            CreateIndex("SCH_DOCUMENTOS.secciones", "seccion_id");
        }
        
        public override void Down()
        {
            DropIndex("SCH_DOCUMENTOS.secciones", new[] { "seccion_id" });
            AlterColumn("SCH_DOCUMENTOS.secciones", "seccion_id", c => c.Int(nullable: false));
            DropColumn("SCH_DOCUMENTOS.secciones", "Codigo");
            CreateIndex("SCH_DOCUMENTOS.secciones", "seccion_id");
        }
    }
}

namespace com.cpp.calypso.web.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CatalogoTipoDocumento : DbMigration
    {
        public override void Up()
        {
            AlterColumn("SCH_DOCUMENTOS.documentos", "tipo_documento", c => c.Int(nullable: false));
            CreateIndex("SCH_DOCUMENTOS.documentos", "tipo_documento");
            AddForeignKey("SCH_DOCUMENTOS.documentos", "tipo_documento", "SCH_CATALOGOS.catalogos", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("SCH_DOCUMENTOS.documentos", "tipo_documento", "SCH_CATALOGOS.catalogos");
            DropIndex("SCH_DOCUMENTOS.documentos", new[] { "tipo_documento" });
            AlterColumn("SCH_DOCUMENTOS.documentos", "tipo_documento", c => c.String(nullable: false));
        }
    }
}

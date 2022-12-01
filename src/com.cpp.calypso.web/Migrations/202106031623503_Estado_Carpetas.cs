namespace com.cpp.calypso.web.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Estado_Carpetas : DbMigration
    {
        public override void Up()
        {
            AddColumn("SCH_DOCUMENTOS.carpetas", "catalogo_estado_id", c => c.Int(nullable: false));
            CreateIndex("SCH_DOCUMENTOS.carpetas", "catalogo_estado_id");
            AddForeignKey("SCH_DOCUMENTOS.carpetas", "catalogo_estado_id", "SCH_CATALOGOS.catalogos", "Id");
            DropColumn("SCH_DOCUMENTOS.carpetas", "estado");
        }
        
        public override void Down()
        {
            AddColumn("SCH_DOCUMENTOS.carpetas", "estado", c => c.String(nullable: false, maxLength: 100));
            DropForeignKey("SCH_DOCUMENTOS.carpetas", "catalogo_estado_id", "SCH_CATALOGOS.catalogos");
            DropIndex("SCH_DOCUMENTOS.carpetas", new[] { "catalogo_estado_id" });
            DropColumn("SCH_DOCUMENTOS.carpetas", "catalogo_estado_id");
        }
    }
}

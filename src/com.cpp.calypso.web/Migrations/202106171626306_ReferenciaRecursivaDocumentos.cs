namespace com.cpp.calypso.web.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ReferenciaRecursivaDocumentos : DbMigration
    {
        public override void Up()
        {
            AddColumn("SCH_DOCUMENTOS.documentos", "DocumentoPadreId", c => c.Int());
            CreateIndex("SCH_DOCUMENTOS.documentos", "DocumentoPadreId");
            AddForeignKey("SCH_DOCUMENTOS.documentos", "DocumentoPadreId", "SCH_DOCUMENTOS.documentos", "id");
        }
        
        public override void Down()
        {
            DropForeignKey("SCH_DOCUMENTOS.documentos", "DocumentoPadreId", "SCH_DOCUMENTOS.documentos");
            DropIndex("SCH_DOCUMENTOS.documentos", new[] { "DocumentoPadreId" });
            DropColumn("SCH_DOCUMENTOS.documentos", "DocumentoPadreId");
        }
    }
}

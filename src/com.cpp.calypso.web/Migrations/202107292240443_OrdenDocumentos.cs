namespace com.cpp.calypso.web.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class OrdenDocumentos : DbMigration
    {
        public override void Up()
        {
            AddColumn("SCH_DOCUMENTOS.documentos", "orden", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("SCH_DOCUMENTOS.documentos", "orden");
        }
    }
}

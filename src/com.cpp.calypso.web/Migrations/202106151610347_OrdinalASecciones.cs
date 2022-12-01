namespace com.cpp.calypso.web.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class OrdinalASecciones : DbMigration
    {
        public override void Up()
        {
            AddColumn("SCH_DOCUMENTOS.secciones", "ordinal", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("SCH_DOCUMENTOS.secciones", "ordinal");
        }
    }
}

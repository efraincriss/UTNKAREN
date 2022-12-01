namespace com.cpp.calypso.web.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Rename_Carpetas : DbMigration
    {
        public override void Up()
        {
            RenameTable(name: "SCH_DOCUMENTOS.caepetas", newName: "carpetas");
        }
        
        public override void Down()
        {
            RenameTable(name: "SCH_DOCUMENTOS.carpetas", newName: "caepetas");
        }
    }
}

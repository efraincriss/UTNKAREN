namespace com.cpp.calypso.web.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CarpetaCampoContenidoTamanio : DbMigration
    {
        public override void Up()
        {
            AlterColumn("SCH_DOCUMENTOS.carpetas", "descripcion", c => c.String(nullable: false, maxLength: 3000));
        }
        
        public override void Down()
        {
            AlterColumn("SCH_DOCUMENTOS.carpetas", "descripcion", c => c.String(nullable: false, maxLength: 400));
        }
    }
}

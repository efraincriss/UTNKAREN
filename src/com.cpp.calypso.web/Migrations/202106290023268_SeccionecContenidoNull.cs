namespace com.cpp.calypso.web.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class SeccionecContenidoNull : DbMigration
    {
        public override void Up()
        {
            AlterColumn("SCH_DOCUMENTOS.secciones", "contenido", c => c.String());
        }
        
        public override void Down()
        {
            AlterColumn("SCH_DOCUMENTOS.secciones", "contenido", c => c.String(nullable: false));
        }
    }
}

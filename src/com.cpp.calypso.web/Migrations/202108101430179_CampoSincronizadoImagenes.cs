namespace com.cpp.calypso.web.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CampoSincronizadoImagenes : DbMigration
    {
        public override void Up()
        {
            AddColumn("SCH_DOCUMENTOS.imagenes_seccion", "Sincronizado", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("SCH_DOCUMENTOS.imagenes_seccion", "Sincronizado");
        }
    }
}

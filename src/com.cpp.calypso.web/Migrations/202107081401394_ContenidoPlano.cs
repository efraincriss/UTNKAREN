namespace com.cpp.calypso.web.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ContenidoPlano : DbMigration
    {
        public override void Up()
        {
            //AddColumn("SCH_DOCUMENTOS.secciones", "contenido_plano", c => c.String());
            AlterColumn("SCH_DOCUMENTOS.carpetas", "nombre_corto", c => c.String(nullable: false));
            AlterColumn("SCH_DOCUMENTOS.carpetas", "nombre_completo", c => c.String(nullable: false));
            AlterColumn("SCH_DOCUMENTOS.carpetas", "descripcion", c => c.String(nullable: false));
            AlterColumn("SCH_DOCUMENTOS.secciones", "seccion", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("SCH_DOCUMENTOS.secciones", "seccion", c => c.String(nullable: false, maxLength: 1000));
            AlterColumn("SCH_DOCUMENTOS.carpetas", "descripcion", c => c.String(nullable: false, maxLength: 3000));
            AlterColumn("SCH_DOCUMENTOS.carpetas", "nombre_completo", c => c.String(nullable: false, maxLength: 1000));
            AlterColumn("SCH_DOCUMENTOS.carpetas", "nombre_corto", c => c.String(nullable: false, maxLength: 100));
           // DropColumn("SCH_DOCUMENTOS.secciones", "contenido_plano");
        }
    }
}

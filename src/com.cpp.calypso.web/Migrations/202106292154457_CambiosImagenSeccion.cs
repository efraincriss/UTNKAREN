namespace com.cpp.calypso.web.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CambiosImagenSeccion : DbMigration
    {
        public override void Up()
        {
            RenameColumn(table: "SCH_DOCUMENTOS.imagenes_seccion", name: "ImagenBase64", newName: "imagen_base_64");
        }
        
        public override void Down()
        {
            RenameColumn(table: "SCH_DOCUMENTOS.imagenes_seccion", name: "imagen_base_64", newName: "ImagenBase64");
        }
    }
}

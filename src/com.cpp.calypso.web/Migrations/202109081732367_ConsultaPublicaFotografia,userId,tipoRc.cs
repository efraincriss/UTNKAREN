namespace com.cpp.calypso.web.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ConsultaPublicaFotografiauserIdtipoRc : DbMigration
    {
        public override void Up()
        {
            //AddColumn("SCH_DOCUMENTOS.imagenes_seccion", "Sincronizado", c => c.Boolean(nullable: false));
            AddColumn("SCH_ACCESOS.consultas_publicas", "fotografia", c => c.String());
            AddColumn("SCH_ACCESOS.consultas_publicas", "usuarioConsumoId", c => c.Int());
            AddColumn("SCH_ACCESOS.consultas_publicas", "tipoRC", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("SCH_ACCESOS.consultas_publicas", "tipoRC");
            DropColumn("SCH_ACCESOS.consultas_publicas", "usuarioConsumoId");
            DropColumn("SCH_ACCESOS.consultas_publicas", "fotografia");
           // DropColumn("SCH_DOCUMENTOS.imagenes_seccion", "Sincronizado");
        }
    }
}

namespace com.cpp.calypso.web.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Map : DbMigration
    {
        public override void Up()
        {
           /* RenameColumn(table: "SCH_RESPALDOS.registro_sincronizaciones", name: "UsuarioId", newName: "usuario_id");
            RenameColumn(table: "SCH_RESPALDOS.registro_sincronizaciones", name: "FechaSincronizacion", newName: "fecha_sincronizacion");*/
        }
        
        public override void Down()
        {
           /* RenameColumn(table: "SCH_RESPALDOS.registro_sincronizaciones", name: "fecha_sincronizacion", newName: "FechaSincronizacion");
            RenameColumn(table: "SCH_RESPALDOS.registro_sincronizaciones", name: "usuario_id", newName: "UsuarioId");*/
        }
    }
}

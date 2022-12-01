namespace com.cpp.calypso.web.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Estructura_Inicial : DbMigration
    {
        public override void Up()
        {
            /*CreateTable(
                "SCH_RESPALDOS.registro_sincronizaciones",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UsuarioId = c.Int(nullable: false),
                        Identificador = c.String(),
                        FechaSincronizacion = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id);*/
            
        }
        
        public override void Down()
        {
            //DropTable("SCH_RESPALDOS.registro_sincronizaciones");
        }
    }
}

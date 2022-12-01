namespace com.cpp.calypso.web.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ProblemasSincronizacion : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "SCH_MONITOREOS.problemas_sincronizacion",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        fecha = c.DateTime(nullable: false),
                        fuente = c.String(),
                        entidad = c.String(),
                        problema = c.String(),
                        solucionado = c.Boolean(nullable: false),
                        fecha_solucion = c.DateTime(),
                        usuario_id = c.String(),
                        observaciones = c.String(),
                        uid = c.String(),
                    })
                .PrimaryKey(t => t.id);
            
        }
        
        public override void Down()
        {
            DropTable("SCH_MONITOREOS.problemas_sincronizacion");
        }
    }
}

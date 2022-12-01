namespace com.cpp.calypso.web.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class EstadoRegistroId : DbMigration
    {
        public override void Up()
        {
            AddColumn("SCH_INGENIERIA.detalles_directos_ingenieria", "estado_registro_id", c => c.Int(nullable: false));
            AddColumn("SCH_INGENIERIA.detalles_directos_ingenieria", "EstadoRegitro_Id", c => c.Int());
            CreateIndex("SCH_INGENIERIA.detalles_directos_ingenieria", "EstadoRegitro_Id");
            AddForeignKey("SCH_INGENIERIA.detalles_directos_ingenieria", "EstadoRegitro_Id", "SCH_CATALOGOS.catalogos", "Id");
            DropColumn("SCH_INGENIERIA.detalles_directos_ingenieria", "estado");
        }
        
        public override void Down()
        {
            AddColumn("SCH_INGENIERIA.detalles_directos_ingenieria", "estado", c => c.String());
            DropForeignKey("SCH_INGENIERIA.detalles_directos_ingenieria", "EstadoRegitro_Id", "SCH_CATALOGOS.catalogos");
            DropIndex("SCH_INGENIERIA.detalles_directos_ingenieria", new[] { "EstadoRegitro_Id" });
            DropColumn("SCH_INGENIERIA.detalles_directos_ingenieria", "EstadoRegitro_Id");
            DropColumn("SCH_INGENIERIA.detalles_directos_ingenieria", "estado_registro_id");
        }
    }
}

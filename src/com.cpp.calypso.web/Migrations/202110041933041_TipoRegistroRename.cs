namespace com.cpp.calypso.web.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class TipoRegistroRename : DbMigration
    {
        public override void Up()
        {
            DropIndex("SCH_INGENIERIA.detalles_directos_ingenieria", new[] { "EstadoRegitro_Id" });
            DropColumn("SCH_INGENIERIA.detalles_directos_ingenieria", "estado_registro_id");
            RenameColumn(table: "SCH_INGENIERIA.detalles_directos_ingenieria", name: "TipoRegistroId", newName: "tipo_registro_id");
            RenameColumn(table: "SCH_INGENIERIA.detalles_directos_ingenieria", name: "EstadoRegitro_Id", newName: "estado_registro_id");
            RenameIndex(table: "SCH_INGENIERIA.detalles_directos_ingenieria", name: "IX_TipoRegistroId", newName: "IX_tipo_registro_id");
            AlterColumn("SCH_INGENIERIA.detalles_directos_ingenieria", "estado_registro_id", c => c.Int(nullable: false));
            CreateIndex("SCH_INGENIERIA.detalles_directos_ingenieria", "estado_registro_id");
        }
        
        public override void Down()
        {
            DropIndex("SCH_INGENIERIA.detalles_directos_ingenieria", new[] { "estado_registro_id" });
            AlterColumn("SCH_INGENIERIA.detalles_directos_ingenieria", "estado_registro_id", c => c.Int());
            RenameIndex(table: "SCH_INGENIERIA.detalles_directos_ingenieria", name: "IX_tipo_registro_id", newName: "IX_TipoRegistroId");
            RenameColumn(table: "SCH_INGENIERIA.detalles_directos_ingenieria", name: "estado_registro_id", newName: "EstadoRegitro_Id");
            RenameColumn(table: "SCH_INGENIERIA.detalles_directos_ingenieria", name: "tipo_registro_id", newName: "TipoRegistroId");
            AddColumn("SCH_INGENIERIA.detalles_directos_ingenieria", "estado_registro_id", c => c.Int(nullable: false));
            CreateIndex("SCH_INGENIERIA.detalles_directos_ingenieria", "EstadoRegitro_Id");
        }
    }
}

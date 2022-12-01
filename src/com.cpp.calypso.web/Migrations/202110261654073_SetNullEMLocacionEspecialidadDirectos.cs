namespace com.cpp.calypso.web.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class SetNullEMLocacionEspecialidadDirectos : DbMigration
    {
        public override void Up()
        {
            DropIndex("SCH_INGENIERIA.detalles_directos_ingenieria", new[] { "etapa_id" });
            DropIndex("SCH_INGENIERIA.detalles_directos_ingenieria", new[] { "especialidad_id" });
            DropIndex("SCH_INGENIERIA.detalles_directos_ingenieria", new[] { "locacion" });
            DropIndex("SCH_INGENIERIA.detalles_directos_ingenieria", new[] { "modalidad_id" });
            AlterColumn("SCH_INGENIERIA.detalles_directos_ingenieria", "etapa_id", c => c.Int());
            AlterColumn("SCH_INGENIERIA.detalles_directos_ingenieria", "especialidad_id", c => c.Int());
            AlterColumn("SCH_INGENIERIA.detalles_directos_ingenieria", "locacion", c => c.Int());
            AlterColumn("SCH_INGENIERIA.detalles_directos_ingenieria", "modalidad_id", c => c.Int());
            CreateIndex("SCH_INGENIERIA.detalles_directos_ingenieria", "etapa_id");
            CreateIndex("SCH_INGENIERIA.detalles_directos_ingenieria", "especialidad_id");
            CreateIndex("SCH_INGENIERIA.detalles_directos_ingenieria", "locacion");
            CreateIndex("SCH_INGENIERIA.detalles_directos_ingenieria", "modalidad_id");
        }
        
        public override void Down()
        {
            DropIndex("SCH_INGENIERIA.detalles_directos_ingenieria", new[] { "modalidad_id" });
            DropIndex("SCH_INGENIERIA.detalles_directos_ingenieria", new[] { "locacion" });
            DropIndex("SCH_INGENIERIA.detalles_directos_ingenieria", new[] { "especialidad_id" });
            DropIndex("SCH_INGENIERIA.detalles_directos_ingenieria", new[] { "etapa_id" });
            AlterColumn("SCH_INGENIERIA.detalles_directos_ingenieria", "modalidad_id", c => c.Int(nullable: false));
            AlterColumn("SCH_INGENIERIA.detalles_directos_ingenieria", "locacion", c => c.Int(nullable: false));
            AlterColumn("SCH_INGENIERIA.detalles_directos_ingenieria", "especialidad_id", c => c.Int(nullable: false));
            AlterColumn("SCH_INGENIERIA.detalles_directos_ingenieria", "etapa_id", c => c.Int(nullable: false));
            CreateIndex("SCH_INGENIERIA.detalles_directos_ingenieria", "modalidad_id");
            CreateIndex("SCH_INGENIERIA.detalles_directos_ingenieria", "locacion");
            CreateIndex("SCH_INGENIERIA.detalles_directos_ingenieria", "especialidad_id");
            CreateIndex("SCH_INGENIERIA.detalles_directos_ingenieria", "etapa_id");
        }
    }
}

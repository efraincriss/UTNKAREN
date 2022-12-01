namespace com.cpp.calypso.web.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UbicacionGastoDirectoEspecialidadTipoColboradorIndirecto : DbMigration
    {
        public override void Up()
        {
            AddColumn("SCH_INGENIERIA.detalles_indirectos_ingenieria", "tipo_colaborador", c => c.Int(nullable: false));
            AddColumn("SCH_INGENIERIA.gastos_directos_certificado", "ubicacion_id", c => c.Int());
            AddColumn("SCH_INGENIERIA.gastos_directos_certificado", "especialidad_id", c => c.Int());
            AddColumn("SCH_INGENIERIA.gastos_directos_certificado", "ubicacion", c => c.String());
            AddColumn("SCH_INGENIERIA.gastos_directos_certificado", "tipo_colaborador", c => c.String());
            AddColumn("SCH_INGENIERIA.gastos_directos_certificado", "NombreEspecialidad", c => c.String());
            CreateIndex("SCH_INGENIERIA.gastos_directos_certificado", "ubicacion_id");
            CreateIndex("SCH_INGENIERIA.gastos_directos_certificado", "especialidad_id");
            AddForeignKey("SCH_INGENIERIA.gastos_directos_certificado", "especialidad_id", "SCH_CATALOGOS.catalogos", "Id");
            AddForeignKey("SCH_INGENIERIA.gastos_directos_certificado", "ubicacion_id", "SCH_CATALOGOS.catalogos", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("SCH_INGENIERIA.gastos_directos_certificado", "ubicacion_id", "SCH_CATALOGOS.catalogos");
            DropForeignKey("SCH_INGENIERIA.gastos_directos_certificado", "especialidad_id", "SCH_CATALOGOS.catalogos");
            DropIndex("SCH_INGENIERIA.gastos_directos_certificado", new[] { "especialidad_id" });
            DropIndex("SCH_INGENIERIA.gastos_directos_certificado", new[] { "ubicacion_id" });
            DropColumn("SCH_INGENIERIA.gastos_directos_certificado", "NombreEspecialidad");
            DropColumn("SCH_INGENIERIA.gastos_directos_certificado", "tipo_colaborador");
            DropColumn("SCH_INGENIERIA.gastos_directos_certificado", "ubicacion");
            DropColumn("SCH_INGENIERIA.gastos_directos_certificado", "especialidad_id");
            DropColumn("SCH_INGENIERIA.gastos_directos_certificado", "ubicacion_id");
            DropColumn("SCH_INGENIERIA.detalles_indirectos_ingenieria", "tipo_colaborador");
        }
    }
}

namespace com.cpp.calypso.web.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CamposMigracionOfertaComercial : DbMigration
    {
        public override void Up()
        {
            AddColumn("SCH_PROYECTOS.oferta_comercial", "monto_ofertado_migración_actual", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AddColumn("SCH_PROYECTOS.oferta_comercial", "monto_so_aprobado_migracion", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AddColumn("SCH_PROYECTOS.oferta_comercial", "monto_so_aprobado_migracion_anterior", c => c.Decimal(nullable: false, precision: 18, scale: 2));
        }
        
        public override void Down()
        {
            DropColumn("SCH_PROYECTOS.oferta_comercial", "monto_so_aprobado_migracion_anterior");
            DropColumn("SCH_PROYECTOS.oferta_comercial", "monto_so_aprobado_migracion");
            DropColumn("SCH_PROYECTOS.oferta_comercial", "monto_ofertado_migración_actual");
        }
    }
}

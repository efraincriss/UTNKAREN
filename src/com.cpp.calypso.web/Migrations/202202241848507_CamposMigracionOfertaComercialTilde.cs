namespace com.cpp.calypso.web.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CamposMigracionOfertaComercialTilde : DbMigration
    {
        public override void Up()
        {
            AddColumn("SCH_PROYECTOS.oferta_comercial", "monto_ofertado_migracion_actual", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            DropColumn("SCH_PROYECTOS.oferta_comercial", "monto_ofertado_migración_actual");
        }
        
        public override void Down()
        {
            AddColumn("SCH_PROYECTOS.oferta_comercial", "monto_ofertado_migración_actual", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            DropColumn("SCH_PROYECTOS.oferta_comercial", "monto_ofertado_migracion_actual");
        }
    }
}

namespace com.cpp.calypso.web.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class MapDecimalsOfertaComericlaNuevasColumnas : DbMigration
    {
        public override void Up()
        {
            AlterColumn("SCH_PROYECTOS.oferta_comercial", "monto_ofertado_migracion_actual", c => c.Decimal(nullable: false, precision: 30, scale: 22));
            AlterColumn("SCH_PROYECTOS.oferta_comercial", "monto_so_aprobado_migracion_anterior", c => c.Decimal(nullable: false, precision: 30, scale: 22));
            AlterColumn("SCH_PROYECTOS.oferta_comercial", "monto_so_aprobado_migracion_actual", c => c.Decimal(nullable: false, precision: 30, scale: 22));
        }
        
        public override void Down()
        {
            AlterColumn("SCH_PROYECTOS.oferta_comercial", "monto_so_aprobado_migracion_actual", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AlterColumn("SCH_PROYECTOS.oferta_comercial", "monto_so_aprobado_migracion_anterior", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AlterColumn("SCH_PROYECTOS.oferta_comercial", "monto_ofertado_migracion_actual", c => c.Decimal(nullable: false, precision: 18, scale: 2));
        }
    }
}

namespace com.cpp.calypso.web.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CamposMigracionDetallesDirectos : DbMigration
    {
        public override void Up()
        {
            //AddColumn("SCH_INGENIERIA.detalles_directos_ingenieria", "tarifa_migracion", c => c.Decimal(nullable: false, precision: 24, scale: 16));
            //AddColumn("SCH_INGENIERIA.detalles_directos_ingenieria", "total_migracion", c => c.Decimal(nullable: false, precision: 24, scale: 16));
            //AddColumn("SCH_INGENIERIA.detalles_directos_ingenieria", "migrado", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("SCH_INGENIERIA.detalles_directos_ingenieria", "migrado");
            DropColumn("SCH_INGENIERIA.detalles_directos_ingenieria", "total_migracion");
            DropColumn("SCH_INGENIERIA.detalles_directos_ingenieria", "tarifa_migracion");
        }
    }
}

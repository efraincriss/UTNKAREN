namespace com.cpp.calypso.web.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Preciosion2416NumeroHorasDirectos : DbMigration
    {
        public override void Up()
        {
            AlterColumn("SCH_INGENIERIA.detalles_directos_ingenieria", "numero_horas", c => c.Decimal(nullable: false, precision: 24, scale: 16));
        }
        
        public override void Down()
        {
            AlterColumn("SCH_INGENIERIA.detalles_directos_ingenieria", "numero_horas", c => c.Decimal(nullable: false, precision: 18, scale: 2));
        }
    }
}

namespace com.cpp.calypso.web.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class PrecisionDecimalesE500 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("SCH_INGENIERIA.detalles_directos_e500", "numero_horas", c => c.Decimal(nullable: false, precision: 24, scale: 16));
        }
        
        public override void Down()
        {
            AlterColumn("SCH_INGENIERIA.detalles_directos_e500", "numero_horas", c => c.Decimal(nullable: false, precision: 18, scale: 2));
        }
    }
}

namespace com.cpp.calypso.web.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class PrecisionDecimalesDetallesCertificados : DbMigration
    {
        public override void Up()
        {
            AlterColumn("SCH_INGENIERIA.gastos_directos_certificado", "TotalHoras", c => c.Decimal(nullable: false, precision: 24, scale: 16));
            AlterColumn("SCH_INGENIERIA.gastos_directos_certificado", "TarifaHoras", c => c.Decimal(nullable: false, precision: 24, scale: 16));
            AlterColumn("SCH_INGENIERIA.gastos_directos_certificado", "Tarifa", c => c.Decimal(nullable: false, precision: 24, scale: 16));
        }
        
        public override void Down()
        {
            AlterColumn("SCH_INGENIERIA.gastos_directos_certificado", "Tarifa", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AlterColumn("SCH_INGENIERIA.gastos_directos_certificado", "TarifaHoras", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AlterColumn("SCH_INGENIERIA.gastos_directos_certificado", "TotalHoras", c => c.Decimal(nullable: false, precision: 18, scale: 2));
        }
    }
}

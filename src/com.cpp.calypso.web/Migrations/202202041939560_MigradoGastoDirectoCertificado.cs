namespace com.cpp.calypso.web.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class MigradoGastoDirectoCertificado : DbMigration
    {
        public override void Up()
        {
            AddColumn("SCH_INGENIERIA.gastos_directos_certificado", "Tarifa", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AddColumn("SCH_INGENIERIA.gastos_directos_certificado", "migrado", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("SCH_INGENIERIA.gastos_directos_certificado", "migrado");
            DropColumn("SCH_INGENIERIA.gastos_directos_certificado", "Tarifa");
        }
    }
}

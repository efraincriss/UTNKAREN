namespace com.cpp.calypso.web.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class PrecisionCabecera : DbMigration
    {
        public override void Up()
        {
            AlterColumn("SCH_INGENIERIA.certificados_ingenieria", "HorasPresupuestadas", c => c.Decimal(nullable: false, precision: 24, scale: 16));
            AlterColumn("SCH_INGENIERIA.certificados_ingenieria", "MontoAnteriorCertificado", c => c.Decimal(nullable: false, precision: 24, scale: 16));
            AlterColumn("SCH_INGENIERIA.certificados_ingenieria", "MontoActualCertificado", c => c.Decimal(nullable: false, precision: 24, scale: 16));
            AlterColumn("SCH_INGENIERIA.certificados_ingenieria", "HorasAnteriorCertificadas", c => c.Decimal(nullable: false, precision: 24, scale: 16));
            AlterColumn("SCH_INGENIERIA.certificados_ingenieria", "HorasActualCertificadas", c => c.Decimal(nullable: false, precision: 24, scale: 16));
        }
        
        public override void Down()
        {
            AlterColumn("SCH_INGENIERIA.certificados_ingenieria", "HorasActualCertificadas", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AlterColumn("SCH_INGENIERIA.certificados_ingenieria", "HorasAnteriorCertificadas", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AlterColumn("SCH_INGENIERIA.certificados_ingenieria", "MontoActualCertificado", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AlterColumn("SCH_INGENIERIA.certificados_ingenieria", "MontoAnteriorCertificado", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AlterColumn("SCH_INGENIERIA.certificados_ingenieria", "HorasPresupuestadas", c => c.Decimal(nullable: false, precision: 18, scale: 2));
        }
    }
}

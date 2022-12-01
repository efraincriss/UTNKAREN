namespace com.cpp.calypso.web.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class PrecionIndirectos : DbMigration
    {
        public override void Up()
        {
            AlterColumn("SCH_INGENIERIA.detalles_indirectos_ingenieria", "dias_laborados", c => c.Decimal(nullable: false, precision: 24, scale: 16));
            AlterColumn("SCH_INGENIERIA.detalles_indirectos_ingenieria", "horas_laboradas", c => c.Decimal(nullable: false, precision: 24, scale: 16));
        }
        
        public override void Down()
        {
            AlterColumn("SCH_INGENIERIA.detalles_indirectos_ingenieria", "horas_laboradas", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AlterColumn("SCH_INGENIERIA.detalles_indirectos_ingenieria", "dias_laborados", c => c.Decimal(nullable: false, precision: 18, scale: 2));
        }
    }
}

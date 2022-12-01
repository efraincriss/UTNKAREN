namespace com.cpp.calypso.web.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AsbuiltColumna : DbMigration
    {
        public override void Up()
        {
            AddColumn("SCH_INGENIERIA.avances_porcentaje_proyecto", "asbuilt_anterior", c => c.Decimal(nullable: false, precision: 18, scale: 4));
            AddColumn("SCH_INGENIERIA.avances_porcentaje_proyecto", "asbuilt_actual", c => c.Decimal(nullable: false, precision: 18, scale: 4));
        }
        
        public override void Down()
        {
            DropColumn("SCH_INGENIERIA.avances_porcentaje_proyecto", "asbuilt_actual");
            DropColumn("SCH_INGENIERIA.avances_porcentaje_proyecto", "asbuilt_anterior");
        }
    }
}

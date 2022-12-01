namespace com.cpp.calypso.web.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class PrecioTOCosoto : DbMigration
    {
        public override void Up()
        {
            AddColumn("SCH_SERVICIOS.reservas_hoteles", "costo", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            DropColumn("SCH_SERVICIOS.reservas_hoteles", "precio");
        }
        
        public override void Down()
        {
            AddColumn("SCH_SERVICIOS.reservas_hoteles", "precio", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            DropColumn("SCH_SERVICIOS.reservas_hoteles", "costo");
        }
    }
}

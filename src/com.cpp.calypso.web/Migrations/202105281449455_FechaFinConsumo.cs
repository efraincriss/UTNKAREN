namespace com.cpp.calypso.web.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class FechaFinConsumo : DbMigration
    {
        public override void Up()
        {
            AddColumn("SCH_SERVICIOS.reservas_hoteles", "fecha_fin_consumo", c => c.DateTime());
        }
        
        public override void Down()
        {
            DropColumn("SCH_SERVICIOS.reservas_hoteles", "fecha_fin_consumo");
        }
    }
}

namespace com.cpp.calypso.web.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AplicaLavanderiaReservasyDetallesReservas : DbMigration
    {
        public override void Up()
        {
            AddColumn("SCH_SERVICIOS.detalles_reservas", "aplica_lavanderia", c => c.Boolean(nullable: false));
            AddColumn("SCH_SERVICIOS.reservas_hoteles", "aplica_lavanderia", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("SCH_SERVICIOS.reservas_hoteles", "aplica_lavanderia");
            DropColumn("SCH_SERVICIOS.detalles_reservas", "aplica_lavanderia");
        }
    }
}

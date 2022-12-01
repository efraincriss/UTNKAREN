namespace com.cpp.calypso.web.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CampoMovilHospedaje : DbMigration
    {
        public override void Up()
        {
            AddColumn("SCH_SERVICIOS.reservas_hoteles", "nombre_tipo_habitacion", c => c.String());
            AddColumn("SCH_SERVICIOS.reservas_hoteles", "numero_habitacion", c => c.String());
            AddColumn("SCH_SERVICIOS.reservas_hoteles", "codigo_espacio", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("SCH_SERVICIOS.reservas_hoteles", "codigo_espacio");
            DropColumn("SCH_SERVICIOS.reservas_hoteles", "numero_habitacion");
            DropColumn("SCH_SERVICIOS.reservas_hoteles", "nombre_tipo_habitacion");
        }
    }
}

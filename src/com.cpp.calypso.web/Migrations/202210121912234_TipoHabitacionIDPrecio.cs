namespace com.cpp.calypso.web.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class TipoHabitacionIDPrecio : DbMigration
    {
        public override void Up()
        {
            AddColumn("SCH_SERVICIOS.reservas_hoteles", "tipo_habitacion_id", c => c.Int());
            AddColumn("SCH_SERVICIOS.reservas_hoteles", "precio", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            CreateIndex("SCH_SERVICIOS.reservas_hoteles", "tipo_habitacion_id");
            AddForeignKey("SCH_SERVICIOS.reservas_hoteles", "tipo_habitacion_id", "SCH_CATALOGOS.catalogos", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("SCH_SERVICIOS.reservas_hoteles", "tipo_habitacion_id", "SCH_CATALOGOS.catalogos");
            DropIndex("SCH_SERVICIOS.reservas_hoteles", new[] { "tipo_habitacion_id" });
            DropColumn("SCH_SERVICIOS.reservas_hoteles", "precio");
            DropColumn("SCH_SERVICIOS.reservas_hoteles", "tipo_habitacion_id");
        }
    }
}

namespace com.cpp.calypso.web.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ReservaHotelesInicioConsumoJustificacionFecha : DbMigration
    {
        public override void Up()
        {
            AddColumn("SCH_SERVICIOS.reservas_hoteles", "inicio_consumo", c => c.Boolean(nullable: false));
            AddColumn("SCH_SERVICIOS.reservas_hoteles", "fecha_inicio_consumo", c => c.DateTime());
            AddColumn("SCH_SERVICIOS.reservas_hoteles", "justificacion_inicio_manual", c => c.String());
            AddColumn("SCH_SERVICIOS.reservas_hoteles", "justificacion_finalizacion_manual", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("SCH_SERVICIOS.reservas_hoteles", "justificacion_finalizacion_manual");
            DropColumn("SCH_SERVICIOS.reservas_hoteles", "justificacion_inicio_manual");
            DropColumn("SCH_SERVICIOS.reservas_hoteles", "fecha_inicio_consumo");
            DropColumn("SCH_SERVICIOS.reservas_hoteles", "inicio_consumo");
        }
    }
}

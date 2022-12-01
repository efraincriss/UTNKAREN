namespace com.cpp.calypso.web.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class HoraInicioHoraFin : DbMigration
    {
        public override void Up()
        {
           AddColumn("SCH_SERVICIOS.tipos_opciones_comidas", "hora_inicio", c => c.DateTime(nullable: false));
            AddColumn("SCH_SERVICIOS.tipos_opciones_comidas", "hora_fin", c => c.DateTime(nullable: false));
          //  DropColumn("SCH_PROYECTOS.rdo_detalles_eac", "ev_actual_version_anterior");
        }
        
        public override void Down()
        {
//AddColumn("SCH_PROYECTOS.rdo_detalles_eac", "ev_actual_version_anterior", c => c.Decimal(nullable: false, precision: 38, scale: 20));
            DropColumn("SCH_SERVICIOS.tipos_opciones_comidas", "hora_fin");
            DropColumn("SCH_SERVICIOS.tipos_opciones_comidas", "hora_inicio");
        }
    }
}

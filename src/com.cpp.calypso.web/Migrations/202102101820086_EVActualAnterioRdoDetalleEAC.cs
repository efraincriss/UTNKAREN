namespace com.cpp.calypso.web.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class EVActualAnterioRdoDetalleEAC : DbMigration
    {
        public override void Up()
        {
            AddColumn("SCH_PROYECTOS.rdo_detalles_eac", "ev_actual_version_anterior", c => c.Decimal(nullable: false, precision: 38, scale: 20));
        }
        
        public override void Down()
        {
            DropColumn("SCH_PROYECTOS.rdo_detalles_eac", "ev_actual_version_anterior");
        }
    }
}

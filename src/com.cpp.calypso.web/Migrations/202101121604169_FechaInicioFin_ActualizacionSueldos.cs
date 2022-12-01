namespace com.cpp.calypso.web.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class FechaInicioFin_ActualizacionSueldos : DbMigration
    {
        public override void Up()
        {
            AddColumn("SCH_RRHH.actualizacion_sueldos", "FechaInicio", c => c.DateTime(nullable: false));
            AddColumn("SCH_RRHH.actualizacion_sueldos", "FechaFin", c => c.DateTime());
        }
        
        public override void Down()
        {
            DropColumn("SCH_RRHH.actualizacion_sueldos", "FechaFin");
            DropColumn("SCH_RRHH.actualizacion_sueldos", "FechaInicio");
        }
    }
}

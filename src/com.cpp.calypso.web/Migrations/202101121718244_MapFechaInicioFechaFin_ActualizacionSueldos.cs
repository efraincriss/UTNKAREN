namespace com.cpp.calypso.web.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class MapFechaInicioFechaFin_ActualizacionSueldos : DbMigration
    {
        public override void Up()
        {
            RenameColumn(table: "SCH_RRHH.actualizacion_sueldos", name: "FechaInicio", newName: "fecha_inicio");
            RenameColumn(table: "SCH_RRHH.actualizacion_sueldos", name: "FechaFin", newName: "fecha_fin");
        }
        
        public override void Down()
        {
            RenameColumn(table: "SCH_RRHH.actualizacion_sueldos", name: "fecha_fin", newName: "FechaFin");
            RenameColumn(table: "SCH_RRHH.actualizacion_sueldos", name: "fecha_inicio", newName: "FechaInicio");
        }
    }
}

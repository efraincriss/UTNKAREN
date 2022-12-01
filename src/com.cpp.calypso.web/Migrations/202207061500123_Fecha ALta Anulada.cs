namespace com.cpp.calypso.web.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class FechaALtaAnulada : DbMigration
    {
        public override void Up()
        {
            AddColumn("SCH_RRHH.colaboradores", "fecha_anulacion_alta", c => c.DateTime());
        }
        
        public override void Down()
        {
            DropColumn("SCH_RRHH.colaboradores", "fecha_anulacion_alta");
        }
    }
}

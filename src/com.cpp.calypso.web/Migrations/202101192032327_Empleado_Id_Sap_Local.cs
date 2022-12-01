namespace com.cpp.calypso.web.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Empleado_Id_Sap_Local : DbMigration
    {
        public override void Up()
        {
            AddColumn("SCH_RRHH.colaboradores", "empleado_id_sap_local", c => c.Int());
        }
        
        public override void Down()
        {
            DropColumn("SCH_RRHH.colaboradores", "empleado_id_sap_local");
        }
    }
}

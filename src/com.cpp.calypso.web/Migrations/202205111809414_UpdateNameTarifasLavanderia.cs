namespace com.cpp.calypso.web.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateNameTarifasLavanderia : DbMigration
    {
        public override void Up()
        {
            RenameTable(name: "SCH_SERVICIOS.tarifas_lavaderia", newName: "tarifas_lavanderia");
        }
        
        public override void Down()
        {
            RenameTable(name: "SCH_SERVICIOS.tarifas_lavanderia", newName: "tarifas_lavaderia");
        }
    }
}

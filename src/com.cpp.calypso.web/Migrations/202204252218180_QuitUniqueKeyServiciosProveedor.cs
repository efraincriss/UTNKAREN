namespace com.cpp.calypso.web.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class QuitUniqueKeyServiciosProveedor : DbMigration
    {
        public override void Up()
        {
            DropIndex("SCH_SERVICIOS.servicios_proveedor", "UX_SPRO_PRO_ID_SER_ID");
            CreateIndex("SCH_SERVICIOS.servicios_proveedor", "servicio_id");
            CreateIndex("SCH_SERVICIOS.servicios_proveedor", "proveedor_id");
        }
        
        public override void Down()
        {
            DropIndex("SCH_SERVICIOS.servicios_proveedor", new[] { "proveedor_id" });
            DropIndex("SCH_SERVICIOS.servicios_proveedor", new[] { "servicio_id" });
            CreateIndex("SCH_SERVICIOS.servicios_proveedor", new[] { "proveedor_id", "servicio_id" }, unique: true, name: "UX_SPRO_PRO_ID_SER_ID");
        }
    }
}

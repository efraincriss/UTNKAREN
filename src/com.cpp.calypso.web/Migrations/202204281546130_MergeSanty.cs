namespace com.cpp.calypso.web.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class MergeSanty : DbMigration
    {
        public override void Up()
        {
            /*DropIndex("SCH_SERVICIOS.servicios_proveedor", "UX_SPRO_PRO_ID_SER_ID");
            AddColumn("SCH_SERVICIOS.detalles_reservas", "aplica_lavanderia", c => c.Boolean(nullable: false));
            AddColumn("SCH_SERVICIOS.reservas_hoteles", "aplica_lavanderia", c => c.Boolean(nullable: false));
            CreateIndex("SCH_SERVICIOS.servicios_proveedor", "servicio_id");
            CreateIndex("SCH_SERVICIOS.servicios_proveedor", "proveedor_id");*/
        }
        
        public override void Down()
        { 
          /*  DropIndex("SCH_SERVICIOS.servicios_proveedor", new[] { "proveedor_id" });
            DropIndex("SCH_SERVICIOS.servicios_proveedor", new[] { "servicio_id" });
            DropColumn("SCH_SERVICIOS.reservas_hoteles", "aplica_lavanderia");
            DropColumn("SCH_SERVICIOS.detalles_reservas", "aplica_lavanderia");
             CreateIndex("SCH_SERVICIOS.servicios_proveedor", new[] { "proveedor_id", "servicio_id" }, unique: true, name: "UX_SPRO_PRO_ID_SER_ID");
         */   
    }
    }
}

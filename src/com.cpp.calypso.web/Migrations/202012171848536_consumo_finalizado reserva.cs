namespace com.cpp.calypso.web.Migrations
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity.Infrastructure.Annotations;
    using System.Data.Entity.Migrations;

    public partial class consumo_finalizadoreserva : DbMigration
    {
        public override void Up()
        {
            AddColumn("SCH_SERVICIOS.reservas_hoteles", "consumo_finalizado", c => c.Boolean(nullable: false));
            }
        public override void Down() { }
    }
}

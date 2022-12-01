namespace com.cpp.calypso.web.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class PorcentajeParticipacionDirectos : DbMigration
    {
        public override void Up()
        {
            AddColumn("SCH_INGENIERIA.certificados_ingenieria", "porcentaje_participacion_directos", c => c.Decimal(nullable: false, precision: 18, scale: 2));
        }
        
        public override void Down()
        {
            DropColumn("SCH_INGENIERIA.certificados_ingenieria", "porcentaje_participacion_directos");
        }
    }
}

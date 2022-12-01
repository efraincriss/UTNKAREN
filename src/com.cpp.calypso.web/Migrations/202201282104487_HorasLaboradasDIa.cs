namespace com.cpp.calypso.web.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class HorasLaboradasDIa : DbMigration
    {
        public override void Up()
        {
            AddColumn("SCH_INGENIERIA.colaborador_certificacion_ingenieria", "horas_laboradas_dia", c => c.Decimal(nullable: false, precision: 18, scale: 2));
        }
        
        public override void Down()
        {
            DropColumn("SCH_INGENIERIA.colaborador_certificacion_ingenieria", "horas_laboradas_dia");
        }
    }
}

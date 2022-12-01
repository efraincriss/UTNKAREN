namespace com.cpp.calypso.web.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class HorasLaboradasDIa1 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("SCH_INGENIERIA.colaborador_certificacion_ingenieria", "horas_laboradas_dia", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("SCH_INGENIERIA.colaborador_certificacion_ingenieria", "horas_laboradas_dia", c => c.Decimal(nullable: false, precision: 18, scale: 2));
        }
    }
}

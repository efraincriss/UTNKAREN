namespace com.cpp.calypso.web.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class FasesProeyctisAvances : DbMigration
    {
        public override void Up()
        {
            AddColumn("SCH_INGENIERIA.avances_porcentaje_proyecto", "unaFase", c => c.Boolean(nullable: false));
            AddColumn("SCH_INGENIERIA.resumen_certificado_ingenieria", "unaFase", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("SCH_INGENIERIA.resumen_certificado_ingenieria", "unaFase");
            DropColumn("SCH_INGENIERIA.avances_porcentaje_proyecto", "unaFase");
        }
    }
}

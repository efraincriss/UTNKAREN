namespace com.cpp.calypso.web.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ColumnaEsViaticoRegistroCertificados : DbMigration
    {
        public override void Up()
        {
            AddColumn("SCH_INGENIERIA.gastos_directos_certificado", "EsViatico", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("SCH_INGENIERIA.gastos_directos_certificado", "EsViatico");
        }
    }
}

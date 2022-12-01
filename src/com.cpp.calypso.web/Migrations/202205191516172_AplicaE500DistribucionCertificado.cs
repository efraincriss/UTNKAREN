namespace com.cpp.calypso.web.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AplicaE500DistribucionCertificado : DbMigration
    {
        public override void Up()
        {
            AddColumn("SCH_INGENIERIA.distribucion_certificado_ingenieria", "AplicaE500", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("SCH_INGENIERIA.distribucion_certificado_ingenieria", "AplicaE500");
        }
    }
}

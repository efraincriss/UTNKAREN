namespace com.cpp.calypso.web.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CertificadoIdColumn : DbMigration
    {
        public override void Up()
        {
          //AddColumn("SCH_INGENIERIA.detalles_indirectos_ingenieria", "CertificadoId", c => c.Int());
        }
        
        public override void Down()
        {
          //  DropColumn("SCH_INGENIERIA.detalles_indirectos_ingenieria", "CertificadoId");
        }
    }
}

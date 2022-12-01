namespace com.cpp.calypso.web.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DetallesIndirectosIngenieria_CampoCertificado : DbMigration
    {
        public override void Up()
        {
            AddColumn("SCH_INGENIERIA.detalles_indirectos_ingenieria", "Certificado", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("SCH_INGENIERIA.detalles_indirectos_ingenieria", "Certificado");
        }
    }
}

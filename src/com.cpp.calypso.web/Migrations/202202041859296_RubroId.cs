namespace com.cpp.calypso.web.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RubroId : DbMigration
    {
        public override void Up()
        {
            DropIndex("SCH_INGENIERIA.gastos_directos_certificado", new[] { "RubroId" });
            AlterColumn("SCH_INGENIERIA.gastos_directos_certificado", "RubroId", c => c.Int());
            CreateIndex("SCH_INGENIERIA.gastos_directos_certificado", "RubroId");
        }
        
        public override void Down()
        {
            DropIndex("SCH_INGENIERIA.gastos_directos_certificado", new[] { "RubroId" });
            AlterColumn("SCH_INGENIERIA.gastos_directos_certificado", "RubroId", c => c.Int(nullable: false));
            CreateIndex("SCH_INGENIERIA.gastos_directos_certificado", "RubroId");
        }
    }
}

namespace com.cpp.calypso.web.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CampoProyectoDistribucionSaldo : DbMigration
    {
        public override void Up()
        {
            AddColumn("SCH_INGENIERIA.detalles_indirectos_ingenieria", "distribucion_proyectos", c => c.String());
            AddColumn("SCH_INGENIERIA.detalles_indirectos_ingenieria", "saldo", c => c.Decimal(nullable: false, precision: 18, scale: 2));
        }
        
        public override void Down()
        {
            DropColumn("SCH_INGENIERIA.detalles_indirectos_ingenieria", "saldo");
            DropColumn("SCH_INGENIERIA.detalles_indirectos_ingenieria", "distribucion_proyectos");
        }
    }
}

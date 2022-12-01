namespace com.cpp.calypso.web.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AplicaViaticoCampoColaboradorIngenieria : DbMigration
    {
        public override void Up()
        {
            AddColumn("SCH_INGENIERIA.colaborador_certificacion_ingenieria", "AplicaViatico", c => c.Boolean(nullable: false));
            AddColumn("SCH_INGENIERIA.gastos_directos_certificado", "AplicaViatico", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("SCH_INGENIERIA.gastos_directos_certificado", "AplicaViatico");
            DropColumn("SCH_INGENIERIA.colaborador_certificacion_ingenieria", "AplicaViatico");
        }
    }
}

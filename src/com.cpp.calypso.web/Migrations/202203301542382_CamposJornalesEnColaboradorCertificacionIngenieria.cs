namespace com.cpp.calypso.web.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CamposJornalesEnColaboradorCertificacionIngenieria : DbMigration
    {
        public override void Up()
        {
            AddColumn("SCH_INGENIERIA.colaborador_certificacion_ingenieria", "es_jornal", c => c.Boolean(nullable: false));
            AddColumn("SCH_INGENIERIA.colaborador_certificacion_ingenieria", "es_gasto_directo", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("SCH_INGENIERIA.colaborador_certificacion_ingenieria", "es_gasto_directo");
            DropColumn("SCH_INGENIERIA.colaborador_certificacion_ingenieria", "es_jornal");
        }
    }
}

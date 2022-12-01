namespace com.cpp.calypso.web.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AplicaViaticoDirecto : DbMigration
    {
        public override void Up()
        {
            AddColumn("SCH_INGENIERIA.colaborador_certificacion_ingenieria", "AplicaViaticoDirecto", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("SCH_INGENIERIA.colaborador_certificacion_ingenieria", "AplicaViaticoDirecto");
        }
    }
}

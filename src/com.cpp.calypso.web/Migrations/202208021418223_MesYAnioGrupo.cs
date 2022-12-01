namespace com.cpp.calypso.web.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class MesYAnioGrupo : DbMigration
    {
        public override void Up()
        {
            AddColumn("SCH_INGENIERIA.grupo_certificado_ingenieria", "Mes", c => c.Int(nullable: false));
            AddColumn("SCH_INGENIERIA.grupo_certificado_ingenieria", "Anio", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("SCH_INGENIERIA.grupo_certificado_ingenieria", "Anio");
            DropColumn("SCH_INGENIERIA.grupo_certificado_ingenieria", "Mes");
        }
    }
}

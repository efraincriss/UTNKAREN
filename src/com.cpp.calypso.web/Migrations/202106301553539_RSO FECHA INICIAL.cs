namespace com.cpp.calypso.web.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RSOFECHAINICIAL : DbMigration
    {
        public override void Up()
        {
            AddColumn("SCH_PROYECTOS.rso_cabeceras", "fecha_inicio", c => c.DateTime());
        }
        
        public override void Down()
        {
            DropColumn("SCH_PROYECTOS.rso_cabeceras", "fecha_inicio");
        }
    }
}

namespace com.cpp.calypso.web.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CampoLogoPredetermindadoyPeriodDiasCurvaRdo : DbMigration
    {
        public override void Up()
        {
            AddColumn("SCH_PROYECTOS.proyectos", "usar_logo_prederminado", c => c.Boolean(nullable: false));
            AddColumn("SCH_PROYECTOS.proyectos", "periodo_curva", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("SCH_PROYECTOS.proyectos", "periodo_curva");
            DropColumn("SCH_PROYECTOS.proyectos", "usar_logo_prederminado");
        }
    }
}

namespace com.cpp.calypso.web.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class idRubroRDO : DbMigration
    {
        public override void Up()
        {
            AddColumn("SCH_PROYECTOS.computos", "id_rubro_RDO", c => c.String());
            AddColumn("SCH_PROYECTOS.rdo_detalles_eac", "id_rubro", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("SCH_PROYECTOS.rdo_detalles_eac", "id_rubro");
            DropColumn("SCH_PROYECTOS.computos", "id_rubro_RDO");
        }
    }
}

namespace com.cpp.calypso.web.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Oacientes2w : DbMigration
    {
        public override void Up()
        {
            AddColumn("SCH_MNA.paciente", "Hospitalizacion", c => c.String());
            AddColumn("SCH_MNA.paciente", "Emergencia", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("SCH_MNA.paciente", "Emergencia");
            DropColumn("SCH_MNA.paciente", "Hospitalizacion");
        }
    }
}

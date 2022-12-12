namespace com.cpp.calypso.web.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Katz : DbMigration
    {
        public override void Up()
        {
            AddColumn("SCH_MNA.katz", "CalificacionDependiente", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("SCH_MNA.katz", "CalificacionDependiente");
        }
    }
}

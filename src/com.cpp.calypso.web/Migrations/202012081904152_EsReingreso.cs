namespace com.cpp.calypso.web.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class EsReingreso : DbMigration
    {
        public override void Up()
        {
            AddColumn("SCH_RRHH.colaboradores", "es_reingreso", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("SCH_RRHH.colaboradores", "es_reingreso");
        }
    }
}

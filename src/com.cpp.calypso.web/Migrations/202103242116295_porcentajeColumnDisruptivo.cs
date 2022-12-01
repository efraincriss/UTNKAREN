namespace com.cpp.calypso.web.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class porcentajeColumnDisruptivo : DbMigration
    {
        public override void Up()
        {
            AddColumn("SCH_PROYECTOS.obras_disruptivo", "porcentaje_disruptivo", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("SCH_PROYECTOS.obras_disruptivo", "porcentaje_disruptivo");
        }
    }
}

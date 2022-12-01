namespace com.cpp.calypso.web.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Estructura_Inicial1 : DbMigration
    {
        public override void Up()
        {
            AddColumn("SCH_MNA.katz", "Puntuacion", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AddColumn("SCH_MNA.mna", "Puntuacion", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AddColumn("SCH_MNA.mna", "DetallePuntuacion", c => c.String());
            AddColumn("SCH_MNA.mna", "ValoracionCompleta", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("SCH_MNA.mna", "ValoracionCompleta");
            DropColumn("SCH_MNA.mna", "DetallePuntuacion");
            DropColumn("SCH_MNA.mna", "Puntuacion");
            DropColumn("SCH_MNA.katz", "Puntuacion");
        }
    }
}

namespace com.cpp.calypso.web.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class EtapasYDesgloceEspecialidad : DbMigration
    {
        public override void Up()
        {
            AddColumn("SCH_INGENIERIA.colaborador_certificacion_ingenieria", "AreaId", c => c.Int(nullable: false));
            AddColumn("SCH_INGENIERIA.gastos_directos_certificado", "NombreEtapa", c => c.String());
            DropColumn("SCH_INGENIERIA.detalles_indirectos_ingenieria", "tipo_colaborador");
        }
        
        public override void Down()
        {
            AddColumn("SCH_INGENIERIA.detalles_indirectos_ingenieria", "tipo_colaborador", c => c.Int(nullable: false));
            DropColumn("SCH_INGENIERIA.gastos_directos_certificado", "NombreEtapa");
            DropColumn("SCH_INGENIERIA.colaborador_certificacion_ingenieria", "AreaId");
        }
    }
}

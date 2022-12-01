namespace com.cpp.calypso.web.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ContratoIdporClienteId : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("SCH_INGENIERIA.grupo_certificado_ingenieria", "contrato_id", "SCH_PROYECTOS.contratos");
            DropIndex("SCH_INGENIERIA.grupo_certificado_ingenieria", new[] { "contrato_id" });
            AddColumn("SCH_INGENIERIA.grupo_certificado_ingenieria", "cliente_id", c => c.Int(nullable: false));
            CreateIndex("SCH_INGENIERIA.grupo_certificado_ingenieria", "cliente_id");
            AddForeignKey("SCH_INGENIERIA.grupo_certificado_ingenieria", "cliente_id", "SCH_PROYECTOS.clientes", "Id");
            DropColumn("SCH_INGENIERIA.grupo_certificado_ingenieria", "contrato_id");
        }
        
        public override void Down()
        {
            AddColumn("SCH_INGENIERIA.grupo_certificado_ingenieria", "contrato_id", c => c.Int(nullable: false));
            DropForeignKey("SCH_INGENIERIA.grupo_certificado_ingenieria", "cliente_id", "SCH_PROYECTOS.clientes");
            DropIndex("SCH_INGENIERIA.grupo_certificado_ingenieria", new[] { "cliente_id" });
            DropColumn("SCH_INGENIERIA.grupo_certificado_ingenieria", "cliente_id");
            CreateIndex("SCH_INGENIERIA.grupo_certificado_ingenieria", "contrato_id");
            AddForeignKey("SCH_INGENIERIA.grupo_certificado_ingenieria", "contrato_id", "SCH_PROYECTOS.contratos", "Id");
        }
    }
}

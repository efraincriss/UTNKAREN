namespace com.cpp.calypso.web.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DisciplinaId : DbMigration
    {
        public override void Up()
        {
            DropIndex("SCH_INGENIERIA.colaborador_certificacion_ingenieria", new[] { "catalogo_disciplina_id" });
            AlterColumn("SCH_INGENIERIA.colaborador_certificacion_ingenieria", "catalogo_disciplina_id", c => c.Int());
            CreateIndex("SCH_INGENIERIA.colaborador_certificacion_ingenieria", "catalogo_disciplina_id");
        }
        
        public override void Down()
        {
            DropIndex("SCH_INGENIERIA.colaborador_certificacion_ingenieria", new[] { "catalogo_disciplina_id" });
            AlterColumn("SCH_INGENIERIA.colaborador_certificacion_ingenieria", "catalogo_disciplina_id", c => c.Int(nullable: false));
            CreateIndex("SCH_INGENIERIA.colaborador_certificacion_ingenieria", "catalogo_disciplina_id");
        }
    }
}

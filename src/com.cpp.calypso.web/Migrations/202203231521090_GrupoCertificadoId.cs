namespace com.cpp.calypso.web.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class GrupoCertificadoId : DbMigration
    {
        public override void Up()
        {
            AddColumn("SCH_INGENIERIA.resumen_certificado_ingenieria", "GrupoCertificadoIngenieriaId", c => c.Int(nullable: false));
            CreateIndex("SCH_INGENIERIA.resumen_certificado_ingenieria", "GrupoCertificadoIngenieriaId");
            AddForeignKey("SCH_INGENIERIA.resumen_certificado_ingenieria", "GrupoCertificadoIngenieriaId", "SCH_INGENIERIA.grupo_certificado_ingenieria", "id");
        }
        
        public override void Down()
        {
            DropForeignKey("SCH_INGENIERIA.resumen_certificado_ingenieria", "GrupoCertificadoIngenieriaId", "SCH_INGENIERIA.grupo_certificado_ingenieria");
            DropIndex("SCH_INGENIERIA.resumen_certificado_ingenieria", new[] { "GrupoCertificadoIngenieriaId" });
            DropColumn("SCH_INGENIERIA.resumen_certificado_ingenieria", "GrupoCertificadoIngenieriaId");
        }
    }
}

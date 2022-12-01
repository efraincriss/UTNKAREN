namespace com.cpp.calypso.web.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Migratipn : DbMigration
    {
        public override void Up()
        {
            /*DropForeignKey("SCH_INGENIERIA.resumen_certificado_ingenieria", "GrupoCertificadoIngenieriaId", "SCH_INGENIERIA.grupo_certificado_ingenieria");
            DropIndex("SCH_INGENIERIA.resumen_certificado_ingenieria", new[] { "GrupoCertificadoIngenieriaId" });
            DropColumn("SCH_PROYECTOS.oferta_comercial", "monto_ofertado_migracion_actual");
            DropColumn("SCH_PROYECTOS.oferta_comercial", "monto_so_aprobado_migracion_anterior");
            DropColumn("SCH_PROYECTOS.oferta_comercial", "monto_so_aprobado_migracion_actual");
            DropColumn("SCH_INGENIERIA.colaborador_certificacion_ingenieria", "AplicaViaticoDirecto");
            DropColumn("SCH_INGENIERIA.resumen_certificado_ingenieria", "GrupoCertificadoIngenieriaId");*/
        }
        
        public override void Down()
        {
            /*
            AddColumn("SCH_INGENIERIA.resumen_certificado_ingenieria", "GrupoCertificadoIngenieriaId", c => c.Int(nullable: false));
            AddColumn("SCH_INGENIERIA.colaborador_certificacion_ingenieria", "AplicaViaticoDirecto", c => c.Boolean(nullable: false));
            AddColumn("SCH_PROYECTOS.oferta_comercial", "monto_so_aprobado_migracion_actual", c => c.Decimal(nullable: false, precision: 24, scale: 16));
            AddColumn("SCH_PROYECTOS.oferta_comercial", "monto_so_aprobado_migracion_anterior", c => c.Decimal(nullable: false, precision: 24, scale: 16));
            AddColumn("SCH_PROYECTOS.oferta_comercial", "monto_ofertado_migracion_actual", c => c.Decimal(nullable: false, precision: 24, scale: 16));
            CreateIndex("SCH_INGENIERIA.resumen_certificado_ingenieria", "GrupoCertificadoIngenieriaId");
            AddForeignKey("SCH_INGENIERIA.resumen_certificado_ingenieria", "GrupoCertificadoIngenieriaId", "SCH_INGENIERIA.grupo_certificado_ingenieria", "id");
    */   
    }
    }
}

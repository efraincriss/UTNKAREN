namespace com.cpp.calypso.web.Migrations
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity.Infrastructure.Annotations;
    using System.Data.Entity.Migrations;
    
    public partial class DistribucionProyectosCertificado : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "SCH_INGENIERIA.distribucion_certificado_ingenieria",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        GrupoCertificadoId = c.Int(nullable: false),
                        ProyectoId = c.Int(nullable: false),
                        AplicaViatico = c.Boolean(nullable: false),
                        AplicaIndirecto = c.Boolean(nullable: false),
                        eliminado = c.Boolean(nullable: false),
                    },
                annotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_DistribucionCertificadoIngenieria_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                })
                .PrimaryKey(t => t.id)
                .ForeignKey("SCH_INGENIERIA.grupo_certificado_ingenieria", t => t.GrupoCertificadoId)
                .ForeignKey("SCH_PROYECTOS.proyectos", t => t.ProyectoId)
                .Index(t => t.GrupoCertificadoId)
                .Index(t => t.ProyectoId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("SCH_INGENIERIA.distribucion_certificado_ingenieria", "ProyectoId", "SCH_PROYECTOS.proyectos");
            DropForeignKey("SCH_INGENIERIA.distribucion_certificado_ingenieria", "GrupoCertificadoId", "SCH_INGENIERIA.grupo_certificado_ingenieria");
            DropIndex("SCH_INGENIERIA.distribucion_certificado_ingenieria", new[] { "ProyectoId" });
            DropIndex("SCH_INGENIERIA.distribucion_certificado_ingenieria", new[] { "GrupoCertificadoId" });
            DropTable("SCH_INGENIERIA.distribucion_certificado_ingenieria",
                removedAnnotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_DistribucionCertificadoIngenieria_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                });
        }
    }
}

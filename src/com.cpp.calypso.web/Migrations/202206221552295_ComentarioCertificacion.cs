namespace com.cpp.calypso.web.Migrations
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity.Infrastructure.Annotations;
    using System.Data.Entity.Migrations;
    
    public partial class ComentarioCertificacion : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "SCH_INGENIERIA.comentarios_certificado_ingenieria",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        proyecto_id = c.Int(nullable: false),
                        certificado_id = c.Int(),
                        fecha_carga = c.DateTime(nullable: false),
                        Comentario = c.String(),
                        vigente = c.Boolean(nullable: false),
                        usuario_creacion = c.Long(),
                        fecha_creacion = c.DateTime(nullable: false),
                        usuario_actualizacion = c.Long(),
                        fecha_actualizacion = c.DateTime(),
                        usuario_eliminacion = c.Long(),
                        fecha_eliminacion = c.DateTime(),
                    },
                annotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_ComentarioCertificado_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                })
                .PrimaryKey(t => t.id)
                .ForeignKey("SCH_PROYECTOS.proyectos", t => t.proyecto_id)
                .Index(t => t.proyecto_id);
            
           /* CreateTable(
                "SCH_INGENIERIA.distribucion_certificado_ingenieria",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        GrupoCertificadoId = c.Int(nullable: false),
                        ProyectoId = c.Int(nullable: false),
                        AplicaViatico = c.Boolean(nullable: false),
                        AplicaIndirecto = c.Boolean(nullable: false),
                        AplicaE500 = c.Boolean(nullable: false),
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
            
            AddColumn("SCH_PROYECTOS.proyectos", "anio_certificacion_ingenieria", c => c.Int(nullable: false));
            AddColumn("SCH_INGENIERIA.resumen_certificado_ingenieria", "anioProyecto", c => c.Int(nullable: false));*/
        }
        
        public override void Down()
        {
           /* DropForeignKey("SCH_INGENIERIA.distribucion_certificado_ingenieria", "ProyectoId", "SCH_PROYECTOS.proyectos");
            DropForeignKey("SCH_INGENIERIA.distribucion_certificado_ingenieria", "GrupoCertificadoId", "SCH_INGENIERIA.grupo_certificado_ingenieria");
            DropForeignKey("SCH_INGENIERIA.comentarios_certificado_ingenieria", "proyecto_id", "SCH_PROYECTOS.proyectos");
            DropIndex("SCH_INGENIERIA.distribucion_certificado_ingenieria", new[] { "ProyectoId" });
            DropIndex("SCH_INGENIERIA.distribucion_certificado_ingenieria", new[] { "GrupoCertificadoId" });
            DropIndex("SCH_INGENIERIA.comentarios_certificado_ingenieria", new[] { "proyecto_id" });
            DropColumn("SCH_INGENIERIA.resumen_certificado_ingenieria", "anioProyecto");
            DropColumn("SCH_PROYECTOS.proyectos", "anio_certificacion_ingenieria");
            DropTable("SCH_INGENIERIA.distribucion_certificado_ingenieria",
                removedAnnotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_DistribucionCertificadoIngenieria_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                });
            DropTable("SCH_INGENIERIA.comentarios_certificado_ingenieria",
                removedAnnotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_ComentarioCertificado_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                });*/
        }
    }
}

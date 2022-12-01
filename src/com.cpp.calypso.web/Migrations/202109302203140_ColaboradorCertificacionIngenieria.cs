namespace com.cpp.calypso.web.Migrations
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity.Infrastructure.Annotations;
    using System.Data.Entity.Migrations;
    
    public partial class ColaboradorCertificacionIngenieria : DbMigration
    {
        public override void Up()
        {
          /*  CreateTable(
                "SCH_INGENIERIA.colaborador_certificacion_ingenieria",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        colaborador_id = c.Int(nullable: false),
                        fecha_desde = c.DateTime(nullable: false),
                        fecha_hasta = c.DateTime(),
                        catalogo_disciplina_id = c.Int(nullable: false),
                        catalogo_modalidad_id = c.Int(nullable: false),
                        catalogo_ubicacion_id = c.Int(nullable: false),
                        categoria_i_d = c.String(),
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
                    { "DynamicFilter_ColaboradorCertificadoIngenieria_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                })
                .PrimaryKey(t => t.id)
                .ForeignKey("SCH_RRHH.colaboradores", t => t.colaborador_id)
                .ForeignKey("SCH_CATALOGOS.catalogos", t => t.catalogo_disciplina_id)
                .ForeignKey("SCH_CATALOGOS.catalogos", t => t.catalogo_modalidad_id)
                .ForeignKey("SCH_CATALOGOS.catalogos", t => t.catalogo_ubicacion_id)
                .Index(t => t.colaborador_id)
                .Index(t => t.catalogo_disciplina_id)
                .Index(t => t.catalogo_modalidad_id)
                .Index(t => t.catalogo_ubicacion_id);*/
            
        }
        
        public override void Down()
        {
            /*
            DropForeignKey("SCH_INGENIERIA.colaborador_certificacion_ingenieria", "catalogo_ubicacion_id", "SCH_CATALOGOS.catalogos");
            DropForeignKey("SCH_INGENIERIA.colaborador_certificacion_ingenieria", "catalogo_modalidad_id", "SCH_CATALOGOS.catalogos");
            DropForeignKey("SCH_INGENIERIA.colaborador_certificacion_ingenieria", "catalogo_disciplina_id", "SCH_CATALOGOS.catalogos");
            DropForeignKey("SCH_INGENIERIA.colaborador_certificacion_ingenieria", "colaborador_id", "SCH_RRHH.colaboradores");
            DropIndex("SCH_INGENIERIA.colaborador_certificacion_ingenieria", new[] { "catalogo_ubicacion_id" });
            DropIndex("SCH_INGENIERIA.colaborador_certificacion_ingenieria", new[] { "catalogo_modalidad_id" });
            DropIndex("SCH_INGENIERIA.colaborador_certificacion_ingenieria", new[] { "catalogo_disciplina_id" });
            DropIndex("SCH_INGENIERIA.colaborador_certificacion_ingenieria", new[] { "colaborador_id" });
            DropTable("SCH_INGENIERIA.colaborador_certificacion_ingenieria",
                removedAnnotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_ColaboradorCertificadoIngenieria_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                });*/
        }
    }
}

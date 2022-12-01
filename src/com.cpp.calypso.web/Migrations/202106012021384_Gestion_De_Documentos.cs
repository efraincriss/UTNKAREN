namespace com.cpp.calypso.web.Migrations
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity.Infrastructure.Annotations;
    using System.Data.Entity.Migrations;
    
    public partial class Gestion_De_Documentos : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "SCH_DOCUMENTOS.caepetas",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        codigo = c.String(nullable: false, maxLength: 20),
                        nombre_corto = c.String(nullable: false, maxLength: 100),
                        nombre_completo = c.String(nullable: false, maxLength: 1000),
                        descripcion = c.String(nullable: false, maxLength: 400),
                        estado = c.String(nullable: false, maxLength: 100),
                        publicado = c.Boolean(nullable: false),
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
                    { "DynamicFilter_Carpeta_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                })
                .PrimaryKey(t => t.id);
            
            CreateTable(
                "SCH_DOCUMENTOS.documentos",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        codigo = c.String(nullable: false),
                        nombre = c.String(nullable: false),
                        carpeta_id = c.Int(nullable: false),
                        cantidad_paginas = c.Int(nullable: false),
                        tipo_documento = c.String(nullable: false),
                        es_imagen = c.Boolean(nullable: false),
                        imagen = c.String(),
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
                    { "DynamicFilter_Documento_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                })
                .PrimaryKey(t => t.id)
                .ForeignKey("SCH_DOCUMENTOS.caepetas", t => t.carpeta_id)
                .Index(t => t.carpeta_id);
            
            CreateTable(
                "SCH_DOCUMENTOS.secciones",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        seccion = c.String(nullable: false, maxLength: 1000),
                        contenido = c.String(nullable: false),
                        documento_id = c.Int(nullable: false),
                        seccion_id = c.Int(nullable: false),
                        numero_pagina = c.String(nullable: false, maxLength: 10),
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
                    { "DynamicFilter_Seccion_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                })
                .PrimaryKey(t => t.id)
                .ForeignKey("SCH_DOCUMENTOS.documentos", t => t.documento_id)
                .ForeignKey("SCH_DOCUMENTOS.secciones", t => t.seccion_id)
                .Index(t => t.documento_id)
                .Index(t => t.seccion_id);
            
            CreateTable(
                "SCH_DOCUMENTOS.usuarios_autorizados",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        carpeta_id = c.Int(nullable: false),
                        usuario_id = c.Int(nullable: false),
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
                    { "DynamicFilter_UsuarioAutorizado_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                })
                .PrimaryKey(t => t.id)
                .ForeignKey("SCH_DOCUMENTOS.caepetas", t => t.carpeta_id)
                .ForeignKey("SCH_USUARIOS.usuarios", t => t.usuario_id)
                .Index(t => t.carpeta_id)
                .Index(t => t.usuario_id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("SCH_DOCUMENTOS.usuarios_autorizados", "usuario_id", "SCH_USUARIOS.usuarios");
            DropForeignKey("SCH_DOCUMENTOS.usuarios_autorizados", "carpeta_id", "SCH_DOCUMENTOS.caepetas");
            DropForeignKey("SCH_DOCUMENTOS.secciones", "seccion_id", "SCH_DOCUMENTOS.secciones");
            DropForeignKey("SCH_DOCUMENTOS.secciones", "documento_id", "SCH_DOCUMENTOS.documentos");
            DropForeignKey("SCH_DOCUMENTOS.documentos", "carpeta_id", "SCH_DOCUMENTOS.caepetas");
            DropIndex("SCH_DOCUMENTOS.usuarios_autorizados", new[] { "usuario_id" });
            DropIndex("SCH_DOCUMENTOS.usuarios_autorizados", new[] { "carpeta_id" });
            DropIndex("SCH_DOCUMENTOS.secciones", new[] { "seccion_id" });
            DropIndex("SCH_DOCUMENTOS.secciones", new[] { "documento_id" });
            DropIndex("SCH_DOCUMENTOS.documentos", new[] { "carpeta_id" });
            DropTable("SCH_DOCUMENTOS.usuarios_autorizados",
                removedAnnotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_UsuarioAutorizado_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                });
            DropTable("SCH_DOCUMENTOS.secciones",
                removedAnnotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_Seccion_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                });
            DropTable("SCH_DOCUMENTOS.documentos",
                removedAnnotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_Documento_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                });
            DropTable("SCH_DOCUMENTOS.caepetas",
                removedAnnotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_Carpeta_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                });
        }
    }
}

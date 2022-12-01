namespace com.cpp.calypso.web.Migrations
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity.Infrastructure.Annotations;
    using System.Data.Entity.Migrations;
    
    public partial class ColaboradorIngenieria_2 : DbMigration
    {
        public override void Up()
        {
            AlterTableAnnotations(
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
                annotations: new Dictionary<string, AnnotationValues>
                {
                    { 
                        "DynamicFilter_ColaboradorCertificacionIngenieria_SoftDelete",
                        new AnnotationValues(oldValue: null, newValue: "EntityFramework.DynamicFilters.DynamicFilterDefinition")
                    },
                    { 
                        "DynamicFilter_ColaboradorCertificadoIngenieria_SoftDelete",
                        new AnnotationValues(oldValue: "EntityFramework.DynamicFilters.DynamicFilterDefinition", newValue: null)
                    },
                });
            
        }
        
        public override void Down()
        {
            AlterTableAnnotations(
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
                annotations: new Dictionary<string, AnnotationValues>
                {
                    { 
                        "DynamicFilter_ColaboradorCertificacionIngenieria_SoftDelete",
                        new AnnotationValues(oldValue: "EntityFramework.DynamicFilters.DynamicFilterDefinition", newValue: null)
                    },
                    { 
                        "DynamicFilter_ColaboradorCertificadoIngenieria_SoftDelete",
                        new AnnotationValues(oldValue: null, newValue: "EntityFramework.DynamicFilters.DynamicFilterDefinition")
                    },
                });
            
        }
    }
}

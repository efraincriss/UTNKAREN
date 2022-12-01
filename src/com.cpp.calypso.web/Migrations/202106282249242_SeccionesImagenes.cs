namespace com.cpp.calypso.web.Migrations
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity.Infrastructure.Annotations;
    using System.Data.Entity.Migrations;
    
    public partial class SeccionesImagenes : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "SCH_DOCUMENTOS.imagenes_seccion",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        seccion_id = c.Int(nullable: false),
                        ImagenBase64 = c.String(),
                        nombre_imagen = c.String(),
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
                    { "DynamicFilter_ImagenSeccion_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                })
                .PrimaryKey(t => t.id)
                .ForeignKey("SCH_DOCUMENTOS.secciones", t => t.seccion_id)
                .Index(t => t.seccion_id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("SCH_DOCUMENTOS.imagenes_seccion", "seccion_id", "SCH_DOCUMENTOS.secciones");
            DropIndex("SCH_DOCUMENTOS.imagenes_seccion", new[] { "seccion_id" });
            DropTable("SCH_DOCUMENTOS.imagenes_seccion",
                removedAnnotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_ImagenSeccion_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                });
        }
    }
}

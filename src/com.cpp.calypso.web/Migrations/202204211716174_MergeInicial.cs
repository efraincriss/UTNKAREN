namespace com.cpp.calypso.web.Migrations
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity.Infrastructure.Annotations;
    using System.Data.Entity.Migrations;
    
    public partial class MergeInicial : DbMigration
    {
        public override void Up()
        {
           /* AlterTableAnnotations(
                "SCH_SERVICIOS.servicios_proveedor",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        servicio_id = c.Int(nullable: false),
                        proveedor_id = c.Int(nullable: false),
                        estado = c.Int(nullable: false),
                        vigente = c.Boolean(nullable: false),
                    },
                annotations: new Dictionary<string, AnnotationValues>
                {
                    { 
                        "DynamicFilter_ServicioProveedor_SoftDelete",
                        new AnnotationValues(oldValue: null, newValue: "EntityFramework.DynamicFilters.DynamicFilterDefinition")
                    },
                });
            
            AddColumn("SCH_CATALOGOS.catalogos", "visualiza_movil", c => c.Boolean());
            AddColumn("SCH_PROYECTOS.oferta_comercial", "monto_ofertado_migracion_actual", c => c.Decimal(nullable: false, precision: 24, scale: 16));
            AddColumn("SCH_PROYECTOS.oferta_comercial", "monto_so_aprobado_migracion_anterior", c => c.Decimal(nullable: false, precision: 24, scale: 16));
            AddColumn("SCH_PROYECTOS.oferta_comercial", "monto_so_aprobado_migracion_actual", c => c.Decimal(nullable: false, precision: 24, scale: 16));
            AddColumn("SCH_SERVICIOS.servicios_proveedor", "vigente", c => c.Boolean(nullable: false));
            AddColumn("SCH_INGENIERIA.colaborador_certificacion_ingenieria", "AplicaViaticoDirecto", c => c.Boolean(nullable: false));
            AddColumn("SCH_INGENIERIA.resumen_certificado_ingenieria", "GrupoCertificadoIngenieriaId", c => c.Int(nullable: false));
            CreateIndex("SCH_INGENIERIA.resumen_certificado_ingenieria", "GrupoCertificadoIngenieriaId");
            AddForeignKey("SCH_INGENIERIA.resumen_certificado_ingenieria", "GrupoCertificadoIngenieriaId", "SCH_INGENIERIA.grupo_certificado_ingenieria", "id");
    */    
    }
        
        public override void Down()
        {
           /* DropForeignKey("SCH_INGENIERIA.resumen_certificado_ingenieria", "GrupoCertificadoIngenieriaId", "SCH_INGENIERIA.grupo_certificado_ingenieria");
            DropIndex("SCH_INGENIERIA.resumen_certificado_ingenieria", new[] { "GrupoCertificadoIngenieriaId" });
            DropColumn("SCH_INGENIERIA.resumen_certificado_ingenieria", "GrupoCertificadoIngenieriaId");
            DropColumn("SCH_INGENIERIA.colaborador_certificacion_ingenieria", "AplicaViaticoDirecto");
            DropColumn("SCH_SERVICIOS.servicios_proveedor", "vigente");
            DropColumn("SCH_PROYECTOS.oferta_comercial", "monto_so_aprobado_migracion_actual");
            DropColumn("SCH_PROYECTOS.oferta_comercial", "monto_so_aprobado_migracion_anterior");
            DropColumn("SCH_PROYECTOS.oferta_comercial", "monto_ofertado_migracion_actual");
            DropColumn("SCH_CATALOGOS.catalogos", "visualiza_movil");
            AlterTableAnnotations(
                "SCH_SERVICIOS.servicios_proveedor",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        servicio_id = c.Int(nullable: false),
                        proveedor_id = c.Int(nullable: false),
                        estado = c.Int(nullable: false),
                        vigente = c.Boolean(nullable: false),
                    },
                annotations: new Dictionary<string, AnnotationValues>
                {
                    { 
                        "DynamicFilter_ServicioProveedor_SoftDelete",
                        new AnnotationValues(oldValue: "EntityFramework.DynamicFilters.DynamicFilterDefinition", newValue: null)
                    },
                });*/
            
        }
    }
}

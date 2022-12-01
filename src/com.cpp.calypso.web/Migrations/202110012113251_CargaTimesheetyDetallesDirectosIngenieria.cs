namespace com.cpp.calypso.web.Migrations
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity.Infrastructure.Annotations;
    using System.Data.Entity.Migrations;
    
    public partial class CargaTimesheetyDetallesDirectosIngenieria : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "SCH_INGENIERIA.carga_timesheet",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        fecha_inicial = c.DateTime(nullable: false),
                        fecha_final = c.DateTime(nullable: false),
                        numero_registros = c.Int(nullable: false),
                        nombre_archivo = c.Int(nullable: false),
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
                    { "DynamicFilter_CargaTimesheet_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                })
                .PrimaryKey(t => t.id);
            
            CreateTable(
                "SCH_INGENIERIA.detalles_directos_ingenieria",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        cedula = c.String(),
                        colaborador_id = c.Int(nullable: false),
                        TipoRegistroId = c.Int(),
                        codigo_proyecto = c.String(),
                        numero_horas = c.Decimal(nullable: false, precision: 18, scale: 2),
                        nombre_ejecutante = c.String(),
                        fecha_trabajo = c.DateTime(nullable: false),
                        observaciones = c.String(),
                        etapa_id = c.Int(nullable: false),
                        especialidad_id = c.Int(nullable: false),
                        estado = c.String(),
                        locacion = c.Int(nullable: false),
                        modalidad_id = c.Int(nullable: false),
                        es_directo = c.Boolean(nullable: false),
                        certificado_id = c.Int(),
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
                    { "DynamicFilter_DetallesDirectosIngenieria_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                })
                .PrimaryKey(t => t.id)
                .ForeignKey("SCH_RRHH.colaboradores", t => t.colaborador_id)
                .ForeignKey("SCH_CATALOGOS.catalogos", t => t.especialidad_id)
                .ForeignKey("SCH_CATALOGOS.catalogos", t => t.etapa_id)
                .ForeignKey("SCH_CATALOGOS.catalogos", t => t.locacion)
                .ForeignKey("SCH_CATALOGOS.catalogos", t => t.modalidad_id)
                .ForeignKey("SCH_CATALOGOS.catalogos", t => t.TipoRegistroId)
                .Index(t => t.colaborador_id)
                .Index(t => t.TipoRegistroId)
                .Index(t => t.etapa_id)
                .Index(t => t.especialidad_id)
                .Index(t => t.locacion)
                .Index(t => t.modalidad_id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("SCH_INGENIERIA.detalles_directos_ingenieria", "TipoRegistroId", "SCH_CATALOGOS.catalogos");
            DropForeignKey("SCH_INGENIERIA.detalles_directos_ingenieria", "modalidad_id", "SCH_CATALOGOS.catalogos");
            DropForeignKey("SCH_INGENIERIA.detalles_directos_ingenieria", "locacion", "SCH_CATALOGOS.catalogos");
            DropForeignKey("SCH_INGENIERIA.detalles_directos_ingenieria", "etapa_id", "SCH_CATALOGOS.catalogos");
            DropForeignKey("SCH_INGENIERIA.detalles_directos_ingenieria", "especialidad_id", "SCH_CATALOGOS.catalogos");
            DropForeignKey("SCH_INGENIERIA.detalles_directos_ingenieria", "colaborador_id", "SCH_RRHH.colaboradores");
            DropIndex("SCH_INGENIERIA.detalles_directos_ingenieria", new[] { "modalidad_id" });
            DropIndex("SCH_INGENIERIA.detalles_directos_ingenieria", new[] { "locacion" });
            DropIndex("SCH_INGENIERIA.detalles_directos_ingenieria", new[] { "especialidad_id" });
            DropIndex("SCH_INGENIERIA.detalles_directos_ingenieria", new[] { "etapa_id" });
            DropIndex("SCH_INGENIERIA.detalles_directos_ingenieria", new[] { "TipoRegistroId" });
            DropIndex("SCH_INGENIERIA.detalles_directos_ingenieria", new[] { "colaborador_id" });
            DropTable("SCH_INGENIERIA.detalles_directos_ingenieria",
                removedAnnotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_DetallesDirectosIngenieria_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                });
            DropTable("SCH_INGENIERIA.carga_timesheet",
                removedAnnotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_CargaTimesheet_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                });
        }
    }
}

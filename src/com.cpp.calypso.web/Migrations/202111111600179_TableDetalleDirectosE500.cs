namespace com.cpp.calypso.web.Migrations
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity.Infrastructure.Annotations;
    using System.Data.Entity.Migrations;
    
    public partial class TableDetalleDirectosE500 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "SCH_INGENIERIA.detalles_directos_e500",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        identificacion = c.String(),
                        colaborador_id = c.Int(nullable: false),
                        cliente_id = c.Int(nullable: false),
                        tipo_registro_id = c.Int(),
                        numero_horas = c.Decimal(nullable: false, precision: 18, scale: 2),
                        nombre_ejecutante = c.String(),
                        fecha_trabajo = c.DateTime(nullable: false),
                        observaciones = c.String(),
                        etapa_id = c.Int(),
                        especialidad_id = c.Int(),
                        estado_registro_id = c.Int(nullable: false),
                        locacion = c.Int(),
                        modalidad_id = c.Int(),
                        certificado_id = c.Int(),
                        fecha_carga = c.DateTime(nullable: false),
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
                    { "DynamicFilter_DetalleDirectoE500_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                })
                .PrimaryKey(t => t.id)
                .ForeignKey("SCH_PROYECTOS.clientes", t => t.cliente_id)
                .ForeignKey("SCH_RRHH.colaboradores", t => t.colaborador_id)
                .ForeignKey("SCH_CATALOGOS.catalogos", t => t.especialidad_id)
                .ForeignKey("SCH_CATALOGOS.catalogos", t => t.estado_registro_id)
                .ForeignKey("SCH_CATALOGOS.catalogos", t => t.etapa_id)
                .ForeignKey("SCH_CATALOGOS.catalogos", t => t.locacion)
                .ForeignKey("SCH_CATALOGOS.catalogos", t => t.modalidad_id)
                .ForeignKey("SCH_CATALOGOS.catalogos", t => t.tipo_registro_id)
                .Index(t => t.colaborador_id)
                .Index(t => t.cliente_id)
                .Index(t => t.tipo_registro_id)
                .Index(t => t.etapa_id)
                .Index(t => t.especialidad_id)
                .Index(t => t.estado_registro_id)
                .Index(t => t.locacion)
                .Index(t => t.modalidad_id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("SCH_INGENIERIA.detalles_directos_e500", "tipo_registro_id", "SCH_CATALOGOS.catalogos");
            DropForeignKey("SCH_INGENIERIA.detalles_directos_e500", "modalidad_id", "SCH_CATALOGOS.catalogos");
            DropForeignKey("SCH_INGENIERIA.detalles_directos_e500", "locacion", "SCH_CATALOGOS.catalogos");
            DropForeignKey("SCH_INGENIERIA.detalles_directos_e500", "etapa_id", "SCH_CATALOGOS.catalogos");
            DropForeignKey("SCH_INGENIERIA.detalles_directos_e500", "estado_registro_id", "SCH_CATALOGOS.catalogos");
            DropForeignKey("SCH_INGENIERIA.detalles_directos_e500", "especialidad_id", "SCH_CATALOGOS.catalogos");
            DropForeignKey("SCH_INGENIERIA.detalles_directos_e500", "colaborador_id", "SCH_RRHH.colaboradores");
            DropForeignKey("SCH_INGENIERIA.detalles_directos_e500", "cliente_id", "SCH_PROYECTOS.clientes");
            DropIndex("SCH_INGENIERIA.detalles_directos_e500", new[] { "modalidad_id" });
            DropIndex("SCH_INGENIERIA.detalles_directos_e500", new[] { "locacion" });
            DropIndex("SCH_INGENIERIA.detalles_directos_e500", new[] { "estado_registro_id" });
            DropIndex("SCH_INGENIERIA.detalles_directos_e500", new[] { "especialidad_id" });
            DropIndex("SCH_INGENIERIA.detalles_directos_e500", new[] { "etapa_id" });
            DropIndex("SCH_INGENIERIA.detalles_directos_e500", new[] { "tipo_registro_id" });
            DropIndex("SCH_INGENIERIA.detalles_directos_e500", new[] { "cliente_id" });
            DropIndex("SCH_INGENIERIA.detalles_directos_e500", new[] { "colaborador_id" });
            DropTable("SCH_INGENIERIA.detalles_directos_e500",
                removedAnnotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_DetalleDirectoE500_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                });
        }
    }
}

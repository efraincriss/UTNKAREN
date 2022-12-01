namespace com.cpp.calypso.web.Migrations
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity.Infrastructure.Annotations;
    using System.Data.Entity.Migrations;
    
    public partial class TablesCertificacionIngenieriaGrupos : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "SCH_INGENIERIA.certificados_ingenieria",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        GrupoCertificadoIngenieriaId = c.Int(nullable: false),
                        ProyectoId = c.Int(nullable: false),
                        EstadoId = c.Int(nullable: false),
                        NumeroCertificado = c.Int(nullable: false),
                        OrdenServicioId = c.Int(),
                        AvanceRealIngenieria = c.Decimal(nullable: false, precision: 18, scale: 2),
                        HorasPresupuestadas = c.Decimal(nullable: false, precision: 18, scale: 2),
                        PorcentajeAsbuilt = c.Decimal(nullable: false, precision: 18, scale: 2),
                        MontoAnteriorCertificado = c.Decimal(nullable: false, precision: 18, scale: 2),
                        MontoActualCertificado = c.Decimal(nullable: false, precision: 18, scale: 2),
                        HorasAnteriorCertificadas = c.Decimal(nullable: false, precision: 18, scale: 2),
                        HorasActualCertificadas = c.Decimal(nullable: false, precision: 18, scale: 2),
                        DistribucionDirectos = c.Boolean(nullable: false),
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
                    { "DynamicFilter_CertificadoIngenieriaProyecto_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                })
                .PrimaryKey(t => t.id)
                .ForeignKey("SCH_INGENIERIA.grupo_certificado_ingenieria", t => t.GrupoCertificadoIngenieriaId)
                .ForeignKey("SCH_PROYECTOS.ordenes_servicio", t => t.OrdenServicioId)
                .ForeignKey("SCH_PROYECTOS.proyectos", t => t.ProyectoId)
                .Index(t => t.GrupoCertificadoIngenieriaId)
                .Index(t => t.ProyectoId)
                .Index(t => t.OrdenServicioId);
            
            CreateTable(
                "SCH_INGENIERIA.grupo_certificado_ingenieria",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        contrato_id = c.Int(nullable: false),
                        fecha_certificado = c.DateTime(nullable: false),
                        fecha_inicio = c.DateTime(nullable: false),
                        fecha_fin = c.DateTime(nullable: false),
                        fecha_generacion = c.DateTime(nullable: false),
                        estado_id = c.Int(nullable: false),
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
                    { "DynamicFilter_GrupoCertificadoIngenieria_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                })
                .PrimaryKey(t => t.id)
                .ForeignKey("SCH_PROYECTOS.contratos", t => t.contrato_id)
                .Index(t => t.contrato_id);
            
            CreateTable(
                "SCH_INGENIERIA.gastos_directos_certificado",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        CertificadoIngenieriaProyectoId = c.Int(nullable: false),
                        TipoGastoId = c.Int(nullable: false),
                        ColaboradorId = c.Int(nullable: false),
                        RubroId = c.Int(nullable: false),
                        UnidadId = c.Int(),
                        TotalHoras = c.Decimal(nullable: false, precision: 18, scale: 2),
                        TarifaHoras = c.Decimal(nullable: false, precision: 18, scale: 2),
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
                    { "DynamicFilter_GastoDirectoCertificado_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                })
                .PrimaryKey(t => t.id)
                .ForeignKey("SCH_INGENIERIA.certificados_ingenieria", t => t.CertificadoIngenieriaProyectoId)
                .ForeignKey("SCH_RRHH.colaboradores", t => t.ColaboradorId)
                .ForeignKey("SCH_PROYECTOS.detalle_preciarios", t => t.RubroId)
                .ForeignKey("SCH_CATALOGOS.catalogos", t => t.UnidadId)
                .Index(t => t.CertificadoIngenieriaProyectoId)
                .Index(t => t.ColaboradorId)
                .Index(t => t.RubroId)
                .Index(t => t.UnidadId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("SCH_INGENIERIA.gastos_directos_certificado", "UnidadId", "SCH_CATALOGOS.catalogos");
            DropForeignKey("SCH_INGENIERIA.gastos_directos_certificado", "RubroId", "SCH_PROYECTOS.detalle_preciarios");
            DropForeignKey("SCH_INGENIERIA.gastos_directos_certificado", "ColaboradorId", "SCH_RRHH.colaboradores");
            DropForeignKey("SCH_INGENIERIA.gastos_directos_certificado", "CertificadoIngenieriaProyectoId", "SCH_INGENIERIA.certificados_ingenieria");
            DropForeignKey("SCH_INGENIERIA.certificados_ingenieria", "ProyectoId", "SCH_PROYECTOS.proyectos");
            DropForeignKey("SCH_INGENIERIA.certificados_ingenieria", "OrdenServicioId", "SCH_PROYECTOS.ordenes_servicio");
            DropForeignKey("SCH_INGENIERIA.certificados_ingenieria", "GrupoCertificadoIngenieriaId", "SCH_INGENIERIA.grupo_certificado_ingenieria");
            DropForeignKey("SCH_INGENIERIA.grupo_certificado_ingenieria", "contrato_id", "SCH_PROYECTOS.contratos");
            DropIndex("SCH_INGENIERIA.gastos_directos_certificado", new[] { "UnidadId" });
            DropIndex("SCH_INGENIERIA.gastos_directos_certificado", new[] { "RubroId" });
            DropIndex("SCH_INGENIERIA.gastos_directos_certificado", new[] { "ColaboradorId" });
            DropIndex("SCH_INGENIERIA.gastos_directos_certificado", new[] { "CertificadoIngenieriaProyectoId" });
            DropIndex("SCH_INGENIERIA.grupo_certificado_ingenieria", new[] { "contrato_id" });
            DropIndex("SCH_INGENIERIA.certificados_ingenieria", new[] { "OrdenServicioId" });
            DropIndex("SCH_INGENIERIA.certificados_ingenieria", new[] { "ProyectoId" });
            DropIndex("SCH_INGENIERIA.certificados_ingenieria", new[] { "GrupoCertificadoIngenieriaId" });
            DropTable("SCH_INGENIERIA.gastos_directos_certificado",
                removedAnnotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_GastoDirectoCertificado_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                });
            DropTable("SCH_INGENIERIA.grupo_certificado_ingenieria",
                removedAnnotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_GrupoCertificadoIngenieria_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                });
            DropTable("SCH_INGENIERIA.certificados_ingenieria",
                removedAnnotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_CertificadoIngenieriaProyecto_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                });
        }
    }
}

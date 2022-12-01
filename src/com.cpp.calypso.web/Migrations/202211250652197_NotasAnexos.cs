namespace com.cpp.calypso.web.Migrations
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity.Infrastructure.Annotations;
    using System.Data.Entity.Migrations;
    
    public partial class NotasAnexos : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "SCH_MNA.katz",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        PacienteId = c.Int(nullable: false),
                        Bano = c.Boolean(nullable: false),
                        Vestido = c.Boolean(nullable: false),
                        Sanitario = c.Boolean(nullable: false),
                        Transferencias = c.Boolean(nullable: false),
                        Continencia = c.Boolean(nullable: false),
                        Alimentacion = c.Boolean(nullable: false),
                        Calificacion = c.String(),
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
                    { "DynamicFilter_Katz_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                })
                .PrimaryKey(t => t.id)
                .ForeignKey("SCH_MNA.paciente", t => t.PacienteId)
                .Index(t => t.PacienteId);
            
            CreateTable(
                "SCH_MNA.paciente",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        Identificacion = c.String(),
                        NombresApellidos = c.String(),
                        Peso = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Edad = c.Decimal(nullable: false, precision: 18, scale: 2),
                        SexoId = c.Int(nullable: false),
                        Talla = c.Decimal(nullable: false, precision: 18, scale: 2),
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
                    { "DynamicFilter_Paciente_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                })
                .PrimaryKey(t => t.id)
                .ForeignKey("SCH_CATALOGOS.catalogos", t => t.SexoId)
                .Index(t => t.SexoId);
            
            CreateTable(
                "SCH_MNA.mna",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        PacienteId = c.Int(nullable: false),
                        Fecha = c.DateTime(nullable: false),
                        PerdidaApetitoId = c.Int(nullable: false),
                        PerdidaPesoId = c.Int(nullable: false),
                        MovilidadId = c.Int(nullable: false),
                        EnfermedadAgudaId = c.Int(nullable: false),
                        ProblemasNeuroId = c.Int(nullable: false),
                        IndiceMasaId = c.Int(nullable: false),
                        ViveDomicilioId = c.Int(nullable: false),
                        MedicamentoDiaId = c.Int(nullable: false),
                        UlceraLesionId = c.Int(nullable: false),
                        ComidaDiariaId = c.Int(nullable: false),
                        ConsumoPersonaId = c.Int(nullable: false),
                        ConsumeLacteos = c.Boolean(nullable: false),
                        ConsumeLegumbres = c.Boolean(nullable: false),
                        ConsumeCarne = c.Boolean(nullable: false),
                        ConsumoFrutasVerdurasId = c.Int(nullable: false),
                        NumeroVasosAguaId = c.Int(nullable: false),
                        ModoAlimentarseId = c.Int(nullable: false),
                        ConsideracionEnfermoId = c.Int(nullable: false),
                        EstadoSaludId = c.Int(nullable: false),
                        CircunferenciaBraquialId = c.Int(nullable: false),
                        CircunferenciaPiernaId = c.Int(nullable: false),
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
                    { "DynamicFilter_MNA_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                })
                .PrimaryKey(t => t.id)
                .ForeignKey("SCH_CATALOGOS.catalogos", t => t.CircunferenciaBraquialId)
                .ForeignKey("SCH_CATALOGOS.catalogos", t => t.CircunferenciaPiernaId)
                .ForeignKey("SCH_CATALOGOS.catalogos", t => t.ComidaDiariaId)
                .ForeignKey("SCH_CATALOGOS.catalogos", t => t.ConsideracionEnfermoId)
                .ForeignKey("SCH_CATALOGOS.catalogos", t => t.ConsumoFrutasVerdurasId)
                .ForeignKey("SCH_CATALOGOS.catalogos", t => t.ConsumoPersonaId)
                .ForeignKey("SCH_CATALOGOS.catalogos", t => t.EnfermedadAgudaId)
                .ForeignKey("SCH_CATALOGOS.catalogos", t => t.EstadoSaludId)
                .ForeignKey("SCH_CATALOGOS.catalogos", t => t.IndiceMasaId)
                .ForeignKey("SCH_CATALOGOS.catalogos", t => t.MedicamentoDiaId)
                .ForeignKey("SCH_CATALOGOS.catalogos", t => t.ModoAlimentarseId)
                .ForeignKey("SCH_CATALOGOS.catalogos", t => t.MovilidadId)
                .ForeignKey("SCH_CATALOGOS.catalogos", t => t.NumeroVasosAguaId)
                .ForeignKey("SCH_MNA.paciente", t => t.PacienteId)
                .ForeignKey("SCH_CATALOGOS.catalogos", t => t.PerdidaApetitoId)
                .ForeignKey("SCH_CATALOGOS.catalogos", t => t.PerdidaPesoId)
                .ForeignKey("SCH_CATALOGOS.catalogos", t => t.ProblemasNeuroId)
                .ForeignKey("SCH_CATALOGOS.catalogos", t => t.UlceraLesionId)
                .ForeignKey("SCH_CATALOGOS.catalogos", t => t.ViveDomicilioId)
                .Index(t => t.PacienteId)
                .Index(t => t.PerdidaApetitoId)
                .Index(t => t.PerdidaPesoId)
                .Index(t => t.MovilidadId)
                .Index(t => t.EnfermedadAgudaId)
                .Index(t => t.ProblemasNeuroId)
                .Index(t => t.IndiceMasaId)
                .Index(t => t.ViveDomicilioId)
                .Index(t => t.MedicamentoDiaId)
                .Index(t => t.UlceraLesionId)
                .Index(t => t.ComidaDiariaId)
                .Index(t => t.ConsumoPersonaId)
                .Index(t => t.ConsumoFrutasVerdurasId)
                .Index(t => t.NumeroVasosAguaId)
                .Index(t => t.ModoAlimentarseId)
                .Index(t => t.ConsideracionEnfermoId)
                .Index(t => t.EstadoSaludId)
                .Index(t => t.CircunferenciaBraquialId)
                .Index(t => t.CircunferenciaPiernaId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("SCH_MNA.mna", "ViveDomicilioId", "SCH_CATALOGOS.catalogos");
            DropForeignKey("SCH_MNA.mna", "UlceraLesionId", "SCH_CATALOGOS.catalogos");
            DropForeignKey("SCH_MNA.mna", "ProblemasNeuroId", "SCH_CATALOGOS.catalogos");
            DropForeignKey("SCH_MNA.mna", "PerdidaPesoId", "SCH_CATALOGOS.catalogos");
            DropForeignKey("SCH_MNA.mna", "PerdidaApetitoId", "SCH_CATALOGOS.catalogos");
            DropForeignKey("SCH_MNA.mna", "PacienteId", "SCH_MNA.paciente");
            DropForeignKey("SCH_MNA.mna", "NumeroVasosAguaId", "SCH_CATALOGOS.catalogos");
            DropForeignKey("SCH_MNA.mna", "MovilidadId", "SCH_CATALOGOS.catalogos");
            DropForeignKey("SCH_MNA.mna", "ModoAlimentarseId", "SCH_CATALOGOS.catalogos");
            DropForeignKey("SCH_MNA.mna", "MedicamentoDiaId", "SCH_CATALOGOS.catalogos");
            DropForeignKey("SCH_MNA.mna", "IndiceMasaId", "SCH_CATALOGOS.catalogos");
            DropForeignKey("SCH_MNA.mna", "EstadoSaludId", "SCH_CATALOGOS.catalogos");
            DropForeignKey("SCH_MNA.mna", "EnfermedadAgudaId", "SCH_CATALOGOS.catalogos");
            DropForeignKey("SCH_MNA.mna", "ConsumoPersonaId", "SCH_CATALOGOS.catalogos");
            DropForeignKey("SCH_MNA.mna", "ConsumoFrutasVerdurasId", "SCH_CATALOGOS.catalogos");
            DropForeignKey("SCH_MNA.mna", "ConsideracionEnfermoId", "SCH_CATALOGOS.catalogos");
            DropForeignKey("SCH_MNA.mna", "ComidaDiariaId", "SCH_CATALOGOS.catalogos");
            DropForeignKey("SCH_MNA.mna", "CircunferenciaPiernaId", "SCH_CATALOGOS.catalogos");
            DropForeignKey("SCH_MNA.mna", "CircunferenciaBraquialId", "SCH_CATALOGOS.catalogos");
            DropForeignKey("SCH_MNA.katz", "PacienteId", "SCH_MNA.paciente");
            DropForeignKey("SCH_MNA.paciente", "SexoId", "SCH_CATALOGOS.catalogos");
            DropIndex("SCH_MNA.mna", new[] { "CircunferenciaPiernaId" });
            DropIndex("SCH_MNA.mna", new[] { "CircunferenciaBraquialId" });
            DropIndex("SCH_MNA.mna", new[] { "EstadoSaludId" });
            DropIndex("SCH_MNA.mna", new[] { "ConsideracionEnfermoId" });
            DropIndex("SCH_MNA.mna", new[] { "ModoAlimentarseId" });
            DropIndex("SCH_MNA.mna", new[] { "NumeroVasosAguaId" });
            DropIndex("SCH_MNA.mna", new[] { "ConsumoFrutasVerdurasId" });
            DropIndex("SCH_MNA.mna", new[] { "ConsumoPersonaId" });
            DropIndex("SCH_MNA.mna", new[] { "ComidaDiariaId" });
            DropIndex("SCH_MNA.mna", new[] { "UlceraLesionId" });
            DropIndex("SCH_MNA.mna", new[] { "MedicamentoDiaId" });
            DropIndex("SCH_MNA.mna", new[] { "ViveDomicilioId" });
            DropIndex("SCH_MNA.mna", new[] { "IndiceMasaId" });
            DropIndex("SCH_MNA.mna", new[] { "ProblemasNeuroId" });
            DropIndex("SCH_MNA.mna", new[] { "EnfermedadAgudaId" });
            DropIndex("SCH_MNA.mna", new[] { "MovilidadId" });
            DropIndex("SCH_MNA.mna", new[] { "PerdidaPesoId" });
            DropIndex("SCH_MNA.mna", new[] { "PerdidaApetitoId" });
            DropIndex("SCH_MNA.mna", new[] { "PacienteId" });
            DropIndex("SCH_MNA.paciente", new[] { "SexoId" });
            DropIndex("SCH_MNA.katz", new[] { "PacienteId" });
            DropTable("SCH_MNA.mna",
                removedAnnotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_MNA_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                });
            DropTable("SCH_MNA.paciente",
                removedAnnotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_Paciente_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                });
            DropTable("SCH_MNA.katz",
                removedAnnotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_Katz_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                });
        }
    }
}

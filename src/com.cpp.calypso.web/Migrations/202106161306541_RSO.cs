namespace com.cpp.calypso.web.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RSO : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "SCH_PROYECTOS.rso_cabeceras",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ProyectoId = c.Int(nullable: false),
                        codigo_rdo = c.String(nullable: false),
                        fecha_rdo = c.DateTime(nullable: false),
                        fecha_envio = c.DateTime(),
                        version = c.String(nullable: false, maxLength: 10),
                        es_definitivo = c.Boolean(nullable: false),
                        observacion = c.String(),
                        vigente = c.Boolean(nullable: false),
                        estado = c.Boolean(nullable: false),
                        emitido = c.Boolean(nullable: false),
                        avance_real_acumulado = c.Decimal(nullable: false, precision: 18, scale: 6),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("SCH_PROYECTOS.proyectos", t => t.ProyectoId)
                .Index(t => t.ProyectoId);
            
            CreateTable(
                "SCH_PROYECTOS.rso_detalles_eac",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UM = c.String(),
                        WbsId = c.Int(nullable: false),
                        RsoCabeceraId = c.Int(nullable: false),
                        ComputoId = c.Int(nullable: false),
                        ItemId = c.Int(nullable: false),
                        codigo_preciario = c.String(nullable: false),
                        nombre_actividad = c.String(nullable: false),
                        porcentaje_presupuesto_total = c.Decimal(nullable: false, precision: 24, scale: 20),
                        porcentaje_costo_eac_total = c.Decimal(nullable: false, precision: 24, scale: 20),
                        presupuesto_total = c.Decimal(nullable: false, precision: 38, scale: 20),
                        cantidad_planificada = c.Decimal(nullable: false, precision: 38, scale: 20),
                        cantidad_eac = c.Decimal(nullable: false, precision: 38, scale: 20),
                        precio_unitario = c.Decimal(nullable: false, precision: 18, scale: 2),
                        costo_presupuesto = c.Decimal(nullable: false, precision: 38, scale: 20),
                        costo_eac = c.Decimal(nullable: false, precision: 38, scale: 20),
                        ac_anterior = c.Decimal(nullable: false, precision: 38, scale: 20),
                        ac_diario = c.Decimal(nullable: false, precision: 38, scale: 20),
                        ac_actual = c.Decimal(nullable: false, precision: 38, scale: 20),
                        ev_anterior = c.Decimal(nullable: false, precision: 38, scale: 20),
                        ev_diario = c.Decimal(nullable: false, precision: 38, scale: 20),
                        ev_actual = c.Decimal(nullable: false, precision: 38, scale: 20),
                        ern_value = c.Decimal(nullable: false, precision: 38, scale: 20),
                        pv_costo_planificado = c.Decimal(nullable: false, precision: 38, scale: 20),
                        fecha_inicio_prevista = c.DateTime(),
                        fecha_fin_prevista = c.DateTime(),
                        fecha_inicio_real = c.DateTime(),
                        fecha_fin_real = c.DateTime(),
                        cantidad_anterior = c.Decimal(nullable: false, precision: 38, scale: 20),
                        cantidad_diaria = c.Decimal(nullable: false, precision: 38, scale: 20),
                        cantidad_acumulada = c.Decimal(nullable: false, precision: 38, scale: 20),
                        porcentaje_avance_anterior = c.Decimal(nullable: false, precision: 24, scale: 20),
                        porcentaje_avance_diario = c.Decimal(nullable: false, precision: 24, scale: 20),
                        porcentaje_avance_actual_acumulado = c.Decimal(nullable: false, precision: 24, scale: 20),
                        porcentaje_avance_previsto_acumulado = c.Decimal(nullable: false, precision: 24, scale: 20),
                        vigente = c.Boolean(nullable: false),
                        ganancia = c.Decimal(nullable: false, precision: 20, scale: 4),
                        PendienteAprobacion = c.Boolean(nullable: false),
                        codigo_especialidad = c.String(),
                        codigo_grupo = c.String(),
                        es_temporal = c.Boolean(nullable: false),
                        costo_budget_version_anterior = c.Decimal(nullable: false, precision: 38, scale: 20),
                        ev_actual_version_anterior = c.Decimal(nullable: false, precision: 38, scale: 20),
                        id_rubro = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("SCH_PROYECTOS.computos", t => t.ComputoId)
                .ForeignKey("SCH_PROYECTOS.items", t => t.ItemId)
                .ForeignKey("SCH_PROYECTOS.rso_cabeceras", t => t.RsoCabeceraId)
                .Index(t => t.RsoCabeceraId)
                .Index(t => t.ComputoId)
                .Index(t => t.ItemId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("SCH_PROYECTOS.rso_detalles_eac", "RsoCabeceraId", "SCH_PROYECTOS.rso_cabeceras");
            DropForeignKey("SCH_PROYECTOS.rso_detalles_eac", "ItemId", "SCH_PROYECTOS.items");
            DropForeignKey("SCH_PROYECTOS.rso_detalles_eac", "ComputoId", "SCH_PROYECTOS.computos");
            DropForeignKey("SCH_PROYECTOS.rso_cabeceras", "ProyectoId", "SCH_PROYECTOS.proyectos");
            DropIndex("SCH_PROYECTOS.rso_detalles_eac", new[] { "ItemId" });
            DropIndex("SCH_PROYECTOS.rso_detalles_eac", new[] { "ComputoId" });
            DropIndex("SCH_PROYECTOS.rso_detalles_eac", new[] { "RsoCabeceraId" });
            DropIndex("SCH_PROYECTOS.rso_cabeceras", new[] { "ProyectoId" });
            DropTable("SCH_PROYECTOS.rso_detalles_eac");
            DropTable("SCH_PROYECTOS.rso_cabeceras");
        }
    }
}

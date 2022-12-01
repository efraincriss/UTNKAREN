namespace com.cpp.calypso.web.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ActualizacionCamposAvancePorcentajeProyecto1 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("SCH_INGENIERIA.avances_porcentaje_proyecto", "avance_previsto_anterior_IB", c => c.Decimal(nullable: false, precision: 18, scale: 4));
            AlterColumn("SCH_INGENIERIA.avances_porcentaje_proyecto", "avance_real_anterior_IB", c => c.Decimal(nullable: false, precision: 18, scale: 4));
            AlterColumn("SCH_INGENIERIA.avances_porcentaje_proyecto", "avance_previsto_actual_IB", c => c.Decimal(nullable: false, precision: 18, scale: 4));
            AlterColumn("SCH_INGENIERIA.avances_porcentaje_proyecto", "avance_real_actual_IB", c => c.Decimal(nullable: false, precision: 18, scale: 4));
            AlterColumn("SCH_INGENIERIA.avances_porcentaje_proyecto", "avance_previsto_anterior_ID", c => c.Decimal(nullable: false, precision: 18, scale: 4));
            AlterColumn("SCH_INGENIERIA.avances_porcentaje_proyecto", "avance_real_anterior_ID", c => c.Decimal(nullable: false, precision: 18, scale: 4));
            AlterColumn("SCH_INGENIERIA.avances_porcentaje_proyecto", "avance_previsto_actual_ID", c => c.Decimal(nullable: false, precision: 18, scale: 4));
            AlterColumn("SCH_INGENIERIA.avances_porcentaje_proyecto", "avance_real_actual_ID", c => c.Decimal(nullable: false, precision: 18, scale: 4));
        }
        
        public override void Down()
        {
            AlterColumn("SCH_INGENIERIA.avances_porcentaje_proyecto", "avance_real_actual_ID", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AlterColumn("SCH_INGENIERIA.avances_porcentaje_proyecto", "avance_previsto_actual_ID", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AlterColumn("SCH_INGENIERIA.avances_porcentaje_proyecto", "avance_real_anterior_ID", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AlterColumn("SCH_INGENIERIA.avances_porcentaje_proyecto", "avance_previsto_anterior_ID", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AlterColumn("SCH_INGENIERIA.avances_porcentaje_proyecto", "avance_real_actual_IB", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AlterColumn("SCH_INGENIERIA.avances_porcentaje_proyecto", "avance_previsto_actual_IB", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AlterColumn("SCH_INGENIERIA.avances_porcentaje_proyecto", "avance_real_anterior_IB", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AlterColumn("SCH_INGENIERIA.avances_porcentaje_proyecto", "avance_previsto_anterior_IB", c => c.Decimal(nullable: false, precision: 18, scale: 2));
        }
    }
}

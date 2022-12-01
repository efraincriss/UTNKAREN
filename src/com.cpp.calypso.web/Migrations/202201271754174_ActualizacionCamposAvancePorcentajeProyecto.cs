namespace com.cpp.calypso.web.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ActualizacionCamposAvancePorcentajeProyecto : DbMigration
    {
        public override void Up()
        {
            AddColumn("SCH_INGENIERIA.avances_porcentaje_proyecto", "avance_previsto_anterior_IB", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AddColumn("SCH_INGENIERIA.avances_porcentaje_proyecto", "avance_real_anterior_IB", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AddColumn("SCH_INGENIERIA.avances_porcentaje_proyecto", "avance_previsto_actual_IB", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AddColumn("SCH_INGENIERIA.avances_porcentaje_proyecto", "avance_real_actual_IB", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AddColumn("SCH_INGENIERIA.avances_porcentaje_proyecto", "avance_previsto_anterior_ID", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AddColumn("SCH_INGENIERIA.avances_porcentaje_proyecto", "avance_real_anterior_ID", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AddColumn("SCH_INGENIERIA.avances_porcentaje_proyecto", "avance_previsto_actual_ID", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AddColumn("SCH_INGENIERIA.avances_porcentaje_proyecto", "avance_real_actual_ID", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            DropColumn("SCH_INGENIERIA.avances_porcentaje_proyecto", "avance_previsto_anterior");
            DropColumn("SCH_INGENIERIA.avances_porcentaje_proyecto", "avance_real_anterior");
            DropColumn("SCH_INGENIERIA.avances_porcentaje_proyecto", "avance_previsto_actual");
            DropColumn("SCH_INGENIERIA.avances_porcentaje_proyecto", "avance_real_actual");
        }
        
        public override void Down()
        {
            AddColumn("SCH_INGENIERIA.avances_porcentaje_proyecto", "avance_real_actual", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AddColumn("SCH_INGENIERIA.avances_porcentaje_proyecto", "avance_previsto_actual", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AddColumn("SCH_INGENIERIA.avances_porcentaje_proyecto", "avance_real_anterior", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AddColumn("SCH_INGENIERIA.avances_porcentaje_proyecto", "avance_previsto_anterior", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            DropColumn("SCH_INGENIERIA.avances_porcentaje_proyecto", "avance_real_actual_ID");
            DropColumn("SCH_INGENIERIA.avances_porcentaje_proyecto", "avance_previsto_actual_ID");
            DropColumn("SCH_INGENIERIA.avances_porcentaje_proyecto", "avance_real_anterior_ID");
            DropColumn("SCH_INGENIERIA.avances_porcentaje_proyecto", "avance_previsto_anterior_ID");
            DropColumn("SCH_INGENIERIA.avances_porcentaje_proyecto", "avance_real_actual_IB");
            DropColumn("SCH_INGENIERIA.avances_porcentaje_proyecto", "avance_previsto_actual_IB");
            DropColumn("SCH_INGENIERIA.avances_porcentaje_proyecto", "avance_real_anterior_IB");
            DropColumn("SCH_INGENIERIA.avances_porcentaje_proyecto", "avance_previsto_anterior_IB");
        }
    }
}

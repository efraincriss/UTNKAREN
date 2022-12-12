namespace com.cpp.calypso.web.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Oacientes : DbMigration
    {
        public override void Up()
        {
            AddColumn("SCH_MNA.paciente", "CentroId", c => c.Int(nullable: false));
            AddColumn("SCH_MNA.paciente", "GrupoEdad", c => c.String());
            AddColumn("SCH_MNA.paciente", "NivelEducativo", c => c.String());
            AddColumn("SCH_MNA.paciente", "EstadoCivil", c => c.String());
            AddColumn("SCH_MNA.paciente", "ViveSolo", c => c.String());
            AddColumn("SCH_MNA.paciente", "ConsumeAlcohol", c => c.String());
            AddColumn("SCH_MNA.paciente", "ConsumeCigarillo", c => c.String());
            AddColumn("SCH_MNA.paciente", "AutoReporteSalud", c => c.String());
            AddColumn("SCH_MNA.paciente", "HipertencionArterial", c => c.String());
            AddColumn("SCH_MNA.paciente", "InsuficienciaArterial", c => c.String());
            AddColumn("SCH_MNA.paciente", "InsuficienciaCardicaCongestiva", c => c.String());
            AddColumn("SCH_MNA.paciente", "Epoc", c => c.String());
            AddColumn("SCH_MNA.paciente", "EnfermedadCerebroVascular", c => c.String());
            CreateIndex("SCH_MNA.paciente", "CentroId");
            AddForeignKey("SCH_MNA.paciente", "CentroId", "SCH_CATALOGOS.catalogos", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("SCH_MNA.paciente", "CentroId", "SCH_CATALOGOS.catalogos");
            DropIndex("SCH_MNA.paciente", new[] { "CentroId" });
            DropColumn("SCH_MNA.paciente", "EnfermedadCerebroVascular");
            DropColumn("SCH_MNA.paciente", "Epoc");
            DropColumn("SCH_MNA.paciente", "InsuficienciaCardicaCongestiva");
            DropColumn("SCH_MNA.paciente", "InsuficienciaArterial");
            DropColumn("SCH_MNA.paciente", "HipertencionArterial");
            DropColumn("SCH_MNA.paciente", "AutoReporteSalud");
            DropColumn("SCH_MNA.paciente", "ConsumeCigarillo");
            DropColumn("SCH_MNA.paciente", "ConsumeAlcohol");
            DropColumn("SCH_MNA.paciente", "ViveSolo");
            DropColumn("SCH_MNA.paciente", "EstadoCivil");
            DropColumn("SCH_MNA.paciente", "NivelEducativo");
            DropColumn("SCH_MNA.paciente", "GrupoEdad");
            DropColumn("SCH_MNA.paciente", "CentroId");
        }
    }
}

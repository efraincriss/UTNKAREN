namespace com.cpp.calypso.web.Migrations
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity.Infrastructure.Annotations;
    using System.Data.Entity.Migrations;
    
    public partial class NuevaColumnaModuloIdParametrosSistema : DbMigration
    {
        public override void Up()
        {
            
            AddColumn("SCH_USUARIOS.parametros", "ModuloId", c => c.Int());
            CreateIndex("SCH_USUARIOS.parametros", "ModuloId");
            AddForeignKey("SCH_USUARIOS.parametros", "ModuloId", "SCH_USUARIOS.modulos", "Id");
        }
        
        public override void Down()
        {
         
            DropIndex("SCH_USUARIOS.parametros", new[] { "ModuloId" });
            DropColumn("SCH_USUARIOS.parametros", "ModuloId");
       
        }
    }
}

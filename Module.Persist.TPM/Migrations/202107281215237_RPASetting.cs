namespace Module.Persist.TPM.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RPASetting : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "Jupiter.RPASetting",
                c => new
                {
                    Id = c.Guid(nullable: false, identity: true),
                    Json = c.String(nullable: false),
                    Type = c.Int(nullable: false)
                })
                .PrimaryKey(t => t.Id);                
        }
        
        public override void Down()
        {        
            DropTable("Jupiter.RPASetting");
        }
    }
}

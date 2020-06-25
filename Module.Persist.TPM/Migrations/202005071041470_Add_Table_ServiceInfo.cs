namespace Module.Persist.TPM.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Add_Table_ServiceInfo : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ServiceInfo",
                c => new
                    {
                        Id = c.Guid(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 255),
                        Value = c.String(),
                        Description = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.Name, unique: true);
            
        }
        
        public override void Down()
        {
            DropIndex("dbo.ServiceInfo", new[] { "Name" });
            DropTable("dbo.ServiceInfo");
        }
    }
}

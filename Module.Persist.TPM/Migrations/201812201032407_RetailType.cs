namespace Module.Persist.TPM.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RetailType : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.RetailType",
                c => new
                    {
                        Id = c.Guid(nullable: false, identity: true),
                        Disabled = c.Boolean(nullable: false),
                        DeletedDate = c.DateTimeOffset(precision: 7),
                        Name = c.String(nullable: false, maxLength: 255),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.Name, unique: true);
        }
        
        public override void Down()
        {
            DropIndex("dbo.RetailType", new[] { "Name" });
            DropTable("dbo.RetailType");
        }
    }
}

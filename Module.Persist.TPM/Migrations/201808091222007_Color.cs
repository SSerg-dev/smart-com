namespace Module.Persist.TPM.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Color : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Color",
                c => new
                    {
                        Id = c.Guid(nullable: false, identity: true),
                        PropertyName = c.String(nullable: false, maxLength: 255),
                        PropertyValue = c.String(nullable: false, maxLength: 255),
                        ColorValue = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropIndex("dbo.Color", new[] { "Id" });
            DropTable("dbo.Color");
        }
    }
}

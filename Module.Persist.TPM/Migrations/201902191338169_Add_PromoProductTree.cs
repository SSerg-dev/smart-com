namespace Module.Persist.TPM.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Add_PromoProductTree : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.PromoProductTree",
                c => new
                    {
                        Id = c.Guid(nullable: false, identity: true),
                        Disabled = c.Boolean(nullable: false),
                        DeletedDate = c.DateTimeOffset(precision: 7),
                        PromoId = c.Guid(nullable: false),
                        ProductTreeObjectId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => new { t.PromoId, t.ProductTreeObjectId, t.Disabled, t.DeletedDate }, unique: true, name: "Unique_PromoProductTree");
            
        }
        
        public override void Down()
        {
            DropIndex("dbo.PromoProductTree", "Unique_PromoProductTree");
            DropTable("dbo.PromoProductTree");
        }
    }
}

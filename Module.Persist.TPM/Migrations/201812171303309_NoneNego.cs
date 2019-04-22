namespace Module.Persist.TPM.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class NoneNego : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.NoneNego",
                c => new
                    {
                        Id = c.Guid(nullable: false, identity: true),
                        Disabled = c.Boolean(nullable: false),
                        DeletedDate = c.DateTimeOffset(precision: 7),
                        Discount = c.Int(),
                        FromDate = c.DateTimeOffset(precision: 7),
                        ToDate = c.DateTimeOffset(precision: 7),
                        CreateDate = c.DateTimeOffset(precision: 7),
                        ProductId = c.Guid(),
                        MechanicId = c.Guid(),
                        MechanicTypeId = c.Guid(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Mechanic", t => t.MechanicId)
                .ForeignKey("dbo.MechanicType", t => t.MechanicTypeId)
                .ForeignKey("dbo.Product", t => t.ProductId)
                .Index(t => t.ProductId)
                .Index(t => t.MechanicId)
                .Index(t => t.MechanicTypeId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.NoneNego", "ProductId", "dbo.Product");
            DropForeignKey("dbo.NoneNego", "MechanicTypeId", "dbo.MechanicType");
            DropForeignKey("dbo.NoneNego", "MechanicId", "dbo.Mechanic");
            DropIndex("dbo.NoneNego", new[] { "MechanicTypeId" });
            DropIndex("dbo.NoneNego", new[] { "MechanicId" });
            DropIndex("dbo.NoneNego", new[] { "ProductId" });
            DropTable("dbo.NoneNego");
        }
    }
}

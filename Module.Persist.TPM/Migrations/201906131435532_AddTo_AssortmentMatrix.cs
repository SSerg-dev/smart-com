namespace Module.Persist.TPM.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddTo_AssortmentMatrix : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AssortmentMatrix", "StartDate", c => c.DateTimeOffset(precision: 7));
            AddColumn("dbo.AssortmentMatrix", "EndDate", c => c.DateTimeOffset(precision: 7));
            AddColumn("dbo.AssortmentMatrix", "ProductId", c => c.Guid(nullable: false));
            CreateIndex("dbo.AssortmentMatrix", "ProductId");
            AddForeignKey("dbo.AssortmentMatrix", "ProductId", "dbo.Product", "Id");
            DropColumn("dbo.AssortmentMatrix", "ZREP_AssortMatrix");
        }
        
        public override void Down()
        {
            AddColumn("dbo.AssortmentMatrix", "ZREP_AssortMatrix", c => c.String(nullable: false, maxLength: 255));
            DropForeignKey("dbo.AssortmentMatrix", "ProductId", "dbo.Product");
            DropIndex("dbo.AssortmentMatrix", new[] { "ProductId" });
            DropColumn("dbo.AssortmentMatrix", "ProductId");
            DropColumn("dbo.AssortmentMatrix", "EndDate");
            DropColumn("dbo.AssortmentMatrix", "StartDate");
        }
    }
}

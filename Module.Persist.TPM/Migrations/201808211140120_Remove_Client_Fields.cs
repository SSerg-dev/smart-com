namespace Module.Persist.TPM.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Remove_Client_Fields : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Client", "DistributorId", "dbo.Distributor");
            DropForeignKey("dbo.Client", "RegionId", "dbo.Region");
            DropForeignKey("dbo.Client", "StoreTypeId", "dbo.StoreType");
            DropIndex("dbo.Client", new[] { "RegionId" });
            DropIndex("dbo.Client", new[] { "DistributorId" });
            DropIndex("dbo.Client", new[] { "StoreTypeId" });
            DropColumn("dbo.Client", "RegionId");
            DropColumn("dbo.Client", "CommercialNetId");
            DropColumn("dbo.Client", "DistributorId");
            DropColumn("dbo.Client", "StoreTypeId");
            DropColumn("dbo.Client", "Store");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Client", "Store", c => c.String(maxLength: 400));
            AddColumn("dbo.Client", "StoreTypeId", c => c.Guid(nullable: false));
            AddColumn("dbo.Client", "DistributorId", c => c.Guid(nullable: false));
            AddColumn("dbo.Client", "CommercialNetId", c => c.Guid(nullable: false));
            AddColumn("dbo.Client", "RegionId", c => c.Guid(nullable: false));
            CreateIndex("dbo.Client", "StoreTypeId");
            CreateIndex("dbo.Client", "DistributorId");
            CreateIndex("dbo.Client", "RegionId");
            AddForeignKey("dbo.Client", "StoreTypeId", "dbo.StoreType", "Id");
            AddForeignKey("dbo.Client", "RegionId", "dbo.Region", "Id");
            AddForeignKey("dbo.Client", "DistributorId", "dbo.Distributor", "Id");
        }
    }
}

namespace Module.Persist.TPM.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class SimpleTree : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ClientTree",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ObjectId = c.Guid(nullable: false, identity: true),
                        parentId = c.Guid(nullable: false),
                        depth = c.Int(nullable: false),
                        Type = c.String(),
                        Name = c.String(),
                        StartDate = c.DateTime(nullable: false),
                        EndDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => new { t.ObjectId, t.EndDate }, unique: true, name: "CX_ObjDate");
            
            //AddColumn("dbo.PromoSales", "Number", c => c.Int());
            //AddColumn("dbo.Demand", "Number", c => c.Int());
            //AddColumn("dbo.Finance", "Number", c => c.Int());
        }
        
        public override void Down()
        {
            DropIndex("dbo.ClientTree", "CX_ObjDate");
            //DropColumn("dbo.Finance", "Number");
            //DropColumn("dbo.Demand", "Number");
            //DropColumn("dbo.PromoSales", "Number");
            DropTable("dbo.ClientTree");
        }
    }
}

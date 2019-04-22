namespace Module.Persist.TPM.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ProductTree : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ProductTree",
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
            
        }
        
        public override void Down()
        {
            DropIndex("dbo.ProductTree", "CX_ObjDate");
            DropTable("dbo.ProductTree");
        }
    }
}

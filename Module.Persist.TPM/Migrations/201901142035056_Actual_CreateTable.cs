namespace Module.Persist.TPM.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Actual_CreateTable : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Actual",
                c => new
                    {
                        Id = c.Guid(nullable: false, identity: true),
                        Disabled = c.Boolean(nullable: false),
                        DeletedDate = c.DateTimeOffset(precision: 7),
                        GRD = c.String(nullable: false, maxLength: 255),
                        DemandCode = c.String(nullable: false, maxLength: 255),
                        SalesOutCode = c.String(nullable: false, maxLength: 255),
                        SalesInCode = c.String(nullable: false, maxLength: 255),
                        Date = c.DateTimeOffset(nullable: false, precision: 7),
                        Actuals = c.Double(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Actual");
        }
    }
}

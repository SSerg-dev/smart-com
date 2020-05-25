namespace Module.Persist.TPM.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Add_CoefficientSI2SO : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.CoefficientSI2SO",
                c => new
                    {
                        Id = c.Guid(nullable: false, identity: true),
                        Disabled = c.Boolean(nullable: false),
                        DeletedDate = c.DateTimeOffset(precision: 7),
                        DemandCode = c.String(maxLength: 255),
                        MarsPeriod = c.String(maxLength: 255),
                        CoefficientValue = c.Double(nullable: false),
                        Lock = c.Boolean(),
                        BrandTechId = c.Guid(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.BrandTech", t => t.BrandTechId)
                .Index(t => new { t.DeletedDate, t.DemandCode, t.MarsPeriod, t.BrandTechId }, unique: true, name: "Unique_Coef");
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.CoefficientSI2SO", "BrandTechId", "dbo.BrandTech");
            DropIndex("dbo.CoefficientSI2SO", "Unique_Coef");
            DropTable("dbo.CoefficientSI2SO");
        }
    }
}

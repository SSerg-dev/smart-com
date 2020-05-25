namespace Module.Persist.TPM.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Remove_Coefficient_Fields : DbMigration
    {
        public override void Up()
        {
            DropIndex("dbo.CoefficientSI2SO", "Unique_Coef");
            CreateIndex("dbo.CoefficientSI2SO", new[] { "DeletedDate", "DemandCode", "BrandTechId" }, unique: true, name: "Unique_Coef");
            DropColumn("dbo.CoefficientSI2SO", "MarsDatePeriod");
            DropColumn("dbo.CoefficientSI2SO", "StartDate");
            DropColumn("dbo.CoefficientSI2SO", "EndDate");
        }
        
        public override void Down()
        {
            AddColumn("dbo.CoefficientSI2SO", "EndDate", c => c.DateTimeOffset(precision: 7));
            AddColumn("dbo.CoefficientSI2SO", "StartDate", c => c.DateTimeOffset(precision: 7));
            AddColumn("dbo.CoefficientSI2SO", "MarsDatePeriod", c => c.String(maxLength: 255));
            DropIndex("dbo.CoefficientSI2SO", "Unique_Coef");
            CreateIndex("dbo.CoefficientSI2SO", new[] { "DeletedDate", "DemandCode", "MarsDatePeriod", "BrandTechId" }, unique: true, name: "Unique_Coef");
        }
    }
}

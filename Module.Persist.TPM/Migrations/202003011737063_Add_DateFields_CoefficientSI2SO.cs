namespace Module.Persist.TPM.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Add_DateFields_CoefficientSI2SO : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.CoefficientSI2SO", "StartDate", c => c.DateTimeOffset(precision: 7));
            AddColumn("dbo.CoefficientSI2SO", "EndDate", c => c.DateTimeOffset(precision: 7));
        }
        
        public override void Down()
        {
            DropColumn("dbo.CoefficientSI2SO", "EndDate");
            DropColumn("dbo.CoefficientSI2SO", "StartDate");
        }
    }
}

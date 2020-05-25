namespace Module.Persist.TPM.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Edit_CoefficientSI2SO : DbMigration
    {
        public override void Up()
        {
            RenameColumn("dbo.CoefficientSI2SO", "MarsPeriod", "MarsDatePeriod");
            AlterColumn("dbo.CoefficientSI2SO", "CoefficientValue", c => c.Double());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.CoefficientSI2SO", "CoefficientValue", c => c.Double(nullable: false));
            RenameColumn("dbo.CoefficientSI2SO", "MarsDatePeriod", "MarsPeriod");
        }
    }
}

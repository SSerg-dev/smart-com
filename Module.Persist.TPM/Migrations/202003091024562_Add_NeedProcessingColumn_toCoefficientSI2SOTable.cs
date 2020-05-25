namespace Module.Persist.TPM.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Add_NeedProcessingColumn_toCoefficientSI2SOTable : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.CoefficientSI2SO", "NeedProcessing", c => c.Boolean(nullable: false, defaultValue: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.CoefficientSI2SO", "NeedProcessing");
        }
    }
}

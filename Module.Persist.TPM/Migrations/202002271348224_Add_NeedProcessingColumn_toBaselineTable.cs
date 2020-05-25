namespace Module.Persist.TPM.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Add_NeedProcessingColumn_toBaselineTable : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.BaseLine", "NeedProcessing", c => c.Boolean(nullable: false, defaultValue: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.BaseLine", "NeedProcessing");
        }
    }
}

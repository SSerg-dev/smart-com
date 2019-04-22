namespace Module.Persist.TPM.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class BaseLineAddLastModifiedDateField : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.BaseLine", "LastModifiedDate", c => c.DateTimeOffset(precision: 7));
        }
        
        public override void Down()
        {
            DropColumn("dbo.BaseLine", "LastModifiedDate");
        }
    }
}

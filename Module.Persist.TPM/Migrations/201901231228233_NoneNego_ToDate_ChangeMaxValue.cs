namespace Module.Persist.TPM.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class NoneNego_ToDate_ChangeMaxValue : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.NoneNego", "ToDate", c => 
                c.DateTimeOffset(precision: 7, defaultValue: DateTimeOffset.MaxValue));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.NoneNego", "ToDate", c => c.DateTimeOffset(precision: 7));
        }
    }
}

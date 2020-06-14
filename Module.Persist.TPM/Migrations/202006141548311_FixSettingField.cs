namespace Module.Persist.TPM.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class FixSettingField : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Setting", "Value", c => c.String());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Setting", "Value", c => c.String(maxLength: 256));
        }
    }
}

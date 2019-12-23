namespace Module.Persist.TPM.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Add_Technology_Tech_code : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Technology", "Tech_code", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Technology", "Tech_code");
        }
    }
}

namespace Module.Persist.TPM.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Add_Brand_Brand_code : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Brand", "Brand_code", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Brand", "Brand_code");
        }
    }
}

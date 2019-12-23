namespace Module.Persist.TPM.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Materials_Models_update : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Brand", "Segmen_code", c => c.String());
            DropColumn("dbo.Brand", "Brand_code");
            AddColumn("dbo.Brand", "Brand_code", c => c.String());
            DropColumn("dbo.Technology", "Tech_code");
            AddColumn("dbo.Technology", "Tech_code", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Brand", "Brand_code");
            AddColumn("dbo.Brand", "Brand_code", c => c.Int(nullable: false));
            DropColumn("dbo.Technology", "Tech_code");
            AddColumn("dbo.Technology", "Tech_code", c => c.Int(nullable: false));
            DropColumn("dbo.Brand", "Segmen_code");
        }
    }
}

namespace Module.Persist.TPM.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AssortmentMatrix_Number : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AssortmentMatrix", "Number", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.AssortmentMatrix", "Number");
        }
    }
}

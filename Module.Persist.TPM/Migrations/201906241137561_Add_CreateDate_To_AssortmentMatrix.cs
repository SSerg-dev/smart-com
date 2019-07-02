namespace Module.Persist.TPM.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Add_CreateDate_To_AssortmentMatrix : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AssortmentMatrix", "CreateDate", c => c.DateTimeOffset(precision: 7));
        }
        
        public override void Down()
        {
            DropColumn("dbo.AssortmentMatrix", "CreateDate");
        }
    }
}

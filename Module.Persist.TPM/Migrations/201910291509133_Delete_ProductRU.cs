namespace Module.Persist.TPM.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Delete_ProductRU : DbMigration
    {
        public override void Up()
        {
           
            DropColumn("dbo.Product", "ProductRU");
            
        }
        
        public override void Down()
        {
            AddColumn("dbo.Product", "ProductRU", c => c.String(maxLength: 255));
        }
    }
}

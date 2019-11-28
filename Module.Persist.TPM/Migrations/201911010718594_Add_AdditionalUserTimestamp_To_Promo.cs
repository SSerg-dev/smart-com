namespace Module.Persist.TPM.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Add_AdditionalUserTimestamp_To_Promo : DbMigration
    {
        public override void Up()
        {
            
             AddColumn("dbo.Promo", "AdditionalUserTimestamp", c => c.String(maxLength: 100));
           
        }
        
        public override void Down()
        {
           
            DropColumn("dbo.Promo", "AdditionalUserTimestamp");
        }
    }
}

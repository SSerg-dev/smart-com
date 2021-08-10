namespace Module.Persist.TPM.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RPASetting_Update_Column_Type : DbMigration
    {
        public override void Up()
        {   
            AddColumn("Jupiter.RPASetting", "Name", c => c.String(nullable: false));
            DropColumn("Jupiter.RPASetting", "Type");
        }
        
        public override void Down()
        {
            AddColumn("Jupiter.RPASetting", "Type", c => c.Int(nullable: false));
            DropColumn("Jupiter.RPASetting", "Name");
            DropTable("Jupiter.RPA");
        }
    }
}

namespace Module.Persist.TPM.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Update_RPA_Columns : DbMigration
    {
        public override void Up()
        {
            AlterColumn("Jupiter.RPA", "Constraint", c => c.String());
            AlterColumn("Jupiter.RPA", "Parametr", c => c.String());
            AlterColumn("Jupiter.RPA", "Status", c => c.String());
            AlterColumn("Jupiter.RPA", "FileURL", c => c.String());
            AlterColumn("Jupiter.RPA", "LogURL", c => c.String());
        }
        
        public override void Down()
        {
            AlterColumn("Jupiter.RPA", "LogURL", c => c.String(nullable: false));
            AlterColumn("Jupiter.RPA", "FileURL", c => c.String(nullable: false));
            AlterColumn("Jupiter.RPA", "Status", c => c.String(nullable: false));
            AlterColumn("Jupiter.RPA", "Parametr", c => c.String(nullable: false));
            AlterColumn("Jupiter.RPA", "Constraint", c => c.String(nullable: false));
        }
    }
}

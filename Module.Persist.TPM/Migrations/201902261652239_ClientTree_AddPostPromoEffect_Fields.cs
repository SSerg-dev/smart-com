namespace Module.Persist.TPM.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ClientTree_AddPostPromoEffect_Fields : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ClientTree", "PostPromoEffectW1", c => c.Double());
            AddColumn("dbo.ClientTree", "PostPromoEffectW2", c => c.Double());
        }
        
        public override void Down()
        {
            DropColumn("dbo.ClientTree", "PostPromoEffectW2");
            DropColumn("dbo.ClientTree", "PostPromoEffectW1");
        }
    }
}

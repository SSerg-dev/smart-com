namespace Module.Persist.TPM.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Promo_Add_DemainPlanig_Role_For_Recalculate : DbMigration
    {
        public override void Up()
        {
            Sql($@"
			        DECLARE @ItemId UNIQUEIDENTIFIER
			        SELECT  @ItemId = Id FROM Jupiter.[AccessPoint] where Resource = 'Promoes' AND Action = 'RecalculatePromo'
			        INSERT INTO Jupiter.AccessPointRole(AccessPointId, RoleId) SELECT @ItemId, Id FROM Jupiter.Role WHERE SystemName = 'DemandPlanning'
                ");
        }
        
        public override void Down()
        {
            Sql($@"
			        DECLARE @ItemId UNIQUEIDENTIFIER
			        SELECT  @ItemId = Id FROM Jupiter.[AccessPoint] where Resource = 'Promoes' AND Action = 'RecalculatePromo'
			        DELETE Jupiter.AccessPointRole WHERE AccessPointId=@ItemId AND RoleId =( SELECT Id FROM Jupiter.Role WHERE SystemName='DemandPlanning')
                ");
        }
    }
}

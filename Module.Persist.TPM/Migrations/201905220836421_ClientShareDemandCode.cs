namespace Module.Persist.TPM.Migrations {
    using System.Data.Entity.Migrations;

    public partial class ClientShareDemandCode : DbMigration {
        public override void Up() {
            Sql("DROP VIEW [ClientTreeSharesView]");
            Sql("CREATE VIEW [ClientTreeSharesView] AS " +
                "WITH CTE(Id, ObjectId, Name, parentId, IsBaseClient, LevelS, BOI, EndDate, StartDate, ResultNameStr, depth, LeafShare, DemandCode) AS " +
                "(SELECT Id, ObjectId, [Name], parentId, IsBaseClient, 0 AS LevelS, ObjectId as BOI, EndDate, StartDate, [Name] as ResultNameStr, depth, Share as LeafShare, DemandCode " +
                "FROM ClientTree WHERE(EndDate IS NULL OR EndDate > GETDATE()) UNION ALL " +
                "SELECT t1.Id, t2.ObjectId, t2.Name, t2.parentId, t2.IsBaseClient, t1.LevelS + 1, BOI, t2.EndDate, t2.StartDate, t2.Name + ' > ' + ResultNameStr, t2.depth, LeafShare, t1.DemandCode " +
                "FROM ClientTree t2 " +
                "JOIN CTE t1 ON t1.parentId = t2.ObjectId AND(t2.EndDate IS NULL OR t2.EndDate > GETDATE()) AND(t2.StartDate < GETDATE()) WHERE t2.ObjectId <> t2.parentId) " +
                "SELECT Id, BOI, ResultNameStr, LeafShare, DemandCode " +
                "FROM CTE " +
                "WHERE CAST(BOI AS  varchar(40)) + '_' + CAST(LevelS AS varchar) IN(SELECT CAST(BOI AS varchar(40)) + '_' + CAST(MAX(LevelS) AS varchar) FROM CTE GROUP BY BOI);");
        }

        public override void Down() {
            Sql("DROP VIEW [ClientTreeSharesView]");
            Sql("CREATE VIEW [ClientTreeSharesView] AS " +
                "WITH CTE(Id, ObjectId, Name, parentId, IsBaseClient, LevelS, BOI, EndDate, StartDate, ResultNameStr, depth, LeafShare) AS " +
                "(SELECT Id, ObjectId, [Name], parentId, IsBaseClient, 0 AS LevelS, ObjectId as BOI, EndDate, StartDate, [Name] as ResultNameStr, depth, Share as LeafShare " +
                "FROM ClientTree WHERE(EndDate IS NULL OR EndDate > GETDATE()) UNION ALL " +
                "SELECT t1.Id, t2.ObjectId, t2.Name, t2.parentId, t2.IsBaseClient, t1.LevelS + 1, BOI, t2.EndDate, t2.StartDate, t2.Name + ' > ' + ResultNameStr, t2.depth, LeafShare " +
                "FROM ClientTree t2 " +
                "JOIN CTE t1 ON t1.parentId = t2.ObjectId AND(t2.EndDate IS NULL OR t2.EndDate > GETDATE()) AND(t2.StartDate < GETDATE()) WHERE t2.ObjectId <> t2.parentId) " +
                "SELECT Id, BOI, ResultNameStr, LeafShare " +
                "FROM CTE " +
                "WHERE CAST(BOI AS  varchar(40)) + '_' + CAST(LevelS AS varchar) IN(SELECT CAST(BOI AS varchar(40)) + '_' + CAST(MAX(LevelS) AS varchar) FROM CTE GROUP BY BOI);");
        }
    }
}

namespace Module.Persist.TPM.Migrations {
    using System.Data.Entity.Migrations;
    public partial class ClientHierarchyView : DbMigration {
        public override void Up() {
            Sql("CREATE VIEW [ClientTreeHierarchyView] AS " +
	                "With RecursiveSearch (ObjectId, parentId, Hierarchy) AS ( " +
                    "Select ObjectId, parentId, CONVERT(varchar(255), '') " +
                    "FROM [ClientTree] AS FirtGeneration " +
                    "WHERE [Type] = 'root' and ((SYSDATETIME() between StartDate and EndDate) or EndDate is NULL)  " +
                    "union all " +
                    "select NextStep.ObjectId, NextStep.parentId, " +
                    "CAST(CASE WHEN Hierarchy = '' " +
                    "    THEN(CAST(NextStep.parentId AS VARCHAR(255))) " +
                    "    ELSE(Hierarchy + '.' + CAST(NextStep.parentId AS VARCHAR(255))) " +
                    "END AS VARCHAR(255)) " +
                    "FROM [ClientTree] AS NextStep " +
                    "INNER JOIN RecursiveSearch as bag on " +
                    "bag.ObjectId = NextStep.parentId " +
                    "where ( (SYSDATETIME() between NextStep.StartDate and NextStep.EndDate) or NextStep.EndDate is NULL) and [Type] <> 'root' " +
                    ") " +
                    "Select ObjectId as Id,  Hierarchy from RecursiveSearch "
                );
        }
        public override void Down() {
            Sql("DROP VIEW IF EXISTS ClientTreeHierarchyView;");
        }
    }
}

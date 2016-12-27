
-- 建立 Stored Procedures
-- =============================================
-- Author: Greene
-- Last Modified Date: 2015-12-04
-- Description: 取得該會員的麵包屑
-- =============================================
CREATE PROCEDURE [dbo].[usp_GetBreadcrumbs]
    @Id int
AS
BEGIN
    SET NOCOUNT ON;

    WITH cte_Parents AS
    (
        SELECT Id, ParentId, Account FROM dbo.Member WHERE Id = @Id
        UNION ALL
        SELECT Member.Id, Member.ParentId, Member.Account FROM dbo.Member
        INNER JOIN cte_Parents ON Member.Id = cte_Parents.ParentId
    )
    SELECT * FROM cte_Parents
    OPTION(MAXRECURSION 32767);
END;
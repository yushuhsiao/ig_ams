
-- =============================================
-- Description: 取得該會員的麵包屑
-- Update date: 2016-11-25
-- =============================================
CREATE PROCEDURE dbo.usp_GetBreadcrumbs
    @Id int
AS
SET NOCOUNT ON;

BEGIN TRY

    WITH cte_Parents AS
    (
        SELECT Id, ParentId, Account FROM dbo.Member WHERE Id = @Id
        UNION ALL
        SELECT Member.Id, Member.ParentId, Member.Account FROM dbo.Member
        INNER JOIN cte_Parents ON Member.Id = cte_Parents.ParentId
    )
    SELECT * FROM cte_Parents
    OPTION(MAXRECURSION 32767);

END TRY
BEGIN CATCH
    RETURN ERROR_NUMBER();
END CATCH;
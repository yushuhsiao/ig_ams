
-- =============================================
-- Description: 取得該會員所有的父會員
-- Update date: 2016-11-25
-- =============================================
CREATE PROCEDURE dbo.usp_GetAllParents
    @Id int
AS
SET NOCOUNT ON;

BEGIN TRY

    WITH cte_Parents AS
    (
        SELECT * FROM dbo.Member WHERE Id = @Id
        UNION ALL
        SELECT Member.* FROM dbo.Member
        INNER JOIN cte_Parents ON Member.Id = cte_Parents.ParentId
    )
    SELECT * FROM cte_Parents WHERE Id <> @Id
    OPTION(MAXRECURSION 32767);

END TRY
BEGIN CATCH
    RETURN ERROR_NUMBER();
END CATCH;
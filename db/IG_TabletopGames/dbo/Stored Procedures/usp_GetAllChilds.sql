
-- =============================================
-- Description: 取得該會員所有的子會員
-- Update date: 2016-11-25
-- =============================================
CREATE PROCEDURE dbo.usp_GetAllChilds
    @Id int
AS
SET NOCOUNT ON;

BEGIN TRY

    WITH cte_Childs AS
    ( 
        SELECT * FROM dbo.Member WHERE ParentId = @Id
        UNION ALL
        SELECT Member.* FROM dbo.Member
        INNER JOIN cte_Childs ON Member.ParentId = cte_Childs.Id
    )
    SELECT * FROM cte_Childs WHERE Role >= 0 AND Role <= 5
    OPTION(MAXRECURSION 32767);

END TRY
BEGIN CATCH
    RETURN ERROR_NUMBER();
END CATCH;

-- =============================================
-- Description: 取得該會員所有的子會員（只拿取部分欄位）
-- Update date: 2016-11-30
-- =============================================
CREATE PROCEDURE dbo.usp_GetAllChildPlayers
    @Id int
AS
SET NOCOUNT ON;

BEGIN TRY

    WITH cte_Childs AS
    ( 
        SELECT Id, Account, Role FROM dbo.Member WHERE ParentId = @Id
        UNION ALL
        SELECT Member.Id, Member.Account, Member.Role FROM dbo.Member
        INNER JOIN cte_Childs ON Member.ParentId = cte_Childs.Id
    )
    SELECT * FROM cte_Childs WHERE Role = 4
    OPTION(MAXRECURSION 32767);

END TRY
BEGIN CATCH
    RETURN ERROR_NUMBER();
END CATCH;
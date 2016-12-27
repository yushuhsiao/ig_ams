
-- 建立 Stored Procedures
-- =============================================
-- Author: Greene
-- Last Modified Date: 2015-12-04
-- Description: 取得該會員所有的子會員
-- =============================================
CREATE PROCEDURE [dbo].[usp_GetAllChilds]
    @Id int
AS
BEGIN
    SET NOCOUNT ON;

    WITH cte_Childs AS
    ( 
        SELECT * FROM dbo.Member WHERE ParentId = @Id
        UNION ALL
        SELECT Member.* FROM dbo.Member
        INNER JOIN cte_Childs ON Member.ParentId = cte_Childs.Id
    )
    SELECT * FROM cte_Childs WHERE Role >= 1 AND Role <= 5
    OPTION(MAXRECURSION 32767);
END;
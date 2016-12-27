
-- 建立 Stored Procedures
-- =============================================
-- Author: Greene
-- Last Modified Date: 2015-12-04
-- Description: 取得該會員所有的父會員
-- =============================================
CREATE PROCEDURE [dbo].[usp_GetAllParents]
    @Id int
AS
BEGIN
    SET NOCOUNT ON;

    WITH cte_Parents AS
    (
        SELECT * FROM dbo.Member WHERE Id = @Id
        UNION ALL
        SELECT Member.* FROM dbo.Member
        INNER JOIN cte_Parents ON Member.Id = cte_Parents.ParentId
    )
    SELECT * FROM cte_Parents WHERE Id <> @Id
    OPTION(MAXRECURSION 32767);
END;
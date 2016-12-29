
-- =============================================
-- Description: 給遊戲伺服器讀取的玩家資料
-- Update date: 2016-11-25
-- =============================================
CREATE VIEW dbo.PlayerData
AS
    SELECT Id, Account, Nickname, Role AS MemberType, Balance, 0 AS Eprob, AccessToken AS SessionId
    FROM dbo.Member
    WHERE (Role = 4 OR Role = 5) AND Status = 1;

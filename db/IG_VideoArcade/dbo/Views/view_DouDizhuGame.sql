
-- =============================================
-- Description: 
-- Update date: 2016-12-21
-- =============================================
CREATE VIEW dbo.view_DouDizhuGame
AS
    SELECT Id, GameId, SerialNumber, BaseValue, LordPlayerId, CallMultiplier, FinalMultiplier, NumOfSpring, NumOfAntiSpring, NumOfBomb, NumOfRocket, 3 AS TotalPlayer, 3 AS ActivePlayer, GameLog, IsResult, InsertDate
    FROM dbo.DouDizhuGame;

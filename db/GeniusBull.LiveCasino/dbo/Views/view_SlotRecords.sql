
-- =============================================
-- Description: 
-- Update date: 2016-12-21
-- =============================================
CREATE VIEW dbo.view_SlotRecords
AS
    SELECT Id, PlayerId, SerialNumber, GameId, ActionType, GameType, JPType, 1 AS TotalPlayer, 1 AS ActivePlayer, BetAmount, WinAmount, WinAmount - BetAmount AS ResultAmount, Balance, InsertDate
    FROM dbo.GameSpin;
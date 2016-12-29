
-- =============================================
-- Description: 
-- Update date: 2016-12-21
-- =============================================
CREATE VIEW dbo.view_DouDizhuBet
AS
    SELECT Id, DouDizhuGameId, PlayerId, IsLord, Results, Fee, BetAmount, WinAmount, WinAmount - BetAmount AS ResultAmount, Balance, InsertDate
    FROM dbo.DouDizhuBet;

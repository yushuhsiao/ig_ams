
-- =============================================
-- Description: 
-- Update date: 2016-12-21
-- =============================================
CREATE VIEW dbo.view_TwMahjongBet
AS
    SELECT Id, MahjongGameId, PlayerId, IsDealer, ExtraHand, SeatPosition, Fee, DcFine, DcCompe, BetAmount, WinAmount, WinAmount - BetAmount + DcCompe - DcFine AS ResultAmount, Balance, InsertDate
    FROM dbo.TwMahjongBet;

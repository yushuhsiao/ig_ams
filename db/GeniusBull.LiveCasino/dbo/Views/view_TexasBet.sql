﻿
-- =============================================
-- Description: 
-- Update date: 2016-12-21
-- =============================================
CREATE VIEW dbo.view_TexasBet
AS
    SELECT Id, TexasGameId, PlayerId, Seat, FirstCard, SecondCard, Bets, Results, Fee, BetAmount, WinAmount, WinAmount - BetAmount AS ResultAmount, Balance, InsertDate
    FROM dbo.TexasBet;
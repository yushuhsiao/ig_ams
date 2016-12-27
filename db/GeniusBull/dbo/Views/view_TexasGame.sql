CREATE VIEW dbo.view_TexasGame
AS
SELECT          Id, GameId, SerialNumber, DealerSeat, SmallBlind, BigBlind, Cards, TotalPlayer, ActivePlayer, GameLog, IsResult, 
                            InsertDate
FROM              dbo.TexasGame
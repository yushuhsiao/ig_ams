CREATE VIEW dbo.view_TwMahjongGame
AS
SELECT          Id, GameId, SerialNumber, Antes, Tai, RoundType, ServiceCharge, TotalFanValue, ActiveFanValue, WindPosition, 
                            4 AS TotalPlayer, 4 AS ActivePlayer, InsertDate
FROM              dbo.TwMahjongGame
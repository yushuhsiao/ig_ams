CREATE VIEW dbo.IG_GameLog2
AS
SELECT          PlatformID, Id, _flag, _sync1, _sync2, SerialNumber, PlayerId, GameId, ActionType, Bets, Odds, Symbols, GameType, 
                            Param_1, Param_2, Param_3, Param_4, Param_5, Pays, WinSpots, Deal_1, Deal_2, BackupCards, WinType, JPType, 
                            BetAmount, WinAmount, Balance, BetAmount - WinAmount AS Amount, 0 AS JP_Balance, 0 AS JP_Base, 
                            0 AS JP_Ratio, 0 AS JP_BaseAmount, 0 AS JP_FillAmount, 0 AS JP_GRAND, 0 AS JP_MAJOR, 0 AS JP_MINOR, 
                            0 AS JP_MINI, 0 AS JP_Total, InsertDate
FROM              (SELECT          PlatformID, 1 AS _Id, Id, _flag, _sync1, _sync2, SerialNumber, PlayerId, GameId, ActionType, Bets, Odds,
                                                         Symbols, GameType, Param_1, Param_2, Param_3, Param_4, Param_5, Pays, WinSpots, 
                                                        1 AS GameFinished, NULL AS Deal_1, NULL AS Deal_2, NULL AS BackupCards, NULL AS WinType, 
                                                        JPType, BetAmount, WinAmount, Balance, InsertDate, SyncFlag, _jp, _jp_GRAND, _jp_MAJOR, 
                                                        _jp_MINOR, _jp_MINI
                            FROM               dbo.GameSpin
                            UNION
                            SELECT          PlatformID, 2 AS _Id, Id, _flag, _sync1, _sync2, SerialNumber, PlayerId, GameId, ActionType, NULL 
                                                        AS Bets, NULL AS Odds, NULL AS Symbols, GameType, NULL AS Param_1, NULL AS Param_2, NULL 
                                                        AS Param_3, NULL AS Param_4, NULL AS Param_5, NULL AS Pays, NULL AS WinSpots, GameFinished, 
                                                        Deal_1, Deal_2, BackupCards, WinType, JPType, BetAmount, WinAmount, Balance, InsertDate, 
                                                        SyncFlag, _jp, _jp_GRAND, _jp_MAJOR, _jp_MINOR, _jp_MINI
                            FROM              dbo.FivePK) AS a
WHERE          (InsertDate >=
                                (SELECT          MIN(InsertDate) AS Expr1
                                  FROM               dbo.IG_GameLog))

GO
EXECUTE sp_addextendedproperty @name = N'MS_DiagramPane1', @value = N'[0E232FF0-B466-11cf-A24F-00AA00A3EFFF, 1.00]
Begin DesignProperties = 
   Begin PaneConfigurations = 
      Begin PaneConfiguration = 0
         NumPanes = 4
         Configuration = "(H (1[28] 4[33] 2[20] 3) )"
      End
      Begin PaneConfiguration = 1
         NumPanes = 3
         Configuration = "(H (1 [50] 4 [25] 3))"
      End
      Begin PaneConfiguration = 2
         NumPanes = 3
         Configuration = "(H (1 [50] 2 [25] 3))"
      End
      Begin PaneConfiguration = 3
         NumPanes = 3
         Configuration = "(H (4 [30] 2 [40] 3))"
      End
      Begin PaneConfiguration = 4
         NumPanes = 2
         Configuration = "(H (1 [56] 3))"
      End
      Begin PaneConfiguration = 5
         NumPanes = 2
         Configuration = "(H (2 [66] 3))"
      End
      Begin PaneConfiguration = 6
         NumPanes = 2
         Configuration = "(H (4 [50] 3))"
      End
      Begin PaneConfiguration = 7
         NumPanes = 1
         Configuration = "(V (3))"
      End
      Begin PaneConfiguration = 8
         NumPanes = 3
         Configuration = "(H (1[56] 4[18] 2) )"
      End
      Begin PaneConfiguration = 9
         NumPanes = 2
         Configuration = "(H (1 [75] 4))"
      End
      Begin PaneConfiguration = 10
         NumPanes = 2
         Configuration = "(H (1[66] 2) )"
      End
      Begin PaneConfiguration = 11
         NumPanes = 2
         Configuration = "(H (4 [60] 2))"
      End
      Begin PaneConfiguration = 12
         NumPanes = 1
         Configuration = "(H (1) )"
      End
      Begin PaneConfiguration = 13
         NumPanes = 1
         Configuration = "(V (4))"
      End
      Begin PaneConfiguration = 14
         NumPanes = 1
         Configuration = "(V (2))"
      End
      ActivePaneConfig = 0
   End
   Begin DiagramPane = 
      Begin Origin = 
         Top = 0
         Left = 0
      End
      Begin Tables = 
         Begin Table = "a"
            Begin Extent = 
               Top = 6
               Left = 38
               Bottom = 136
               Right = 207
            End
            DisplayFlags = 280
            TopColumn = 0
         End
      End
   End
   Begin SQLPane = 
   End
   Begin DataPane = 
      Begin ParameterDefaults = ""
      End
   End
   Begin CriteriaPane = 
      Begin ColumnWidths = 11
         Column = 1440
         Alias = 1380
         Table = 1170
         Output = 720
         Append = 1400
         NewValue = 1170
         SortType = 1350
         SortOrder = 1410
         GroupBy = 1350
         Filter = 1350
         Or = 1350
         Or = 1350
         Or = 1350
      End
   End
End
', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'VIEW', @level1name = N'IG_GameLog2';


GO
EXECUTE sp_addextendedproperty @name = N'MS_DiagramPaneCount', @value = 1, @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'VIEW', @level1name = N'IG_GameLog2';


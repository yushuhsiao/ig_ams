CREATE VIEW dbo.IG_GameLog_JP
AS
SELECT          a.Id AS Id_A, b.Id AS Id_B, b.SerialNumber, b.PlayerId, b.GameId, b.ActionType, a.JackpotType, b.JPType, a.Jackpot, 
                            b.JP_Balance AS JP_Jackpot, a.Base, b.JP_Base, a.Ratio, b.JP_Ratio, a.WinAmount, 
                            b.WinAmount AS JP_WinAmount, a.BaseAmount, b.JP_BaseAmount, a.FillAmount, b.JP_FillAmount, b.WinType, 
                            b.BetAmount, b.Balance, b.Amount, b.JP_GRAND, b.JP_MAJOR, b.JP_MINOR, b.JP_MINI, b.JP_Total, 
                            b.InsertDate AS InsertDate_B, a.InsertDate AS InsertDate_A, b.Bets, b.Odds, b.Symbols, b.GameType, b.Param_1, 
                            b.Param_2, b.Param_3, b.Param_4, b.Param_5, b.Pays, b.WinSpots, b.Deal_1, b.Deal_2, b.BackupCards
FROM              dbo.JackpotLog AS a LEFT OUTER JOIN
                            dbo.IG_GameLog AS b ON a.SerialNumber = b.SerialNumber AND a.GameId = b.GameId AND 
                            a.PlayerId = b.PlayerId AND b.ActionType = 'PULLJP'

GO
EXECUTE sp_addextendedproperty @name = N'MS_DiagramPane1', @value = N'[0E232FF0-B466-11cf-A24F-00AA00A3EFFF, 1.00]
Begin DesignProperties = 
   Begin PaneConfigurations = 
      Begin PaneConfiguration = 0
         NumPanes = 4
         Configuration = "(H (1[24] 4[37] 2[21] 3) )"
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
         Configuration = "(H (1[24] 4[63] 2) )"
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
      ActivePaneConfig = 8
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
               Right = 205
            End
            DisplayFlags = 280
            TopColumn = 8
         End
         Begin Table = "b"
            Begin Extent = 
               Top = 6
               Left = 243
               Bottom = 136
               Right = 420
            End
            DisplayFlags = 280
            TopColumn = 32
         End
      End
   End
   Begin SQLPane = 
   End
   Begin DataPane = 
      PaneHidden = 
      Begin ParameterDefaults = ""
      End
   End
   Begin CriteriaPane = 
      Begin ColumnWidths = 11
         Column = 1440
         Alias = 1275
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
', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'VIEW', @level1name = N'IG_GameLog_JP';


GO
EXECUTE sp_addextendedproperty @name = N'MS_DiagramPaneCount', @value = 1, @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'VIEW', @level1name = N'IG_GameLog_JP';


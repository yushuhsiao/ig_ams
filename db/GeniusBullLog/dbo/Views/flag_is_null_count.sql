CREATE VIEW dbo.flag_is_null_count
AS
SELECT          name, cnt
FROM              (SELECT          'TwMahjongGame' AS name, COUNT(*) AS cnt
                            FROM               dbo.TwMahjongGame
                            WHERE           (_flag IS NULL)
                            UNION
                            SELECT          'TwMahjongBet' AS name, COUNT(*) AS cnt
                            FROM              dbo.TwMahjongBet
                            WHERE          (_flag IS NULL)
                            UNION
                            SELECT          'DouDizhuGame' AS name, COUNT(*) AS cnt
                            FROM              dbo.DouDizhuGame
                            WHERE          (_flag IS NULL)
                            UNION
                            SELECT          'DouDizhuBet' AS name, COUNT(*) AS cnt
                            FROM              dbo.DouDizhuBet
                            WHERE          (_flag IS NULL)
                            UNION
                            SELECT          'TexasGame' AS name, COUNT(*) AS cnt
                            FROM              dbo.TexasGame
                            WHERE          (_flag IS NULL)
                            UNION
                            SELECT          'TexasBet' AS name, COUNT(*) AS cnt
                            FROM              dbo.TexasBet
                            WHERE          (_flag IS NULL)
                            UNION
                            SELECT          'IG_GameLog' AS name, COUNT(*) AS cnt
                            FROM              dbo.IG_GameLog AS IG_GameLog
                            WHERE          (_flag IS NULL)
                            UNION
                            SELECT          'Oasis' AS name, COUNT(*) AS cnt
                            FROM              dbo.Oasis
                            WHERE          (_flag IS NULL)
                            UNION
                            SELECT          'RedDog' AS name, COUNT(*) AS cnt
                            FROM              dbo.RedDog
                            WHERE          (_flag IS NULL)
                            UNION
                            SELECT          'GameSpin' AS name, COUNT(*) AS cnt
                            FROM              dbo.GameSpin
                            WHERE          (_flag IS NULL)
                            UNION
                            SELECT          'FivePK' AS name, COUNT(*) AS cnt
                            FROM              dbo.FivePK
                            WHERE          (_flag IS NULL)
                            UNION
                            SELECT          'SlotGameLog' AS name, COUNT(*) AS cnt
                            FROM              dbo.SlotGameLog
                            WHERE          (_flag IS NULL)
                            UNION
                            SELECT          'JackpotLog' AS name, COUNT(*) AS cnt
                            FROM              dbo.JackpotLog
                            WHERE          (_flag IS NULL)
                            UNION
                            SELECT          'JackpotUpdateLog' AS name, COUNT(*) AS cnt
                            FROM              dbo.JackpotUpdateLog
                            WHERE          (_flag IS NULL)) AS a

GO
EXECUTE sp_addextendedproperty @name = N'MS_DiagramPane1', @value = N'[0E232FF0-B466-11cf-A24F-00AA00A3EFFF, 1.00]
Begin DesignProperties = 
   Begin PaneConfigurations = 
      Begin PaneConfiguration = 0
         NumPanes = 4
         Configuration = "(H (1[28] 4[21] 2[33] 3) )"
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
               Bottom = 102
               Right = 203
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
         Alias = 900
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
', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'VIEW', @level1name = N'flag_is_null_count';


GO
EXECUTE sp_addextendedproperty @name = N'MS_DiagramPaneCount', @value = 1, @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'VIEW', @level1name = N'flag_is_null_count';


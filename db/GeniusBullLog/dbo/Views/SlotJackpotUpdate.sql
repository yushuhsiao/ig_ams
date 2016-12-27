CREATE VIEW dbo.SlotJackpotUpdate
AS
SELECT          a.PlayerId, a.GameId, a.SerialNumber, a.cnt, b.PushAmount AS JP_GRAND, c.PushAmount AS JP_MAJOR, 
                            d.PushAmount AS JP_MINOR, e.PushAmount AS JP_MINI
FROM              (SELECT          PlayerId, GameId, SerialNumber, COUNT(*) AS cnt
                            FROM               dbo.JackpotUpdateLog
                            GROUP BY    PlayerId, GameId, SerialNumber) AS a LEFT OUTER JOIN
                            dbo.JackpotUpdateLog AS b ON a.SerialNumber = b.SerialNumber AND a.PlayerId = b.PlayerId AND 
                            a.GameId = b.GameId AND b.JackpotType = 'GRAND' LEFT OUTER JOIN
                            dbo.JackpotUpdateLog AS c ON a.SerialNumber = c.SerialNumber AND a.PlayerId = c.PlayerId AND 
                            a.GameId = c.GameId AND c.JackpotType = 'MAJOR' LEFT OUTER JOIN
                            dbo.JackpotUpdateLog AS d ON a.SerialNumber = d.SerialNumber AND a.PlayerId = d.PlayerId AND 
                            a.GameId = d.GameId AND d.JackpotType = 'MINOR' LEFT OUTER JOIN
                            dbo.JackpotUpdateLog AS e ON a.SerialNumber = e.SerialNumber AND a.PlayerId = e.PlayerId AND 
                            a.GameId = e.GameId AND e.JackpotType = 'MINI'
GO
EXECUTE sp_addextendedproperty @name = N'MS_DiagramPaneCount', @value = 1, @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'VIEW', @level1name = N'SlotJackpotUpdate';


GO
EXECUTE sp_addextendedproperty @name = N'MS_DiagramPane1', @value = N'[0E232FF0-B466-11cf-A24F-00AA00A3EFFF, 1.00]
Begin DesignProperties = 
   Begin PaneConfigurations = 
      Begin PaneConfiguration = 0
         NumPanes = 4
         Configuration = "(H (1[40] 4[20] 2[20] 3) )"
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
               Right = 205
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "b"
            Begin Extent = 
               Top = 6
               Left = 243
               Bottom = 136
               Right = 410
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "c"
            Begin Extent = 
               Top = 6
               Left = 448
               Bottom = 136
               Right = 615
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "d"
            Begin Extent = 
               Top = 6
               Left = 653
               Bottom = 136
               Right = 820
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "e"
            Begin Extent = 
               Top = 6
               Left = 858
               Bottom = 136
               Right = 1025
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
', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'VIEW', @level1name = N'SlotJackpotUpdate';


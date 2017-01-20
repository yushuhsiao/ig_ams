CREATE VIEW dbo.MemberAvatarX
AS
SELECT          dbo.Member.Id AS PlayerId, dbo.MemberAvatar.OwnerId, dbo.MemberJoinTable.GameId, 
                            dbo.MemberJoinTable.TableId, dbo.MemberJoinTable.State AS JoinState, dbo.MemberJoinTable.JoinTime, 
                            dbo.Member.Balance, dbo.Wallet.Balance AS WalletBalance, dbo.Member.ParentId, dbo.Member.Account, 
                            dbo.Member.Password, dbo.Member.Nickname, dbo.Member.Stock, dbo.Member.Role, dbo.Member.Type, 
                            dbo.Member.Status, dbo.Member.Email, dbo.Member.RegisterTime, dbo.Member.LastLoginIp, 
                            dbo.Member.LastLoginTime, dbo.Member.AccessToken, dbo.Wallet.KeepAliveKey
FROM              dbo.MemberAvatar WITH (nolock) LEFT OUTER JOIN
                            dbo.Member WITH (nolock) ON dbo.Member.Id = dbo.MemberAvatar.PlayerId LEFT OUTER JOIN
                            dbo.MemberJoinTable WITH (nolock) ON dbo.Member.Id = dbo.MemberJoinTable.PlayerId LEFT OUTER JOIN
                            dbo.Wallet WITH (nolock) ON dbo.Member.Id = dbo.Wallet.PlayerId AND 
                            dbo.MemberJoinTable.GameId = dbo.Wallet.GameId
GO
EXECUTE sp_addextendedproperty @name = N'MS_DiagramPaneCount', @value = 1, @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'VIEW', @level1name = N'MemberAvatarX';


GO
EXECUTE sp_addextendedproperty @name = N'MS_DiagramPane1', @value = N'[0E232FF0-B466-11cf-A24F-00AA00A3EFFF, 1.00]
Begin DesignProperties = 
   Begin PaneConfigurations = 
      Begin PaneConfiguration = 0
         NumPanes = 4
         Configuration = "(H (1[27] 4[34] 2[20] 3) )"
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
         Begin Table = "MemberAvatar"
            Begin Extent = 
               Top = 6
               Left = 38
               Bottom = 102
               Right = 203
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "Member"
            Begin Extent = 
               Top = 6
               Left = 241
               Bottom = 136
               Right = 412
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "MemberJoinTable"
            Begin Extent = 
               Top = 6
               Left = 450
               Bottom = 136
               Right = 615
            End
            DisplayFlags = 280
            TopColumn = 2
         End
         Begin Table = "Wallet"
            Begin Extent = 
               Top = 6
               Left = 653
               Bottom = 136
               Right = 818
            End
            DisplayFlags = 280
            TopColumn = 3
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
         Alias = 1800
         Table = 4005
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
', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'VIEW', @level1name = N'MemberAvatarX';


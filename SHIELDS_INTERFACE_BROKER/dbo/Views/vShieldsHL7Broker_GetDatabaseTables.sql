CREATE VIEW dbo.vShieldsHL7Broker_GetDatabaseTables
AS
SELECT        A.ID AS CONFIG_ID, A.NAME AS CONFIG_NAME, A.DESCRIPTION AS CONFIG_DESCRIPTION, DI.ID AS DATABASE_INSTANCE_ID, DI.NAME AS DATABASE_NAME, 
                         DI.SERVER AS DATABASE_SERVER, DT.ID AS DATABASE_TABLE_ID, DT.NAME AS DATABASE_TABLE, DIR.NAME AS DIRECTION_TYPE_NAME, 
                         COT.NAME AS COMMUNICATION_TYPE_NAME, CO.ID AS COMMUNICATION_ID, DTR.REQUIRES_IDENTITY AS DATABASE_TABLE_RELATION_HAS_DEPENDENCIES, 
                         DTR.ID AS DATABASE_TABLE_RELATION_ID
FROM            dbo.SYS_APPLICATION AS A INNER JOIN
                         dbo.SYS_COMMUNICATION AS CO ON CO.APPLICATION_ID = A.ID INNER JOIN
                         dbo.SYS_COMMUNICATION_TYPE AS COT ON COT.ID = CO.COMMUNICATION_TYPE_ID INNER JOIN
                         dbo.SYS_DIRECTION_TYPE AS DIR ON DIR.ID = CO.DIRECTION_TYPE_ID INNER JOIN
                         dbo.SYS_DATABASE_INSTANCE AS DI ON DI.COMMUNICATION_ID = CO.ID INNER JOIN
                         dbo.SYS_DATABASE_TABLE AS DT ON DI.ID = DT.DATABASE_INSTANCE_ID LEFT OUTER JOIN
                         dbo.SYS_DATABASE_TABLE_RELATION AS DTR ON DTR.TARGET_DATABASE_TABLE_ID = DT.ID

GO
GRANT SELECT
    ON OBJECT::[dbo].[vShieldsHL7Broker_GetDatabaseTables] TO [ReadingRadPooledUser]
    AS [dbo];


GO
EXECUTE sp_addextendedproperty @name = N'MS_DiagramPane1', @value = N'[0E232FF0-B466-11cf-A24F-00AA00A3EFFF, 1.00]
Begin DesignProperties = 
   Begin PaneConfigurations = 
      Begin PaneConfiguration = 0
         NumPanes = 4
         Configuration = "(H (1[41] 4[20] 2[24] 3) )"
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
         Begin Table = "A"
            Begin Extent = 
               Top = 6
               Left = 38
               Bottom = 118
               Right = 208
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "CO"
            Begin Extent = 
               Top = 120
               Left = 464
               Bottom = 249
               Right = 704
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "COT"
            Begin Extent = 
               Top = 120
               Left = 38
               Bottom = 215
               Right = 208
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "DIR"
            Begin Extent = 
               Top = 138
               Left = 246
               Bottom = 233
               Right = 416
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "DI"
            Begin Extent = 
               Top = 6
               Left = 246
               Bottom = 135
               Right = 426
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "DT"
            Begin Extent = 
               Top = 6
               Left = 464
               Bottom = 118
               Right = 688
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "DTR"
            Begin Extent = 
               Top = 234
               Left = 38
               Bottom = 363
               Right = 289
            End
            DisplayFlags = 280
            TopColumn = 0
       ', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'VIEW', @level1name = N'vShieldsHL7Broker_GetDatabaseTables';






GO
EXECUTE sp_addextendedproperty @name = N'MS_DiagramPane2', @value = N'  End
      End
   End
   Begin SQLPane = 
   End
   Begin DataPane = 
      Begin ParameterDefaults = ""
      End
      Begin ColumnWidths = 9
         Width = 284
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
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
', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'VIEW', @level1name = N'vShieldsHL7Broker_GetDatabaseTables';




GO
EXECUTE sp_addextendedproperty @name = N'MS_DiagramPaneCount', @value = 2, @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'VIEW', @level1name = N'vShieldsHL7Broker_GetDatabaseTables';


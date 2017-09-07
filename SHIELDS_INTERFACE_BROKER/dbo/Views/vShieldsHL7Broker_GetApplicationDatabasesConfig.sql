CREATE VIEW dbo.vShieldsHL7Broker_GetApplicationDatabasesConfig
AS
SELECT        A.ID AS CONFIG_ID, A.NAME AS CONFIG_NAME, A.DESCRIPTION AS CONFIG_DESCRIPTION, DI.NAME AS DATABASE_NAME, 
                         DI.IP_ADDRESS AS DATABASE_IP_ADDRESS, DI.SERVER AS DATABASE_SERVER, CT.NAME AS CREDENTIAL_TYPE, C.ID AS CREDENTIAL_ID, 
                         C.USERNAME AS CREDENTIAL_USERNAME, C.PASSWORD AS CREDENTIAL_PASSWORD, COT.NAME AS COMMUNICATION_TYPE_NAME, 
                         CO.COMMUNICATION_TYPE_ID AS COMMUNICATION_COMMUNICATION_TYPE_ID, CO.APPLICATION_ID AS COMMUNICATION_APPLICATION_ID, 
                         CO.DIRECTION_TYPE_ID AS COMMUNICATION_DIRECTION_TYPE_ID, DT.NAME AS DIRECTION_TYPE_NAME, CO.ID AS COMMUNICATION_ID, 
                         DI.ID AS DATABASE_INSTANCE_ID, S.AUTOSTART AS APP_SETTING_AUTOSTART, S.DISABLED AS APP_SETTING_DISABLED, S.ID AS APP_SETTING_ID
FROM            dbo.SYS_APPLICATION AS A INNER JOIN
                         dbo.SYS_COMMUNICATION AS CO ON CO.APPLICATION_ID = A.ID INNER JOIN
                         dbo.SYS_DATABASE_INSTANCE AS DI ON CO.ID = DI.COMMUNICATION_ID INNER JOIN
                         dbo.SYS_CREDENTIAL AS C ON C.ID = DI.CREDENTIAL_ID INNER JOIN
                         dbo.SYS_CREDENTIAL_TYPE AS CT ON CT.ID = C.CREDENTIAL_TYPE_ID INNER JOIN
                         dbo.SYS_COMMUNICATION_TYPE AS COT ON COT.ID = CO.COMMUNICATION_TYPE_ID INNER JOIN
                         dbo.SYS_DIRECTION_TYPE AS DT ON DT.ID = CO.DIRECTION_TYPE_ID INNER JOIN
                         dbo.SYS_APPLICATION_SETTING AS S ON S.COMMUNICATION_ID = CO.ID

GO
GRANT SELECT
    ON OBJECT::[dbo].[vShieldsHL7Broker_GetApplicationDatabasesConfig] TO [ReadingRadPooledUser]
    AS [dbo];


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
         Top = -96
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
               Top = 138
               Left = 38
               Bottom = 267
               Right = 278
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
         Begin Table = "C"
            Begin Extent = 
               Top = 6
               Left = 464
               Bottom = 135
               Right = 669
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "CT"
            Begin Extent = 
               Top = 6
               Left = 707
               Bottom = 101
               Right = 877
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "COT"
            Begin Extent = 
               Top = 138
               Left = 316
               Bottom = 233
               Right = 486
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "DT"
            Begin Extent = 
               Top = 138
               Left = 524
               Bottom = 233
               Right = 694
            End
            DisplayFlags = 280
            TopColumn = 0
         ', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'VIEW', @level1name = N'vShieldsHL7Broker_GetApplicationDatabasesConfig';








GO
EXECUTE sp_addextendedproperty @name = N'MS_DiagramPane2', @value = N'End
         Begin Table = "S"
            Begin Extent = 
               Top = 234
               Left = 316
               Bottom = 363
               Right = 524
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
      Begin ColumnWidths = 10
         Width = 284
         Width = 1500
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
         Column = 2160
         Alias = 2820
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
', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'VIEW', @level1name = N'vShieldsHL7Broker_GetApplicationDatabasesConfig';








GO
EXECUTE sp_addextendedproperty @name = N'MS_DiagramPaneCount', @value = 2, @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'VIEW', @level1name = N'vShieldsHL7Broker_GetApplicationDatabasesConfig';


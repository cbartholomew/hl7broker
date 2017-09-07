CREATE VIEW dbo.vShieldsHL7Broker_GetAppBrokerMainList
AS
SELECT        B.ID AS BROKER_ID, A.DESCRIPTION AS APPLICATION_DESCRIPTION, A.NAME AS APPLICATION_NAME, B.QUEUE_COUNT AS BROKER_QUEUE_COUNT, 
                         S.NAME AS INTERFACE_STATUS_NAME, S.STATUS_COLOR AS INTERFACE_STATUS_COLOR, CT.NAME AS COMMUNICATION_TYPE_NAME, 
                         DT.NAME AS DIRECTION_TYPE_NAME, B.PROCESS_ID AS BROKER_PROCESS_ID, B.LAST_MESSAGE_DTTM AS BROKER_LAST_MESSAGE_DTTM, 
                         B.LAST_MESSAGE_ID AS BROKER_LAST_MESSAGE_ID, M.HL7_RAW AS MESSAGE_HL7_RAW, M.MESSAGE_HEADER_INSTANCE_ID, CO.DIRECTION_TYPE_ID, 
                         CO.COMMUNICATION_TYPE_ID, B.COMMUNICATION_ID AS BROKER_COMMUNICATION_ID, B.INTERFACE_STATUS_ID AS BROKER_INTERFACE_STATUS_ID, 
                         CO.APPLICATION_ID AS COMMUNICATION_APPLICATION_ID
FROM            dbo.APP_BROKER AS B WITH (NOLOCK) INNER JOIN
                         dbo.SYS_COMMUNICATION AS CO WITH (NOLOCK) ON CO.ID = B.COMMUNICATION_ID INNER JOIN
                         dbo.SYS_COMMUNICATION_TYPE AS CT WITH (NOLOCK) ON CT.ID = CO.COMMUNICATION_TYPE_ID INNER JOIN
                         dbo.SYS_APPLICATION AS A WITH (NOLOCK) ON A.ID = CO.APPLICATION_ID INNER JOIN
                         dbo.SYS_DIRECTION_TYPE AS DT WITH (NOLOCK) ON DT.ID = CO.DIRECTION_TYPE_ID INNER JOIN
                         dbo.SYS_INTERFACE_STATUS AS S WITH (NOLOCK) ON S.ID = B.INTERFACE_STATUS_ID LEFT OUTER JOIN
                         dbo.APP_MESSAGE AS M WITH (NOLOCK) ON M.ID = B.LAST_MESSAGE_ID
GO
GRANT SELECT
    ON OBJECT::[dbo].[vShieldsHL7Broker_GetAppBrokerMainList] TO [ReadingRadPooledUser]
    AS [dbo];


GO
EXECUTE sp_addextendedproperty @name = N'MS_DiagramPaneCount', @value = 2, @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'VIEW', @level1name = N'vShieldsHL7Broker_GetAppBrokerMainList';


GO
EXECUTE sp_addextendedproperty @name = N'MS_DiagramPane2', @value = N'      End
   End
   Begin SQLPane = 
   End
   Begin DataPane = 
      Begin ParameterDefaults = ""
      End
   End
   Begin CriteriaPane = 
      Begin ColumnWidths = 11
         Column = 2880
         Alias = 2520
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
', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'VIEW', @level1name = N'vShieldsHL7Broker_GetAppBrokerMainList';


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
         Begin Table = "B"
            Begin Extent = 
               Top = 6
               Left = 38
               Bottom = 135
               Right = 250
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "CO"
            Begin Extent = 
               Top = 6
               Left = 288
               Bottom = 135
               Right = 528
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "CT"
            Begin Extent = 
               Top = 6
               Left = 566
               Bottom = 101
               Right = 736
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "A"
            Begin Extent = 
               Top = 6
               Left = 774
               Bottom = 118
               Right = 944
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "DT"
            Begin Extent = 
               Top = 102
               Left = 566
               Bottom = 197
               Right = 736
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "S"
            Begin Extent = 
               Top = 120
               Left = 774
               Bottom = 232
               Right = 949
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "M"
            Begin Extent = 
               Top = 138
               Left = 38
               Bottom = 250
               Right = 304
            End
            DisplayFlags = 280
            TopColumn = 0
         End
', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'VIEW', @level1name = N'vShieldsHL7Broker_GetAppBrokerMainList';


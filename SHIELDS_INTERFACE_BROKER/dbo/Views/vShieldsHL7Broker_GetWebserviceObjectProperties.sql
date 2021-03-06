﻿CREATE VIEW dbo.vShieldsHL7Broker_GetWebserviceObjectProperties
AS
SELECT        A.ID AS CONFIG_ID, A.NAME AS CONFIG_NAME, A.DESCRIPTION AS CONFIG_DESCRIPTION, WP.NAME AS WEBSERVICE_PROPERTY, 
                         WP.MESSAGE_GROUP_INSTANCE_ID AS WEBSERVICE_PROPERTY_MESSAGE_GROUP_INSTANCE_ID, 
                         WP.COLUMN_DATA_TYPE AS WEBSERVICE_PROPERTY_COLUMN_DATA_TYPE, WO.ID AS WEBSERVICE_OBJECT_ID, WP.ID AS WEBSERVICE_PROPERTY_ID, 
                         DIR.NAME AS DIRECTION_TYPE_NAME, COT.NAME AS COMMUNICATION_TYPE_NAME
FROM            dbo.SYS_APPLICATION AS A INNER JOIN
                         dbo.SYS_COMMUNICATION AS CO ON CO.APPLICATION_ID = A.ID INNER JOIN
                         dbo.SYS_COMMUNICATION_TYPE AS COT ON COT.ID = CO.COMMUNICATION_TYPE_ID INNER JOIN
                         dbo.SYS_DIRECTION_TYPE AS DIR ON DIR.ID = CO.DIRECTION_TYPE_ID INNER JOIN
                         dbo.SYS_WEBSERVICE_INSTANCE AS WI ON CO.ID = WI.COMMUNICATION_ID INNER JOIN
                         dbo.SYS_WEBSERVICE_OBJECT AS WO ON WI.ID = WO.WEBSERVICE_INSTANCE_ID INNER JOIN
                         dbo.SYS_WEBSERVICE_PROPERTY_SET AS WP ON WO.ID = WP.WEBSERVICE_OBJECT_ID
GO
GRANT SELECT
    ON OBJECT::[dbo].[vShieldsHL7Broker_GetWebserviceObjectProperties] TO [InterfaceBrokerUser]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[vShieldsHL7Broker_GetWebserviceObjectProperties] TO [ReadingRadPooledUser]
    AS [dbo];


GO
EXECUTE sp_addextendedproperty @name = N'MS_DiagramPaneCount', @value = 2, @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'VIEW', @level1name = N'vShieldsHL7Broker_GetWebserviceObjectProperties';


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
', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'VIEW', @level1name = N'vShieldsHL7Broker_GetWebserviceObjectProperties';


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
               Top = 6
               Left = 246
               Bottom = 135
               Right = 486
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "COT"
            Begin Extent = 
               Top = 6
               Left = 524
               Bottom = 101
               Right = 694
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "DIR"
            Begin Extent = 
               Top = 102
               Left = 524
               Bottom = 197
               Right = 694
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "WI"
            Begin Extent = 
               Top = 120
               Left = 38
               Bottom = 249
               Right = 246
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "WO"
            Begin Extent = 
               Top = 138
               Left = 284
               Bottom = 250
               Right = 517
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "WP"
            Begin Extent = 
               Top = 198
               Left = 555
               Bottom = 327
               Right = 816
            End
            DisplayFlags = 280
            TopColumn = 0
       ', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'VIEW', @level1name = N'vShieldsHL7Broker_GetWebserviceObjectProperties';


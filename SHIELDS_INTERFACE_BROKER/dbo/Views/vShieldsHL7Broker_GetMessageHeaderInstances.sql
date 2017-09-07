CREATE VIEW dbo.vShieldsHL7Broker_GetMessageHeaderInstances
AS
SELECT        MHI.ID AS MHI_ID, MHI.SENDING_APPLICATION AS MHI_SENDING_APPLICATION, MHI.RECEIVING_APPLICATION AS MHI_RECEIVING_APPLICATION, 
                         MHI.SENDING_FACILITY AS MHI_SENDING_FACILITY, MHI.RECEIVING_FACILTIY AS MHI_RECEIVING_FACILTIY, MHI.MESSAGE_DTTM AS MHI_MESSAGE_DTTM, 
                         MHI.MESSAGE_CONTROL_ID AS MHI_MESSAGE_CONTROL_ID, MHI.MESSAGE_TYPE AS MHI_MESSAGE_TYPE, MHI.VERSION_ID AS MHI_VERSION_ID, 
                         MHI.APPLICATION_ACK_TYPE AS MHI_APPLICATION_ACK_TYPE, MHI.ACCEPT_ACK_TYPE AS MHI_ACCEPT_ACK_TYPE, 
                         MHI.ORDER_CONTROL_CODE AS MHI_ORDER_CONTROL_CODE, MHI.PATIENT_IDENTIFIER AS MHI_PATIENT_IDENTIFIER, MHI.PROCESSED AS MHI_PROCESSED, 
                         MHI.PROCESSED_DTTM AS MHI_PROCESSED_DTTM, MHI.PROCESSED_COUNT AS MHI_PROCESSED_COUNT, 
                         MHI.PENDING_REPROCESS_DTTM AS MHI_PENDING_REPROCESS_DTTM, MHI.CREATED_DTTM AS MHI_CREATED_DTTM, M.ID AS MESSAGE_ID, 
                         M.HL7_RAW AS MESSAGE_HL7_RAW
FROM            dbo.APP_MESSAGE_HEADER_INSTANCE AS MHI WITH (NOLOCK) INNER JOIN
                         dbo.APP_MESSAGE AS M WITH (NOLOCK) ON MHI.ID = M.MESSAGE_HEADER_INSTANCE_ID
GO
GRANT SELECT
    ON OBJECT::[dbo].[vShieldsHL7Broker_GetMessageHeaderInstances] TO [ReadingRadPooledUser]
    AS [dbo];


GO
EXECUTE sp_addextendedproperty @name = N'MS_DiagramPaneCount', @value = 1, @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'VIEW', @level1name = N'vShieldsHL7Broker_GetMessageHeaderInstances';


GO
EXECUTE sp_addextendedproperty @name = N'MS_DiagramPane1', @value = N'[0E232FF0-B466-11cf-A24F-00AA00A3EFFF, 1.00]
Begin DesignProperties = 
   Begin PaneConfigurations = 
      Begin PaneConfiguration = 0
         NumPanes = 4
         Configuration = "(H (1[25] 4[36] 2[20] 3) )"
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
         Begin Table = "MHI"
            Begin Extent = 
               Top = 6
               Left = 38
               Bottom = 135
               Right = 264
            End
            DisplayFlags = 280
            TopColumn = 14
         End
         Begin Table = "M"
            Begin Extent = 
               Top = 6
               Left = 302
               Bottom = 118
               Right = 568
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
         Column = 2460
         Alias = 2580
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
', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'VIEW', @level1name = N'vShieldsHL7Broker_GetMessageHeaderInstances';


﻿CREATE TABLE [dbo].[SYS_WEBSERVICE_PROPERTY_SET] (
    [ID]                        BIGINT        IDENTITY (1, 1) NOT NULL,
    [WEBSERVICE_OBJECT_ID]      BIGINT        NULL,
    [NAME]                      VARCHAR (200) NULL,
    [MESSAGE_GROUP_INSTANCE_ID] BIGINT        NULL,
    [COLUMN_DATA_TYPE]          VARCHAR (50)  NULL,
    CONSTRAINT [PK_SYS_WEBSERVICE_PROPERTY_SET] PRIMARY KEY CLUSTERED ([ID] ASC),
    CONSTRAINT [FK_SYS_WEBSERVICE_PROPERTY_SET_SYS_MESSAGE_GROUP_INSTANCE] FOREIGN KEY ([MESSAGE_GROUP_INSTANCE_ID]) REFERENCES [dbo].[SYS_MESSAGE_GROUP_INSTANCE] ([ID]),
    CONSTRAINT [FK_SYS_WEBSERVICE_PROPERTY_SET_SYS_WEBSERVICE_OBJECT] FOREIGN KEY ([WEBSERVICE_OBJECT_ID]) REFERENCES [dbo].[SYS_WEBSERVICE_OBJECT] ([ID])
);


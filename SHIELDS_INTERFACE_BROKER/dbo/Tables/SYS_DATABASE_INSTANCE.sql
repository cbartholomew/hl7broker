﻿CREATE TABLE [dbo].[SYS_DATABASE_INSTANCE] (
    [ID]               BIGINT         IDENTITY (1, 1) NOT NULL,
    [COMMUNICATION_ID] BIGINT         NULL,
    [CREDENTIAL_ID]    BIGINT         NULL,
    [NAME]             NVARCHAR (255) NULL,
    [SERVER]           NVARCHAR (255) NULL,
    [IP_ADDRESS]       NVARCHAR (255) NULL,
    CONSTRAINT [PK_SYS_DATABASE_INSTANCE] PRIMARY KEY CLUSTERED ([ID] ASC),
    CONSTRAINT [FK_SYS_DATABASE_INSTANCE_SYS_CREDENTIAL] FOREIGN KEY ([CREDENTIAL_ID]) REFERENCES [dbo].[SYS_CREDENTIAL] ([ID])
);

﻿CREATE TABLE [dbo].[SYS_CREDENTIAL_TYPE] (
    [ID]   BIGINT         IDENTITY (1, 1) NOT NULL,
    [NAME] NVARCHAR (255) NULL,
    CONSTRAINT [PK_SYS_CREDENTIAL_TYPE] PRIMARY KEY CLUSTERED ([ID] ASC)
);

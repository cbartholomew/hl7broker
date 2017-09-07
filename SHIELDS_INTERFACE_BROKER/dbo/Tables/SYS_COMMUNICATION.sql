﻿CREATE TABLE [dbo].[SYS_COMMUNICATION] (
    [ID]                    BIGINT IDENTITY (1, 1) NOT NULL,
    [DIRECTION_TYPE_ID]     BIGINT NULL,
    [COMMUNICATION_TYPE_ID] BIGINT NULL,
    [APPLICATION_ID]        BIGINT NULL,
    CONSTRAINT [PK_SYS_COMMUNICATION] PRIMARY KEY CLUSTERED ([ID] ASC),
    CONSTRAINT [FK_SYS_COMMUNICATION_SYS_APPLICATION] FOREIGN KEY ([APPLICATION_ID]) REFERENCES [dbo].[SYS_APPLICATION] ([ID]),
    CONSTRAINT [FK_SYS_COMMUNICATION_SYS_COMMUNICATION_TYPE] FOREIGN KEY ([COMMUNICATION_TYPE_ID]) REFERENCES [dbo].[SYS_COMMUNICATION_TYPE] ([ID]),
    CONSTRAINT [FK_SYS_COMMUNICATION_SYS_DIRECTION_TYPE] FOREIGN KEY ([DIRECTION_TYPE_ID]) REFERENCES [dbo].[SYS_DIRECTION_TYPE] ([ID])
);


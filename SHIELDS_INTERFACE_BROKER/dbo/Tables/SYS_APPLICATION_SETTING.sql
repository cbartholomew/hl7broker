﻿CREATE TABLE [dbo].[SYS_APPLICATION_SETTING] (
    [ID]               INT IDENTITY (1, 1) NOT NULL,
    [APPLICATION_ID]   INT NOT NULL,
    [COMMUNICATION_ID] INT CONSTRAINT [DF_SYS_APPLICATION_SETTING_COMMUNICATION_ID] DEFAULT ((0)) NOT NULL,
    [DISABLED]         BIT CONSTRAINT [DF_SYS_APPLICATION_SETTINGS_DISABLED] DEFAULT ((0)) NOT NULL,
    [AUTOSTART]        BIT CONSTRAINT [DF_SYS_APPLICATION_SETTINGS_AUTOSTART] DEFAULT ((0)) NOT NULL
);


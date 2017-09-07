CREATE TABLE [dbo].[APP_ACKNOWLEDGEMENT] (
    [ID]                      BIGINT   IDENTITY (1, 1) NOT NULL,
    [MESSAGE_ID]              BIGINT   NOT NULL,
    [ACKNOWLEDGEMENT_TYPE_ID] INT      NOT NULL,
    [RAW]                     TEXT     NOT NULL,
    [CREATED_DTTM]            DATETIME NOT NULL
);


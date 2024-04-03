CREATE TABLE [dbo].[UserFinance] (
    [Id]             INT             IDENTITY (1, 1) NOT NULL,
    [Email]          NVARCHAR (100)  NULL,
    [AccountName]    NVARCHAR (100)  NULL,
    [AccountBalance] DECIMAL (18, 2) NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC),
    CHECK ([AccountBalance]>=(-999999999999.99) AND [AccountBalance]<=(999999999999.99))
);


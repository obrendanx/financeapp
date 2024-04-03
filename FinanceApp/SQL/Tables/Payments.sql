CREATE TABLE [dbo].[Payments] (
    [PaymentId]    INT             IDENTITY (1, 1) NOT NULL,
    [PaymentName]  VARCHAR (100)   NOT NULL,
    [PaymentTotal] DECIMAL (18, 2) NOT NULL,
    [PaymentDate]  NVARCHAR (50)   NOT NULL,
    [PaymentFreq]  VARCHAR (20)    NOT NULL,
    [Email]        NVARCHAR (256)  NOT NULL,
    PRIMARY KEY CLUSTERED ([PaymentId] ASC)
);
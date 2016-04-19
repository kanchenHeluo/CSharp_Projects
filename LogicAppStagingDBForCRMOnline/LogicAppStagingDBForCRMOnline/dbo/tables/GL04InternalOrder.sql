CREATE TABLE [dbo].[GL04InternalOrder] (
    [InternalOrder]               INT              IDENTITY (1, 1) NOT NULL,
    [InternalOrderGuid]           UNIQUEIDENTIFIER NOT NULL,
    [InternalOrderNbr]            CHAR (12)        NOT NULL,
    [InternalOrderStatusCode]     CHAR (2)         NOT NULL,
    [GLCompanyCode]               CHAR (4)         NOT NULL,
    [InternalOrderDesc]           VARCHAR (40)     NULL,
    [ProfitCenterCode]            CHAR (10)        NOT NULL,
    [ProfitCenterEndDate]         DATETIME         NOT NULL,
    [PhysicalOrderCompleteInd]    CHAR (1)         NOT NULL,
    [PhysicalOrderClosedInd]      CHAR (1)         NOT NULL,
    [MarkedDeleteInd]             CHAR (1)         NOT NULL,
    [InternalOrderReleaseDate]    DATETIME         NULL,
    [InternalOrderCompletionDate] DATETIME         NULL,
    [InternalOrderWorkBeginDate]  DATETIME         NULL,
    [InternalOrderWorkEndDate]    DATETIME         NULL,
    [InternalOrderManagerName]    VARCHAR (20)     NULL,
    [CreatedDate]                 DATETIME         NULL,
    [TaxJurisdictionCode]         VARCHAR (15)     NULL,

    CONSTRAINT [PK_InternalOrderNbr] PRIMARY KEY CLUSTERED ([InternalOrderNbr] ASC)
);


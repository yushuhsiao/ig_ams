CREATE TABLE [dbo].[TranCert] (
    [CertID]         UNIQUEIDENTIFIER NOT NULL,
    [PaymentAccount] UNIQUEIDENTIFIER NOT NULL,
    [TranID]         UNIQUEIDENTIFIER NULL,
    [SerialNumber]   VARCHAR (16)     NULL,
    [data1]          VARCHAR (MAX)    NULL,
    [data2]          VARCHAR (MAX)    NULL,
    [CreateTime]     DATETIME         NOT NULL,
    [CreateUser]     INT              NOT NULL,
    CONSTRAINT [PK_TranCert] PRIMARY KEY CLUSTERED ([CertID] ASC)
);




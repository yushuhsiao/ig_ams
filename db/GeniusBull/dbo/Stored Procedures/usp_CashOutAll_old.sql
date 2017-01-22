
-- =============================================
-- Description: 把玩家電子錢包所有的錢轉到戶頭
-- Update date: 2016-05-27
-- =============================================
CREATE PROCEDURE dbo.usp_CashOutAll_old
    @PlayerBalance decimal(18, 2) OUTPUT,
    @WalletBalance decimal(18, 2) OUTPUT,
    @PlayerId int,
    @GameId int,
    @Date Datetime = NULL
AS
SET NOCOUNT ON;

DECLARE @Balance decimal(18, 2);

IF @Date IS NULL SET @Date = GETDATE();

BEGIN TRY
    BEGIN TRANSACTION;

        SET @PlayerBalance = (SELECT Balance FROM dbo.Member WITH (UPDLOCK) WHERE Id = @PlayerId);
        SET @WalletBalance = (SELECT Balance FROM dbo.Wallet WITH (UPDLOCK) WHERE PlayerId = @PlayerId AND GameId = @GameId);

        -- 查無此玩家或查無此玩家的錢包時 RETURN 2 後跳出
        IF @PlayerBalance IS NULL OR @WalletBalance IS NULL
        BEGIN
            SET @PlayerBalance = 0;
            SET @WalletBalance = 0;
            ROLLBACK TRANSACTION;
            RETURN 2;
        END;

        -- 扣掉玩家錢包所有的錢，加到玩家戶頭
        SET @Balance = @WalletBalance
        SET @PlayerBalance = @PlayerBalance + @Balance;
        SET @WalletBalance = @WalletBalance - @Balance;

        UPDATE dbo.Member SET Balance = @PlayerBalance
        WHERE Id = @PlayerId;

        UPDATE dbo.Wallet SET Balance = @WalletBalance, ModifyDate = @Date
        WHERE PlayerId = @PlayerId AND GameId = @GameId;

        -- 紀錄 Log
        INSERT INTO dbo.WalletTranLog (PlayerId, GameId, Type, Amount, AccountBalance, WalletBalance, TransactionTime)
        VALUES (@PlayerId, @GameId, 1, @Balance, @PlayerBalance, @WalletBalance, @Date);

    COMMIT TRANSACTION;
    RETURN 0;
END TRY
BEGIN CATCH
    IF @@TRANCOUNT > 0 ROLLBACK TRANSACTION;
    RETURN ERROR_NUMBER();
END CATCH;
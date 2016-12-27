
-- =============================================
-- Description: 把玩家電子錢包的錢轉到戶頭
-- Update date: 2016-05-27
-- =============================================
CREATE PROCEDURE dbo.usp_CashOut
    @PlayerBalance decimal(18, 2) OUTPUT,
    @WalletBalance decimal(18, 2) OUTPUT,
    @PlayerId int,
    @GameId int,
    @Balance decimal(18, 2),
    @Date Datetime = NULL
AS
SET NOCOUNT ON;

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

        -- 如果 @Balance < 0 或 @Balance > 玩家錢包的錢時 RETURN 2 後跳出
        IF @Balance < 0 OR @Balance > @WalletBalance
        BEGIN
            ROLLBACK TRANSACTION;
            RETURN 2;
        END;

        -- 扣掉玩家錢包的錢，加到玩家戶頭
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
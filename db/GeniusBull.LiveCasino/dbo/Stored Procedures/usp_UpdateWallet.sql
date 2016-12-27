
-- =============================================
-- Description: 更新玩家錢包的錢
-- Update date: 2016-05-27
-- =============================================
CREATE PROCEDURE dbo.usp_UpdateWallet
    @PlayerId int,
    @GameId int,
    @Balance decimal(18, 2),
    @Date Datetime = NULL
AS
SET NOCOUNT ON;

IF @Date IS NULL SET @Date = GETDATE();

BEGIN TRY
    -- 如果 @Balance < 0 RETURN 2 並跳出
    IF @Balance < 0
    BEGIN
        RETURN 2;
    END;

    -- 更新玩家錢包的錢
    UPDATE dbo.Wallet SET Balance = @Balance, ModifyDate = @Date
    WHERE PlayerId = @PlayerId AND GameId = @GameId;

    RETURN 0;
END TRY
BEGIN CATCH
    RETURN ERROR_NUMBER();
END CATCH;
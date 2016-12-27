
-- =============================================
-- Description: 存入 Minor
-- Update date: 2016-12-07
-- =============================================
CREATE PROCEDURE dbo.usp_PushMinorJackpot
    @Jackpot decimal(18, 6) OUTPUT,
    @PlayerId int,
    @GameId int,
    @SerialNumber varchar(18),
    @Amount decimal(18, 6),
    @Date Datetime = NULL
AS
SET NOCOUNT ON;

IF @Date IS NULL SET @Date = GETDATE();

BEGIN TRY
    BEGIN TRANSACTION;

        -- 取得目前的 Jackpot
        SET @Jackpot = (SELECT Amount FROM dbo.Jackpot WITH (UPDLOCK) WHERE JackpotType = 'MINOR' AND GameId = @GameId);

        IF @Jackpot IS NULL
        BEGIN
            SET @Jackpot = @Amount + (SELECT Base FROM dbo.JackpotConfig WHERE JackpotType = 'MINOR');

            INSERT INTO dbo.Jackpot (PlayerId, GameId, JackpotType, Amount)
            VALUES (0, @GameId, 'MINOR', @Jackpot);
        END
        ELSE
        BEGIN
            SET @Jackpot = @Jackpot + @Amount;

            UPDATE dbo.Jackpot SET Amount = @Jackpot
            WHERE JackpotType = 'MINOR' AND GameId = @GameId;
        END;

        -- 紀錄 Log，如果 SerialNumber = NULL 表示這次交易是用來初始化 Jackpot 的
        IF @SerialNumber IS NOT NULL
        BEGIN
            INSERT INTO dbo.JackpotUpdateLog (PlayerId, GameId, SerialNumber, JackpotType, Jackpot, PushAmount, InsertDate)
            VALUES (@PlayerId, @GameId, @SerialNumber, 'MINOR', @Jackpot, @Amount, @Date);
        END;

    COMMIT TRANSACTION;
    RETURN 0;
END TRY
BEGIN CATCH
    IF @@TRANCOUNT > 0 ROLLBACK TRANSACTION;
    RETURN ERROR_NUMBER();
END CATCH;
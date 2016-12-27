
-- =============================================
-- Description: 更新 Jackpot
-- Update date: 2016-05-27
-- =============================================
CREATE PROCEDURE dbo.usp_UpdateJackpotAmount
    @Jackpot decimal(18, 6) OUTPUT,
    @JackpotType varchar(10),
    @Amount decimal(18, 6),
    @GameId int = 0,
    @PlayerId int = 0
AS
SET NOCOUNT ON;

BEGIN TRY
    BEGIN TRANSACTION;

        SET @Jackpot = (SELECT Amount FROM dbo.Jackpot WITH (UPDLOCK) WHERE JackpotType = @JackpotType AND PlayerId = @PlayerId AND GameId = @GameId);

        IF @Jackpot IS NULL
        BEGIN
            SET @Jackpot = @Amount + (SELECT Base FROM dbo.JackpotConfig WHERE JackpotType = @JackpotType);

            INSERT INTO dbo.Jackpot (PlayerId, GameId, JackpotType, Amount)
            VALUES (@PlayerId, @GameId, @JackpotType, @Jackpot);
        END
        ELSE
        BEGIN
            SET @Jackpot = @Jackpot + @Amount;

            UPDATE dbo.Jackpot SET Amount = @Jackpot
            WHERE JackpotType = @JackpotType AND PlayerId = @PlayerId AND GameId = @GameId;
        END;

    COMMIT TRANSACTION;
    RETURN 0;
END TRY
BEGIN CATCH
    IF @@TRANCOUNT > 0 ROLLBACK TRANSACTION;
    RETURN ERROR_NUMBER();
END CATCH;
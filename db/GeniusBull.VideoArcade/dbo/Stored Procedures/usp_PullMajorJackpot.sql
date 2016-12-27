
-- =============================================
-- Description: 提取 Major，提取 Jackpot * @Ratio / 100，剩餘的 Jackpot 若小於打底時，補到打底值
-- Update date: 2016-11-22
-- =============================================
CREATE PROCEDURE dbo.usp_PullMajorJackpot
    @WinAmount decimal(18, 2) OUTPUT,
    @BaseAmount decimal(18, 6) OUTPUT,
    @GameId int,
    @PlayerId int,
    @SerialNumber varchar(18),
    @Ratio decimal(18, 6),
    @Date Datetime = NULL
AS
SET NOCOUNT ON;

DECLARE @Goal int = 0;
DECLARE @Base int = 0;
DECLARE @Percent decimal(18, 6) = @Ratio / 100;
DECLARE @FillAmount decimal(18, 6) = 0;
DECLARE @Jackpot decimal(18, 6);

IF @Date IS NULL SET @Date = GETDATE();

BEGIN TRY
    BEGIN TRANSACTION;

        -- 取得目標值跟打底
        SELECT @Goal = Goal, @Base = Base FROM dbo.JackpotConfig WHERE JackpotType = 'MAJOR';

        -- 取得目前的 Jackpot
        SET @Jackpot = (SELECT Amount FROM dbo.Jackpot WITH (UPDLOCK) WHERE JackpotType = 'MAJOR');

        -- 查無此 Jackpot 時 RETURN 2 後跳出
        IF @Jackpot IS NULL
        BEGIN
            SET @WinAmount = 0;
            SET @BaseAmount = 0;
            ROLLBACK TRANSACTION;
            RETURN 2;
        END;

        -- 如果 Jackpot 小於目標值，RETURN 2 後跳出
        IF @Jackpot < @Goal
        BEGIN
            SET @WinAmount = 0;
            SET @BaseAmount = @Jackpot;
            ROLLBACK TRANSACTION;
            RETURN 2;
        END;

        -- 玩家贏得的 Jackpot & 分派後剩餘的 Jackpot
        SET @WinAmount = @Jackpot * @Percent;
        SET @BaseAmount = @Jackpot - @WinAmount;

        -- 剩餘的 Jackpot 小於打底時，補到打底值
        IF @BaseAmount < @Base
        BEGIN
            SET @FillAmount = @Base - @BaseAmount;
            SET @BaseAmount = @Base;
        END;

        -- 更新 JackpotType
        UPDATE dbo.Jackpot SET Amount = @BaseAmount
        WHERE JackpotType = 'MAJOR';

        -- 紀錄 Log
        INSERT INTO dbo.JackpotLog (PlayerId, GameId, SerialNumber, JackpotType, Jackpot, Base, Ratio, WinAmount, BaseAmount, FillAmount, InsertDate)
        VALUES (@PlayerId, @GameId, @SerialNumber, 'MAJOR', @Jackpot, @Base, @Ratio, @WinAmount, @BaseAmount, @FillAmount, @Date);

    COMMIT TRANSACTION;
    RETURN 0;
END TRY
BEGIN CATCH
    IF @@TRANCOUNT > 0 ROLLBACK TRANSACTION;
    RETURN ERROR_NUMBER();
END CATCH;

-- =============================================
-- Description: 提取 Minor，提取 Jackpot * @Ratio / 100，剩餘的 Jackpot 若小於打底時，補到打底值
-- Update date: 2016-12-30
-- =============================================
CREATE PROCEDURE dbo.usp_PullMinorJackpot
    @WinAmount decimal(18, 2) OUTPUT,
    @BaseAmount decimal(18, 6) OUTPUT,
    @GameId int,
    @PlayerId int,
    @SerialNumber varchar(18),
    @Ratio decimal(18, 6),
    @Date Datetime = NULL
AS
SET NOCOUNT ON;

DECLARE @Base int = 0;
DECLARE @Percent decimal(18, 6) = @Ratio / 100;
DECLARE @FillAmount decimal(18, 6) = 0;
DECLARE @Jackpot decimal(18, 6);

IF @Date IS NULL SET @Date = GETDATE();

BEGIN TRY
    BEGIN TRANSACTION;

        -- 取得打底
        SELECT @Base = Base FROM dbo.JackpotConfig WHERE JackpotType = 'MINOR';

        -- 取得目前的 Jackpot
        SET @Jackpot = (SELECT Amount FROM dbo.Jackpot WITH (UPDLOCK) WHERE JackpotType = 'MINOR' AND GameId = @GameId);

        -- 查無此 Jackpot 時 RETURN 2 後跳出
        IF @Jackpot IS NULL
        BEGIN
            SET @WinAmount = 0;
            SET @BaseAmount = 0;
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
        WHERE JackpotType = 'MINOR' AND GameId = @GameId;

        -- 紀錄 Log
        INSERT INTO dbo.JackpotLog (PlayerId, GameId, SerialNumber, JackpotType, Jackpot, Base, Ratio, WinAmount, BaseAmount, FillAmount, InsertDate)
        VALUES (@PlayerId, @GameId, @SerialNumber, 'MINOR', @Jackpot, @Base, @Ratio, @WinAmount, @BaseAmount, @FillAmount, @Date);

        -- IG Game Log
        INSERT INTO dbo.IG_GameLog (SerialNumber, PlayerId, GameId, ActionType, GameType, JPType, BetAmount, WinAmount, Balance, Amount, JP_Balance, JP_Base, JP_Ratio, JP_BaseAmount, JP_FillAmount, JP_GRAND, JP_MAJOR, JP_MINOR, JP_MINI, InsertDate)
        VALUES (@SerialNumber, @PlayerId, @GameId, 'PULLJP', '', 'MINOR', 0, @WinAmount, 0, @WinAmount, @Jackpot, @Base, @Ratio, @BaseAmount, @FillAmount, 0, 0, 0, 0, @Date);

    COMMIT TRANSACTION;
    RETURN 0;
END TRY
BEGIN CATCH
    IF @@TRANCOUNT > 0 ROLLBACK TRANSACTION;
    RETURN ERROR_NUMBER();
END CATCH;

-- =============================================
-- Description: 取得當前的 Jackpot 跟其目標值
-- Update date: 2016-05-27
-- =============================================
CREATE PROCEDURE dbo.usp_JackpotAllow
    @Jackpot decimal(18, 6) OUTPUT,
    @Goal int OUTPUT,
    @JackpotType varchar(10)
AS
SET NOCOUNT ON;

SET @Jackpot = 0;
SET @Goal = 0;

BEGIN TRY
    SELECT @Jackpot = Amount FROM dbo.Jackpot WHERE JackpotType = @JackpotType;
    SELECT @Goal = Goal FROM dbo.JackpotConfig WHERE JackpotType = @JackpotType;

    RETURN 0;
END TRY
BEGIN CATCH
    RETURN ERROR_NUMBER();
END CATCH;
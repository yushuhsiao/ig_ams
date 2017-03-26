
CREATE FUNCTION [dbo].[GetTranID]
(
	@UserID int,
	@PlatformID int,
	@DataTime datetime
)
RETURNS uniqueidentifier
AS
BEGIN
	DECLARE @ret uniqueidentifier
	select top(1) @ret = TranID from TranLog
	where UserID=@UserID and PlatformID=@PlatformID and CreateTime <= @DataTime
	order by CreateTime desc
	RETURN @ret
END
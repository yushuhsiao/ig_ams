CREATE FUNCTION [dbo].[Cash_Check]
(
	@PlayerId int,
    @GameId int,
    @TableId int,
    @OwnerId int,
	@MaxCount int = 6)
RETURNS int
AS
BEGIN
	declare @JoinCount int
	select @JoinCount = count(*) from MemberJoinTable with(nolock)
	where PlayerId <> @PlayerId and GameId = @GameId and TableId = @TableId and OwnerId = @OwnerId and [State] <> 0
	if @JoinCount > 0
		return -1
	else
	begin
		select @JoinCount = count(*) from MemberJoinTable with(nolock)
		where PlayerId <> @PlayerId and OwnerId = @OwnerId and GameId = @GameId and [State] <> 0
		if @JoinCount >= @MaxCount
			return -2
	end
	return @JoinCount
END
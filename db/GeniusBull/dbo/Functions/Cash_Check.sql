CREATE FUNCTION Cash_Check
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
	where PlayerId <> @PlayerId and GameId = @GameId and TableId = @TableId and OwnerId = @OwnerId and [State]=1
	if @JoinCount > 0
		set @JoinCount = -1
	else
	begin
		select @JoinCount = count(*) from MemberJoinTable with(nolock)
		where PlayerId <> @PlayerId and OwnerId = @OwnerId and GameId = @GameId
		if @JoinCount >= @MaxCount
			set @JoinCount = -2
	end
	return @JoinCount
END
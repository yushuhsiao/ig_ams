CREATE PROCEDURE [dbo].[sp_MemberJoinTable] (
	@PlayerId int, 
	@GameId int, 
	@TableId int,
	@AccessToken varchar(50),
	@LastLoginIp varchar(50) = null,
	@MaxCount int=6)
AS
BEGIN
	declare @JoinCount int, @AvatarId int, @Nickname nvarchar(50), @State int
	select @JoinCount = count(*) from MemberJoinTable with(nolock) where OwnerId = @PlayerId and GameId = @GameId and [State] <> 0
	if @JoinCount >= @MaxCount return

	--select *
	--from MemberAvatar a with(nolock)
	--left join MemberJoinTable b with(nolock) on a.PlayerId = b.PlayerId and b.GameId = @GameId
	--where a.OwnerId = @PlayerId
	--order by b.JoinTime asc

	select top(1) @AvatarId = a.PlayerId, @State = isnull([State], 0)
	from MemberAvatar a with(nolock)
	left join MemberJoinTable b with(nolock) on a.PlayerId = b.PlayerId and b.GameId = @GameId
	where a.OwnerId = @PlayerId and (b.[State] is null or b.[State] = 0)
	order by b.JoinTime asc

	if @AvatarId is null return

	select @Nickname = Nickname from dbo.Member nolock
	where Id = @PlayerId
	update dbo.Member set Nickname = @Nickname, LastLoginIp = @LastLoginIp, LastLoginTime = getdate(), AccessToken = @AccessToken
	where Id = @AvatarId

	delete from dbo.MemberJoinTable
	where PlayerId = @AvatarId and GameId = @GameId

	insert into dbo.MemberJoinTable (PlayerId, OwnerId, GameId, TableId, [State])
	values (@AvatarId, @PlayerId, @GameId, @TableId, 0)

	select * from dbo.MemberJoinTable with(nolock)
	where PlayerId = @AvatarId and GameId = @GameId

	--select * from dbo.Member nolock where Id in (@PlayerId, @AvatarId)
END
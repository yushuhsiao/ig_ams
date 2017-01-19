CREATE PROCEDURE dbo.sp_GetMemberAvatar (
	@PlayerId int, 
	@GameId int, 
	@TableId int, 
	@Account varchar(50), 
	@AccessToken varchar(50),
	@MaxCount int=5)
AS
BEGIN
declare
	@AvatarId int, 
	@AvatarCount int, 
	@Nickname nvarchar(50), 
	@LastLoginIp varchar(50)

	select @AvatarCount = count(*) from dbo.MemberAvatar with(nolock) where OwnerId = @PlayerId
	if @AvatarCount < @MaxCount
	begin
		insert into dbo.Member ( Account, ParentId, [Password], Nickname, Balance, Stock, [Role], [Type], [Status], Email, RegisterTime, LastLoginIp, LastLoginTime, AccessToken)
		select                  @Account, ParentId, [Password], Nickname,       0, Stock, [Role], [Type], [Status], Email,    getdate(), LastLoginIp, getdate(), @AccessToken
		from dbo.Member with(nolock) where Id = @PlayerId

		set @AvatarId = @@IDENTITY

		insert into dbo.MemberAvatar (PlayerId, OwnerId) values (@AvatarId, @PlayerId)
	end
	else
	begin
		select top(1) @AvatarId = a.Id, @Nickname = a.Account, @LastLoginIp = a.LastLoginIp
		from MemberAvatar b with(nolock) left join Member a with(nolock) on a.Id = b.PlayerId
		where b.OwnerId = @PlayerId order by a.LastLoginTime asc

		update dbo.Member set Nickname = @Nickname, LastLoginIp = @LastLoginIp, LastLoginTime = getdate(), AccessToken=@AccessToken
		where Id = @AvatarId

		delete from dbo.MemberJoinTable where PlayerId = @AvatarId and GameId = @GameId
	end
	insert into dbo.MemberJoinTable (PlayerId, GameId, OwnerId, TableId, [State])
	values (@AvatarId, @GameId, @PlayerId, @TableId, 0)
END
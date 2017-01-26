CREATE PROCEDURE [dbo].[sp_MemberAvatar_Add] (
	@PlayerId int, 
	@Account varchar(50), 
	@MaxCount int=5)
AS
BEGIN
declare
	@AvatarId int, 
	@AvatarCount int

	select @AvatarCount = count(*) from dbo.MemberAvatar with(nolock) where OwnerId = @PlayerId
	if @AvatarCount >= @MaxCount return

	insert into dbo.Member ( Account, ParentId, [Password], Nickname, Balance, Stock, [Role], [Type], [Status], Email, RegisterTime)
	select                  @Account, ParentId, [Password], Nickname,       0, Stock, [Role], [Type], [Status], Email,    getdate()
	from dbo.Member with(nolock) where Id = @PlayerId

	set @AvatarId = @@IDENTITY

	insert into dbo.MemberAvatar (PlayerId, OwnerId) values (@AvatarId, @PlayerId)
	select * from dbo.Member nolock where Id = @AvatarId
END
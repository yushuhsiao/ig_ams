
-- @@rowcount = 1 代表新增成功
CREATE PROCEDURE [dbo].[sp_AddBlackList]
	@MemberId int,
	@BlacklistId int
AS
BEGIN
	declare @cnt int

	select @cnt=count(Id) from MemberBlacklist where MemberId=@MemberId

	if @cnt < 20
		insert into MemberBlacklist (MemberId, BlacklistId, BlacklistTime) values (@MemberId, @BlacklistId, getdate())	
END
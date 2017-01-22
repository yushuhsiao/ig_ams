CREATE FUNCTION [dbo].[GetBalance] (@PlayerId int, @Balance decimal(18,2)) RETURNS decimal(18,2)
AS
BEGIN
	declare @OwnerId int, @OwnerBalance decimal(18,2)

	select @OwnerId = OwnerId from dbo.MemberAvatar nolock
	where PlayerId = @PlayerId

	if @OwnerId is not null
		select @OwnerBalance = Balance from dbo.Member nolock
		where Id = @OwnerId

	RETURN isnull(@OwnerBalance, @Balance)
END
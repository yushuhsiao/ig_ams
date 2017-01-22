CREATE FUNCTION dbo.GetAvatarOwnerId(@PlayerId int)
RETURNS int
AS
BEGIN
	declare @OwnerId int
	select @OwnerId = OwnerId from MemberAvatar nolock where PlayerId = @PlayerId
	RETURN isnull(@OwnerId,@PlayerId)
END

CREATE PROCEDURE [dbo].[pwd_update]
	@UserID int,
	@n int,
	@Active tinyint,
	@a varchar(50),
	@b varchar(50),
	@c varchar(50),
	@TTL int = null,
	@CreateUser int
AS
BEGIN
	insert into Pwd2   (UserID, ver, Active, n, a, b, c, TTL, CreateTime, CreateUser, ModifyTime, ModifyUser)
	select UserID, convert(bigint, ver), Active, n, a, b, c, TTL, CreateTime, CreateUser, getdate(), @CreateUser
	from Pwd1 nolock where [UserID]=@UserID
	delete from Pwd1 where [UserID]=@UserID
	insert into Pwd1 ([UserID], [n], [a], [b], [c], [Active], TTL, [CreateTime], [CreateUser])
	values           (@UserID , @n , @a , @b , @c , @Active ,@TTL, getdate()   , @CreateUser)
END
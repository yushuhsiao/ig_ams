CREATE PROCEDURE [dbo].[alloc_UserID]
	@Min int = 10000,
	@CorpID int,
	@UserType int,
	@UserName varchar(20)
AS
BEGIN
	declare
		@retry int,
		@ID int,
		@uid uniqueidentifier
	set @retry=1000
	while @retry > 0
	begin
		begin try
			set @uid = newid()
			insert into UserID ([uid], CorpID, UserType, UserName) values (@uid, @CorpID, @UserType, @UserName)
			select @ID = ID from UserID nolock where [uid] = @uid
			if @ID > @Min
			begin
				select @ID as ID, @uid as [uid]
				return
			end
			--delete from UserID where ID=@ID
		end try
		begin catch
		end catch
		set @retry = @retry - 1
	end
END
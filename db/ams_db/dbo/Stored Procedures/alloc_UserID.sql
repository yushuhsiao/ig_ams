

CREATE PROCEDURE [dbo].[alloc_UserID] (@ID int = null output, @uid uniqueidentifier output, @UserType tinyint = 0, @ACNT varchar(20) = '', @retry int=100, @verify bit=1)
AS
BEGIN
_begin:		if @ID is not null goto _end;
			set @uid = newid()
			insert into UserID(U,T,A) values (@uid, isnull(@UserType, 0), isnull(@ACNT,''))
			select @ID = I from UserID nolock where U = @uid;
			if @ID in (0,1) goto _clear
			if @verify <> 1 goto _next
_verify:	if exists (select ID from [Agent] nolock where ID=@ID) goto _clear
			if exists (select ID from [Admin] nolock where ID=@ID) goto _clear
			if exists (select ID from [Player] nolock where ID=@ID) goto _clear
			goto _next
_clear:		select @ID=null, @uid=null
_next:		set @retry=@retry-1
			if @retry > 0 goto _begin
_end:
-- truncate table _alloc_UserID
--declare @ID int, @uid uniqueidentifier
--exec dbo.alloc_UserID @ID output, @uid output
--select @ID, @uid
--select * from _alloc_UserID
END


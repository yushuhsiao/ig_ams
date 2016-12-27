CREATE procedure lang_proc @ver_name varchar(20) = null, @VPath varchar(200), @LCID int, @Name varchar(50), @Text nvarchar(50)
as
begin
	declare @t nvarchar(50)
	select @t = [Text] from Lang nolock where VPath=@VPath and LCID=@LCID and Name=@Name
	if @Text is null and @t is null return
	else if @Text = @t goto _end
	else if @Text is null
		delete from Lang where VPath=@VPath and LCID=@LCID and Name=@Name
	else if @t is null
		insert into Lang (VPath, LCID, Name, [Text]) values (@VPath, @LCID, @Name, @Text)
	else
		update Lang set [Text]=@Text where VPath=@VPath and LCID=@LCID and Name=@Name
	if @ver_name is not null
		exec table_ver @name=@ver_name, @get=null, @set=1
_end:
	select @t -- return old value
end
CREATE procedure [dbo].[table_ver] @name varchar(20), @set bit=null, @get bit=1
as
begin
	declare @ver timestamp

	if @set=1
	begin
		select @ver=ver from TableVer nolock where name=@name
		if @ver is null
			insert into TableVer (name) values (@name)
		else
			update TableVer set t=getdate() where name=@name
	end

	if @get=1
	begin
		select @ver=ver from TableVer nolock where name=@name
		select isnull(convert(bigint, @ver), 0) as ver
	end
_end:
end
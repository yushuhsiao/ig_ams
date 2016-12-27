CREATE PROCEDURE [dbo].[sp_GetImage]
	@MemberID int,
	@ImageType varchar(10),
	@Index int = 0,
	@Success bit = 1,
	@data1 bit = 1,
	@data2 bit = 0
AS
BEGIN
	declare @id uniqueidentifier

	select @id=ID from 
	(select row_number() over (order by CreateTime desc) as rowid, ID from Pictures nolock where MemberID=@MemberID and ImageType=@ImageType and @Success=1) a
	where rowid=@Index
	
	select * from Pictures nolock where ID = @id
	if @data1 = 1 select data from Pictures1 where ID=@id
	if @data2 = 1 select data as Template from Pictures2 where ID=@id

	--exec sp_GetImageByID @id=id, @data1=@data1, @data2=@data2
	--if @id is null return
	--if @GetAll = 1
	--	select * from Pictures nolock where ID = @id
	--else
	--	select @id as ID
END
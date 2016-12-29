CREATE PROCEDURE [dbo].[sp_GetImageByID]
	@id uniqueidentifier,
	@data1 bit = 1,
	@data2 bit = 0
AS
BEGIN
	if @id is null return
	select * from Pictures nolock where ID = @id
	if @data1 = 1 select data from Pictures1 where ID=@id
	if @data2 = 1 select data as Template from Pictures2 where ID=@id
END

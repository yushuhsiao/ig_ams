CREATE PROCEDURE [dbo].[sp_SaveImage]
	@CorpID int,
	@MemberID int,
	@ImageType varchar(10),
	@TakePictureKey varchar(50) = null,
	@Success bit,
	@data image,
	@template image = null,
	@id uniqueidentifier = null
AS
BEGIN
	if @id is null
	begin
		set @id = newid()
		insert into Pictures ( ID,  CorpID,  MemberID,  ImageType,  TakePictureKey,  Success)
		values               (@id, @CorpID, @MemberID, @ImageType, @TakePictureKey, @Success)
								 insert into Pictures1 (ID,data) values (@id, @data)
	end
	if @template is not null insert into Pictures2 (ID,data) values (@id, @template)
	select                 ID,  CorpID,  MemberID,  ImageType,  TakePictureKey,  Success, CreateTime from Pictures nolock where ID=@id
END

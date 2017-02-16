CREATE PROCEDURE [dbo].[alloc_TranID] (@prefix varchar(10)='_', @group int = 1, @sn varchar(16) output, @ID uniqueidentifier output, @len int = 16, @retry int = 100)
AS
BEGIN
	declare @SN2 int
_begin:
	begin try
		set @ID = newid()
		begin tran
		if @group=2 goto _2
		if @group=3 goto _3
_1:
		insert into TranID1 (ID, prefix) values (@ID, @prefix)
		select @SN2=SN from TranID1 nolock where ID=@ID
		goto _0
_2:
		insert into TranID2 (ID, prefix) values (@ID, @prefix)
		select @SN2=SN from TranID2 nolock where ID=@ID
		goto _0
_3:
		insert into TranID3 (ID, prefix) values (@ID, @prefix)
		select @SN2=SN from TranID3 nolock where ID=@ID
		goto _0
_0:
		commit tran
		set @sn=@prefix+right('0000000000000000' + convert(varchar, @SN2), @len - len(@prefix))
		goto _end
	end try
	begin catch
		rollback tran
	end catch
	set @retry = @retry - 1
	goto _begin
_end:
END
-- =============================================
-- Description: 把玩家電子錢包所有的錢轉到戶頭
-- Update date: 2016-01-18
-- =============================================
CREATE PROCEDURE [dbo].[usp_CashOutAll]
    @PlayerBalance decimal(18, 2) OUTPUT,
    @WalletBalance decimal(18, 2) OUTPUT,
    @PlayerId int,
    @GameId int,

    @Date Datetime = NULL
AS
SET NOCOUNT ON;
declare
	@TableId int, @JoinCount int, @OwnerId int,
	@prevPlayerBalance decimal(18, 2), 
	@prevWalletBalance decimal(18, 2)

	select @TableId = TableId from MemberJoinTable with(nolock) where PlayerId = @PlayerId and GameId = @GameId

	-- 取得帳號分身資訊
	set @OwnerId = dbo.GetAvatarOwnerId(@PlayerId)

	insert into WalletTranRequestLog ([Type],[PlayerId],[GameId],[TableId],OwnerId,[Date]) values ('CashOutAll',@PlayerId,@GameId,@TableId,@OwnerId,@Date )

	-- 取得玩家點數, 查無此玩家時 RETURN 2 後跳出
	select @prevPlayerBalance = Balance from dbo.Member with(updlock) where Id = @OwnerId
	if @prevPlayerBalance is null goto _exit2

	-- 取得錢包原有點數, 錢包不存在時 RETURN 2 跳出
	select @prevWalletBalance = Balance FROM dbo.Wallet with(updlock) WHERE PlayerId = @PlayerId AND GameId = @GameId
	if @prevWalletBalance is null goto _exit2
	if @Date is null set @Date = getdate();

	begin try
		begin tran

		update dbo.Member set Balance = Balance + @prevWalletBalance
		where Id = @OwnerId

		update dbo.Wallet set Balance = Balance - @prevWalletBalance, ModifyDate = @Date
		where PlayerId = @PlayerId and GameId = @GameId

		-- 取得更新後的點數
		select @PlayerBalance = Balance from dbo.Member
		where Id = @OwnerId

		select @WalletBalance = Balance from dbo.Wallet
		where PlayerId = @PlayerId and GameId = @GameId

		if @PlayerBalance < 0 or @WalletBalance < 0 goto _exit2x; -- 更新後的點數小於 0 時返回錯誤

		-- 紀錄 Log
		insert into dbo.WalletTranLog (PlayerId, GameId, [Type], Amount, AccountBalance, WalletBalance, TransactionTime)
		values (@PlayerId, @GameId, 0, @PlayerBalance - @prevPlayerBalance, @PlayerBalance, @WalletBalance, @Date);

		if @TableId > 0
			update MemberJoinTable set [State]=0 where PlayerId = @PlayerId and GameId = @GameId and TableId = @TableId

		commit tran
		return 0;
	end try
	begin catch
		if @@trancount > 0 rollback tran;
		return error_number();
	end catch

_exit2x:
	rollback tran
_exit2:
	select @PlayerBalance=0, @WalletBalance=0
	return 2
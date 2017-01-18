-- =============================================
-- Description: 把玩家戶頭的錢轉到電子錢包
-- Update date: 2017-01-18
-- =============================================
CREATE PROCEDURE [dbo].[usp_CashIn]
    @PlayerBalance decimal(18, 2) OUTPUT,
    @WalletBalance decimal(18, 2) OUTPUT,
    @PlayerId int,
    @GameId int,
    @Balance decimal(18, 2),
    @Date Datetime = NULL
AS
SET NOCOUNT ON;
declare
	@TableId int, @JoinCount int, @OwnerId int,
	@prevPlayerBalance decimal(18, 2), 
	@prevWalletBalance decimal(18, 2)

	insert into WalletTranRequestLog ([Type],[PlayerId],[GameId],[Balance],[Date]) values ('CashIn', @PlayerId ,@GameId ,@Balance ,@Date )
	if @Balance < 0 goto _exit2;

	-- 取得帳號分身資訊
	set @OwnerId = isnull((select OwnerId from MemberAvatar nolock where PlayerId = @PlayerId), @PlayerId)

	-- 取得玩家點數, 查無此玩家時 RETURN 2 後跳出
	select @prevPlayerBalance = Balance from dbo.Member with(updlock) where Id = @OwnerId
	if @prevPlayerBalance is null goto _exit2

	-- MemberJoinTable.State, 在 dbo.usp_CashOutAll 時設為 0
	select @TableId = TableId from MemberJoinTable with(updlock) where PlayerId = @PlayerId and GameId = @GameId
	if @TableId > 0
	begin
		select @JoinCount = count(*) from MemberJoinTable with(updlock)
		where PlayerId <> @PlayerId and GameId = @GameId and TableId = @TableId and OwnerId = @OwnerId and [State] <> 0 
		if @JoinCount > 0 goto _exit2
	end

	-- 取得錢包原有點數
	select @prevWalletBalance = Balance FROM dbo.Wallet WITH (UPDLOCK) WHERE PlayerId = @PlayerId AND GameId = @GameId
	if @Date is null set @Date = getdate();

	begin try
		begin tran

		update dbo.Member set Balance = Balance - @Balance
		where Id = @OwnerId

		if @prevWalletBalance is null
			insert into dbo.Wallet (PlayerId, GameId, Balance, InsertDate, ModifyDate)
			values (@PlayerId, @GameId, 0, getdate(), getdate());
		else
			update dbo.Wallet set Balance = Balance + @Balance, ModifyDate = @Date
			where PlayerId = @PlayerId and GameId = @GameId

		-- 取得更新後的點數
		select @PlayerBalance = Balance from dbo.Member
		where Id = @OwnerId

		select @WalletBalance = Balance from dbo.Wallet
		where PlayerId = @PlayerId and GameId = @GameId

		if @PlayerBalance < 0 or @WalletBalance < 0 goto _exit2x; -- 更新後的點數小於 0 時返回錯誤

		-- 紀錄 Log
		insert into dbo.WalletTranLog (PlayerId, GameId, [Type], Amount, AccountBalance, WalletBalance, TransactionTime)
		values (@PlayerId, @GameId, 0, @prevPlayerBalance - @PlayerBalance, @PlayerBalance, @WalletBalance, @Date);

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
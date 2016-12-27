-- ================================================
-- Use for reset Jackpot
-- ================================================
CREATE procedure [dbo].[sp_JackpotReset] as
begin
	begin try
		begin tran
		delete from Jackpot where Id not in (1,2)
		update Jackpot set Amount = (select Base from JackpotConfig nolock where JackpotType='GRAND') where Id=1
		update Jackpot set Amount = (select Base from JackpotConfig nolock where JackpotType='MAJOR') where Id=2
		select * from Jackpot nolock
		commit tran
	end try
	begin catch
		rollback tran
		throw
	end catch
end
--select *, datediff(ss, CreateTime, getdate()) from RecogSession
CREATE procedure [dbo].[sp_GetRecogSession] as
begin
	declare @id uniqueidentifier
	--update RecogSession set BeginTime = null,EndTime = null
	select top(1) * from RecogSession nolock
	where (BeginTime is null or datediff(ss, BeginTime, getdate()) > 3600) and EndTime is null
	order by CreateTime
end

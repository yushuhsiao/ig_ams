-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date, ,>
-- Description:	<Description, ,>
-- =============================================
CREATE FUNCTION [dbo].[Eprob_Reel_Rate](@a int, @b int)
RETURNS decimal(9,2)
AS
BEGIN
	if @a = 0 return 0
	return ( convert(decimal, @b) / convert(decimal, @a)) * 100
END
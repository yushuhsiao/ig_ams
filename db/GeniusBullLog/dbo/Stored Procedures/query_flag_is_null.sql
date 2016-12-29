CREATE PROCEDURE [dbo].[query_flag_is_null] as begin
select Id, 'TexasBet		' as n from TexasBet			where _flag is null union
select Id, 'TexasGame		' as n from TexasGame			where _flag is null union
select Id, 'DouDizhuBet		' as n from DouDizhuBet			where _flag is null union
select Id, 'DouDizhuGame	' as n from DouDizhuGame		where _flag is null union
select Id, 'TwMahjongBet	' as n from TwMahjongBet		where _flag is null union
select Id, 'TwMahjongGame	' as n from TwMahjongGame		where _flag is null union
select Id, 'Oasis			' as n from Oasis				where _flag is null union
select Id, 'RedDog			' as n from RedDog				where _flag is null union
select Id, 'FivePK			' as n from FivePK				where _flag is null union
select Id, 'GameSpin		' as n from GameSpin			where _flag is null
end

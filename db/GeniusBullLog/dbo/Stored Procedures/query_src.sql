CREATE PROCEDURE [dbo].[query_src] as begin
select Id, 'TexasBet'			as n from DGDB01.IG_VideoArcade.dbo.TexasBet			except select Id, 'TexasBet'			as n from TexasBet			union
select Id, 'TexasGame'			as n from DGDB01.IG_VideoArcade.dbo.TexasGame			except select Id, 'TexasGame'			as n from TexasGame			union
select Id, 'DouDizhuBet'		as n from DGDB01.IG_VideoArcade.dbo.DouDizhuBet			except select Id, 'DouDizhuBet'			as n from DouDizhuBet		union
select Id, 'DouDizhuGame'		as n from DGDB01.IG_VideoArcade.dbo.DouDizhuGame		except select Id, 'DouDizhuGame'		as n from DouDizhuGame		union
select Id, 'TwMahjongBet'		as n from DGDB01.IG_VideoArcade.dbo.TwMahjongBet		except select Id, 'TwMahjongBet' 		as n from TwMahjongBet		union
select Id, 'TwMahjongGame'		as n from DGDB01.IG_VideoArcade.dbo.TwMahjongGame		except select Id, 'TwMahjongGame'		as n from TwMahjongGame		union
select Id, 'IG_GameLog'			as n from DGDB01.IG_VideoArcade.dbo.IG_GameLog			except select Id, 'IG_GameLog'			as n from IG_GameLog		union
select Id, 'Oasis'				as n from DGDB01.IG_VideoArcade.dbo.Oasis				except select Id, 'Oasis'				as n from Oasis				union
select Id, 'RedDog'				as n from DGDB01.IG_VideoArcade.dbo.RedDog				except select Id, 'RedDog'				as n from RedDog			union
--select Id, 'FivePK'			as n from DGDB01.IG_VideoArcade.dbo.FivePK				except select Id, 'FivePK'				as n from FivePK			union
--select Id, 'GameSpin'			as n from DGDB01.IG_VideoArcade.dbo.GameSpin			except select Id, 'GameSpin'			as n from GameSpin			union
--select Id, 'JackpotLog'		as n from DGDB01.IG_VideoArcade.dbo.JackpotLog			except select Id, 'JackpotLog'			as n from JackpotLog		union
--select Id, 'JackpotUpdateLog'	as n from DGDB01.IG_VideoArcade.dbo.JackpotUpdateLog	except select Id, 'JackpotUpdateLog'	as n from JackpotUpdateLog	union
select Id, 'TexasBet'			as n from DGDB01.IG_TabletopGames.dbo.TexasBet			except select Id, 'TexasBet'			as n from TexasBet			union
select Id, 'TexasGame'			as n from DGDB01.IG_TabletopGames.dbo.TexasGame			except select Id, 'TexasGame'			as n from TexasGame			union
select Id, 'DouDizhuBet'		as n from DGDB01.IG_TabletopGames.dbo.DouDizhuBet		except select Id, 'DouDizhuBet'			as n from DouDizhuBet		union
select Id, 'DouDizhuGame'		as n from DGDB01.IG_TabletopGames.dbo.DouDizhuGame		except select Id, 'DouDizhuGame'		as n from DouDizhuGame		union
select Id, 'TwMahjongBet'		as n from DGDB01.IG_TabletopGames.dbo.TwMahjongBet		except select Id, 'TwMahjongBet' 		as n from TwMahjongBet		union
select Id, 'TwMahjongGame'		as n from DGDB01.IG_TabletopGames.dbo.TwMahjongGame		except select Id, 'TwMahjongGame'		as n from TwMahjongGame		union
select Id, 'IG_GameLog'			as n from DGDB01.IG_TabletopGames.dbo.IG_GameLog		except select Id, 'IG_GameLog'			as n from IG_GameLog		union
select Id, 'Oasis'				as n from DGDB01.IG_TabletopGames.dbo.Oasis				except select Id, 'Oasis'				as n from Oasis				union
select Id, 'RedDog'				as n from DGDB01.IG_TabletopGames.dbo.RedDog			except select Id, 'RedDog'				as n from RedDog			
--select Id, 'FivePK'			as n from DGDB01.IG_TabletopGames.dbo.FivePK			except select Id, 'FivePK'				as n from FivePK			union
--select Id, 'GameSpin'			as n from DGDB01.IG_TabletopGames.dbo.GameSpin			except select Id, 'GameSpin'			as n from GameSpin			union
--select Id, 'JackpotLog'		as n from DGDB01.IG_TabletopGames.dbo.JackpotLog		except select Id, 'JackpotLog'			as n from JackpotLog		union
--select Id, 'JackpotUpdateLog'	as n from DGDB01.IG_TabletopGames.dbo.JackpotUpdateLog	except select Id, 'JackpotUpdateLog'	as n from JackpotUpdateLog	
end
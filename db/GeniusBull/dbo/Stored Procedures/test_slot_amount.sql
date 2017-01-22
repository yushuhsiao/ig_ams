CREATE procedure test_slot_amount as begin

select a.GameId from AMSDB01.GeniusBullLog.dbo.FivePK a
left join FivePK b
on a.Id=b.Id
where a.WinAmount <> b.WinAmount
group by a.GameId

select a.GameId from AMSDB01.GeniusBullLog.dbo.GameSpin a
left join GameSpin b
on a.Id=b.Id
where a.WinAmount <> b.WinAmount
group by a.GameId

end
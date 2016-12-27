
CREATE PROCEDURE [dbo].[TableSchema] AS BEGIN declare @x int
/*
	Config		- Config
	Currency	-
	Game		-
	GrpA1		-
	GrpA2		-
	GrpB1		-
	GrpB2		-
	GrpC1		-
	GrpC2		-
	Lang		-
	LoginLog	-
	LoginUrl	-
	Menu		-
	PWD			- Password
	PWDHist		- Password History
	PayA		- Agent Payment
	PayAD		- Agent Payment Default
	PayB		- Player Payment
	PayBD		- Player Payment Default
	Sec1		- Permission define
	Sec2		- Permission apply
	Sec3		- UserMenu
	TableVer	-
	UserA		- Agent
	UserAB		- Agent Balance					AgentBalance = UserAB.b1 + UserAB.b2
	UserB		- Member
	UserBB		- Member Balance				MemberBalance = UserBB.b1 + UserBB.b2 + sum(UserBG.Amount)
	UserBG		- Member Balance for Game
	UserBGVer	- Member Balance for Game (version)
	UserC		- Admin
	UserID		- ID alloc

	AccessControlList	- 受控管的路徑列表
	AccessControl		- User Access Control
*/
END


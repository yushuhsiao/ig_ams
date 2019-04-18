sln
	ams.sln		主要 sln
	ams.asp.sln	包含 asp.net core 原始碼的 sln
db
	ams_core	單一, 核心資料庫
	ams_user	複數, 用戶資料
	ams_log		複數, 用戶帳務資料

app
	ams					後台主站
	ams_api.Internal	Internal API Server, 跟內部其他系統串接用
	ams_api.Public		Public API Server, 供外部使用
	LiveStreamServer
	LogService			帳務拋轉
	PhotoServer			照片上傳

lib
	Tool
		工具函式庫

	InnateGlory.Base
		基本定義

	ams.Core
		共用函式庫/基本系統服務

	InnateGlory.GameBase
		遊戲基底

	ams.Payments
		實作支付平台介接時需引用的基底

	ams.Platforms
		實作遊戲平台介接時需引用的基底

	ams.TagHelpers
		TagHelper for Razor

	ams.Transactions

真人視訊系統

Sql Server
後台基本架構: view 和 data 分離 (類似 mvc), data 的部分以 api 的形式實作, 可以直接作為銜接營運商後台(現金流後台)的 api
投注 api 在不同遊戲時共用相同的基礎結構
fms 功能單純化, 盡可能只提供 live 和 replay, 必要的時候可增加 fms 的數量以承載流量
荷官介面 : html5, 內嵌視訊 (<video> or 其他 video plug-in)
荷官介面考慮額外提供觸控介面的版本, 取代 keyboard/mouse 



電子牌, 投注api
json webservice / websocket, 初期只實現其中之一, 後期同時提供
as3 可以找到 websocket package, 參見 https://github.com/theturtle32/AS3WebSocket, https://github.com/gimite/web-socket-js
提供 timestamp, 如果跟 fmle 的 timecode 差距太大就切斷視頻或是重新連接



遊戲後台
以現金流後台為基礎來修改
Sql Server
沿用 Corp/Admin/Agent/Member 的架構
dealer/pitboss 歸類為 admin
整合相關工具 (監控以及其他輔助工具)



mongoDB
mongoDB 做為程式記憶體使用 (狀態管理, 投注佇列.....)
量達到一定程度時, 增加 mongoDB 的數量 (非 sharded)
在每個牌桌各自有獨立 mongoDB 的情況下還有效能問題時, 考慮使用 sharded
不考慮使用 replication



corelib
	 核心共用函式庫
admin
	管理後臺, 荷官界面
game
	遊戲介面

	

tables
Config		組態設定
Game		遊戲列表
Lang		語系定義
LoginLog	登入紀錄
LoginUrl	登入網址
MenuA		功能表定義
MenuB		權限定義
UserA		代理帳號
UserB		管理帳號
UserC		會員帳號











平台設定
遊戲設定
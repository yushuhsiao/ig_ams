package events {
	import flash.net.Socket;
	import flash.net.XMLSocket;
	import flash.events.Event;
	import flash.events.IOErrorEvent;
	import flash.events.SecurityErrorEvent;
	import flash.events.ProgressEvent;
	import flash.errors.IOError;
	import flash.events.KeyboardEvent;
	import flash.events.DataEvent;
	import flash.events.MouseEvent;
	import flash.display.Sprite;
	import flash.system.Security;
	import flash.system.SecurityDomain;
	import flash.net.*;


	import com.worlize.websocket.*;
	import flash.utils.ByteArray;

	//import events.GUID;
	//import com.adobe.serialization.json.JSONDecoder;

	public class Connect extends Sprite {
		public var host: String = "ws://192.168.6.125:";
		public var port: String = "8080";
		public var protocol: String = "echo-protocol";
		//	public var socket:XMLSocket;
		public var status_: String;
		public var con_status: Boolean = false;
		
		public var data_: String;
		public var Data_: Object;
		
		public var bet_details: Object;
	
			public var account_details: Object;
		public var drew_details: Object;
		public var game_details: Object;
		public var countdown_details: Object;
			public var draw_details: Object;
			public var win_details: Object;
			public var won_details: Object;
		public var interval_details: Object;
			public var win_counter_details: Object;
			public var big_road_details: Object;
			
			public var account_f:Boolean = false;
		public var drew_f:Boolean = false;
				public var bet_f:Boolean = false;
		public var countdown_f:Boolean = false;
		public var draw_f:Boolean = false;
		public var win_f:Boolean = false;
		public var won_f:Boolean = false;
		public var interval_f:Boolean = false;
		public var win_counter_f:Boolean = false;
		public var big_road_f:Boolean = false;
		public var reset_f:Boolean = false;
		
		public var guid: String ;
		public var websocket: WebSocket;


		public function Connect() {
			
			super();
	doConnect();
		}
			public function doConnect() {
						websocket = new WebSocket(host + port, "*", protocol);
			//websocket.enableDeflateStream = true;
			websocket.addEventListener(WebSocketEvent.CLOSED, handleWebSocketClosed);
			websocket.addEventListener(WebSocketEvent.OPEN, handleWebSocketOpen);
			websocket.addEventListener(WebSocketEvent.MESSAGE, handleWebSocketMessage);

			websocket.addEventListener(WebSocketErrorEvent.CONNECTION_FAIL, handleConnectionFail);
			websocket.connect();

			}
		private function handleWebSocketOpen(event: WebSocketEvent): void {
			trace("Connected");
			status_ = "Connected";
			con_status = true;
			websocket.sendUTF(JSON.stringify({
				Server: "Server_1"
			}));

			//var binaryData:ByteArray = new ByteArray();
			//binaryData.writeUTF("Hello as Binary Message!");
			//websocket.sendBytes(binaryData);
		}

		private function handleWebSocketClosed(event: WebSocketEvent): void {
			trace("Disconnected");
			status_ = "Disconnected";
			doConnect();
		}

		private function handleConnectionFail(event: WebSocketErrorEvent): void {
			trace("Connection Failure: " + event.text);
		}

		private function handleWebSocketMessage(event: WebSocketEvent): void {
			if (event.message.type === WebSocketMessage.TYPE_UTF8) {
				trace("Got message: " + event.message.utf8Data);
				data_ = event.message.utf8Data.toString();
				Data_ = JSON.parse(data_);
				if(Data_.function_ == "Account"){
					 account_details =JSON.parse(data_);
					guid = account_details.ID;
					account_f = true;
				}
				if(Data_.function_ == "Drew"){
					drew_details =JSON.parse(data_);
					drew_f = true;
				}
				if(Data_.function_ == "Won"){
					won_details =JSON.parse(data_);
					won_f = true;
				}
					if(Data_.function_ == "Reset"){
					reset_f = true;
				}
				if(Data_.function_ == "Bet"){
					 bet_details =JSON.parse(data_);
					bet_f = true;
				}
				else if(Data_.function_ == "Countdown"){
					countdown_details = JSON.parse(data_);
					countdown_f = true;
				}
				else if(Data_.function_ == "Interval"){
					interval_details = JSON.parse(data_);
					interval_f = true;
				}
				else if(Data_.function_ == "Draw"){
					draw_details = JSON.parse(data_);
					draw_f = true;
				}
				else if(Data_.function_ == "Win"){
					win_details = JSON.parse(data_);
					win_f = true;
				}
				else if(Data_.function_ == "Win_counter"){
					win_counter_details = JSON.parse(data_);
					win_counter_f = true;
				}
					else if(Data_.function_ == "Big_road"){
					big_road_details = JSON.parse(data_);
					big_road_f = true;
				}
			
				
			} else if (event.message.type === WebSocketMessage.TYPE_BINARY) {
				trace("Got binary message of length " + event.message.binaryData.length);
			}
		}
		

	}

}
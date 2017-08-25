package screens {

	import flash.display.*;
	import flash.events.*;
	import flash.net.*;
	import flash.media.*;
	import flash.system.*;
	import flash.utils.ByteArray;

	

	 public class video extends Sprite {
		 private var netStreamObj: NetStream;
		 private var nc: NetConnection;
		 public var vid: Video;

			public var streamID: String;
		 public var videoURL: String;
		 private var metaListener: Object;

		 public function video() {
			init_RTMP();
		}

		public function init_RTMP(): void {
		
			streamID = "tft_1";
			videoURL = "rtmp://192.168.6.125:1935/live/";

			vid = new Video(); 

			nc = new NetConnection();
			nc.addEventListener(NetStatusEvent.NET_STATUS, onConnectionStatus);
			nc.addEventListener(AsyncErrorEvent.ASYNC_ERROR, asyncErrorHandler);
			nc.client = {
				onBWDone: function (): void {}
			};
			nc.connect(videoURL);
			this.addChildAt(vid,0);
		}

		private function onConnectionStatus(e: NetStatusEvent): void {
		
			if (e.info.code == "NetConnection.Connect.Success") {
				//trace("Creating NetStream");
				netStreamObj = new NetStream(nc);
 
				metaListener = new Object();
				metaListener.onMetaData = received_Meta;
				netStreamObj.client = metaListener;

				netStreamObj.play(streamID);
				vid.smoothing = true;
				
				vid.attachNetStream(netStreamObj);
			
				
			}
		}

		private function playback(): void {
			//trace((++counter) + " Buffer length: " + netStreamObj.bufferLength); 
		}

		 private function asyncErrorHandler(event: AsyncErrorEvent): void {
			trace("asyncErrorHandler.." + "\r");
		}

		 private function onFCSubscribe(info: Object): void {
			trace("onFCSubscribe - succesful");
		}

		 private function onBWDone(...rest): void {
			var p_bw: Number;
			if (rest.length > 0) {
				p_bw = rest[0];
			}
			trace("bandwidth = " + p_bw + " Kbps.");
		}

		private function received_Meta(data: Object): void {
			var _stageW: int = stage.stageWidth;
			var _stageH: int = stage.stageHeight;

			var _videoW: int;
			var _videoH: int;
			var _aspectH: int;

			var Aspect_num: Number; 
			Aspect_num = data.width / data.height;

			
			_videoW = _stageW;
			_videoH = _videoW / Aspect_num;
			_aspectH = (_stageH - _videoH) / 2;

			vid.x = 0;
			vid.y = _aspectH;
			vid.width = _videoW;
			vid.height = _videoH;
		}
	}

}
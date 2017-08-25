package {
	import flash.events.*;
	import flash.media.Video;
	import flash.display.Sprite;
		import flash.net.NetConnection;
	import flash.net.NetStream;
	import flash.text.TextField;
	import flash.display.SimpleButton;
	import flash.events.NetStatusEvent;
	import flash.events.AsyncErrorEvent;

	public class StreamingVideoPlayer extends Sprite {
		private var _videoURL: String;
		private var _video: Video;
		private var _vidDuration: Number;
		private var _vidXmax: Number;
		private var _vidYmax: Number;
		private var _vidWidth: Number;
		private var _vidHeight: Number;
		private var _main_nc: NetConnection = new NetConnection();
		private var _serverLoc: String;
		private var _ns: NetStream;
		private var _start_btn: SimpleButton;

		/* ------------Contructors/Initialization-----------*/
		public function StreamingVideoPlayer(serverLoc: String,
			flvLocation: String, vidDuration: Number, vidXmax: Number,
			vidYmax: Number): void {
			//set video params
			_videoURL = flvLocation;
			_vidDuration = vidDuration;
			_vidXmax = vidXmax;
			_vidYmax = vidYmax;
			//add eventListeners to NetConnection and connect
			_main_nc.addEventListener(NetStatusEvent.NET_STATUS,
				onNetStatus);
			_main_nc.addEventListener(
				SecurityErrorEvent.SECURITY_ERROR,
				securityErrorHandler);
				_main_nc.addEventListener(AsyncErrorEvent.ASYNC_ERROR, asyncErrorHandler);
			_main_nc.connect(serverLoc);
		}
		private function onNetStatus(event: Object): void {
			//handles NetConnection and NetStream status events
			switch (event.info.code) {
				case "NetConnection.Connect.Success":
					//play stream if connection successful
					connectStream();
					break;
				case "NetStream.Play.StreamNotFound":
					//error if stream file not found in
					//location specified
					trace("Stream not found: " + _videoURL);
					break;
				case "NetStream.Play.Stop":
					//do if video is stopped
					videoPlayComplete();
					break;
			}
			//trace(event.info.code);
		}
		

		/* -------------------Connection------------------- */
		private function connectStream(): void {
			//netstream object
			_ns = new NetStream(_main_nc);
			_ns.addEventListener(AsyncErrorEvent.ASYNC_ERROR,
				asyncErrorHandler);
			_ns.addEventListener(NetStatusEvent.NET_STATUS,
				onNetStatus);
			//other event handlers assigned 
			//to the netstream client property
			var custom_obj: Object = new Object();
			custom_obj.onMetaData = onMetaDataHandler;
			custom_obj.onCuePoint = onCuePointHandler;
			custom_obj.onPlayStatus = playStatus;
			_ns.client = custom_obj;
			//attach netstream to the video object
			_video = new Video(_vidXmax, _vidYmax);
			_video.attachNetStream(_ns);
			//video position could be parameterized but hardcoded
			//here to account for optional movie frame border
			_video.x = 1;
			_video.y = 1;
			addChild(_video);
			setVideoInit();
			_ns.play(_videoURL);
		}

		/* -------------NetStream actions and events-------------- */
		private function videoPlayComplete(): void {
			setVideoInit();
		}

		private function setVideoInit(): void {
			_ns.play(_videoURL);
			_ns.pause();
			_ns.seek(_vidDuration / 2);
			addStartBtn();
		}
		private function playStatus(event: Object): void {
			//handles onPlayStatus complete event if available
			switch (event.info.code) {
				case "NetStream.Play.Complete":
					//do if video play completes
					videoPlayComplete();
					break;
			}
			//trace(event.info.code);
		}
		private function playFlv(event: MouseEvent): void {
			_ns.play(_videoURL);
			//_ns.seek(192); //used for testing
			removeChild(_start_btn);
		}
		private function pauseFlv(event: MouseEvent): void {
			_ns.pause();
		}

		/* ---------Buttons, labels, and other assets------------- */
		//could be placed in it's own VideoControler class
		private function addStartBtn(): void {
			_start_btn = new SimpleButton();
			_start_btn.width = 80;
			_start_btn.x = (_vidXmax - _start_btn.width) / 2 + _video.x;
			_start_btn.y = (_vidYmax - _start_btn.height) / 2 + _video.y;
			//_start_btn.label = "Start Video";
			_start_btn.addEventListener(MouseEvent.CLICK, playFlv);
			//addChild(_start_btn);
		}

		/* -----------------Information handlers---------------- */
		private function onMetaDataHandler(metaInfoObj: Object): void {
			_video.width = metaInfoObj.width;
			_vidWidth = metaInfoObj.width;
			_video.height = metaInfoObj.height;
			_vidHeight = metaInfoObj.height;
			//trace("metadata: duration=" + metaInfoObj.duration + 
			//"width=" + metaInfoObj.width + " height=" +
			//metaInfoObj.height + " framerate=" +
			//metaInfoObj.framerate);
		}
		private function onCuePointHandler(cueInfoObj: Object): void {
			//trace("cuepoint: time=" + cueInfoObj.time + " name=" + 
			//cueInfoObj.name + " type=" + cueInfoObj.type);
		}

		/* -----------------------Error handlers------------------------ */
		private function securityErrorHandler(event: SecurityErrorEvent): void {
			trace("securityErrorHandler: " + event);
		}
		private function asyncErrorHandler(event: AsyncErrorEvent): void {
			trace(event.text);
		}
	}
}
package {
	import flash.display.Sprite;
	import flash.display.StageAlign;
	import flash.display.StageScaleMode;
	import flash.display.StageDisplayState;
	
	
	import starling.core.Starling;
	import starling.events.ResizeEvent;
	import starling.utils.RectangleUtil;
		import starling.utils.ScaleMode;
	
	import screens.Game;

	import flash.events.Event;
	import flash.events.ProgressEvent;
	import flash.geom.Rectangle;
	
import flash.system.Capabilities;

	[SWF( frameRate = "60", backgroundColor = "#000000")]
	public class Main extends Sprite {
		private var intilStarling: Starling;

		private static const BASE_WIDTH:Number = 1024||Capabilities.screenResolutionX;
		private static const BASE_HEIGHT:Number = 600||Capabilities.screenResolutionY;
		private static const SCALE_MODE:Boolean = true;
		trace(Capabilities.screenResolutionX,Capabilities.screenResolutionY)
		private var mStarling:Starling;
		private var mBaseRect:Rectangle;
 
	
		private static const PROGRESS_BAR_HEIGHT: Number = 20;

		public function Main() {
			super();
			this.loaderInfo.addEventListener(ProgressEvent.PROGRESS, loaderInfo_progressHandler);
			this.loaderInfo.addEventListener(Event.COMPLETE, loaderInfo_completeHandler);
		}
	

		private function loaderInfo_progressHandler(event: ProgressEvent): void {
			this.graphics.clear();
			this.graphics.beginFill(0xcccccc);
			this.graphics.drawRect(0, (this.stage.stageHeight - PROGRESS_BAR_HEIGHT) / 2,
			this.stage.stageWidth * event.bytesLoaded / event.bytesTotal, PROGRESS_BAR_HEIGHT);
			this.graphics.endFill();
		}
	
		private function loaderInfo_completeHandler(event: Event): void {
			
			this.graphics.clear();
			Starling.handleLostContext = true;
			stage.align = StageAlign.TOP_LEFT;
			stage.scaleMode = StageScaleMode.NO_SCALE;
			//stage.displayState = StageDisplayState.FULL_SCREEN_INTERACTIVE; 
			
			/* baseRect is a rectangle that represents the base dimensions at which you developped your game initially. */
			this.mBaseRect = new Rectangle(0, 0, BASE_WIDTH, BASE_HEIGHT);
 
			/* Create the viewport to pass through to starling, this uses the base width and height */
			var initialViewportRect:Rectangle = new Rectangle(0, 0, BASE_WIDTH, BASE_HEIGHT);
 
			/* Create starling using the Flash stage and the viewport we created above. */
			mStarling = new Starling(Game, stage, initialViewportRect);
 
			mStarling.stage.addEventListener(Event.RESIZE, onStarlingStageResize);
			mStarling.start();
	
		}
		
		private function onStarlingStageResize(event:ResizeEvent):void
		{
			this.resetScreenSize();
		}
 private function resetScreenSize():void {
			var screenRect:Rectangle = new Rectangle(0, 0, stage.stageWidth, stage.stageHeight);
 
			var viewportRect:Rectangle = RectangleUtil.fit(this.mBaseRect, screenRect, ScaleMode.SHOW_ALL);
			//viewportRect.copyFrom(screenRect);
		//	viewportRect.x = 0;
		//	viewportRect.y = 0;
			
			Starling.current.viewPort = viewportRect;
		}
	

		  
	}
}
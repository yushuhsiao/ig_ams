package screens {
	
	import events.GameSettings;
	import events.GameAnimation;
	import events.Connect;
	//import Main;
	
	import com.greensock.*;
	import com.greensock.easing.*;
	import com.greensock.plugins.*;

	import starling.core.Starling;
	import starling.display.Sprite;
	import starling.display.Image;
	import starling.textures.Texture;

	import starling.events.Event;
	import starling.events.Touch;
	import starling.events.TouchPhase;
	import starling.events.TouchEvent;
	
	import starling.text.TextField;
	
	//import com.worlize.websocket.*;
	import flash.events.DataEvent;
	 
	import com.greensock.plugins.ColorMatrixFilterPlugin;
	import com.adobe.protocols.dict.events.ConnectedEvent;

	import flash.events.TimerEvent;
		import flash.utils.Timer;
		
		import flash.events.SecurityErrorEvent;


	//import flash.display.Bitmap;
	//import flash.text.Font;

	//import flash.filters.GlowFilter;


	//TweenPlugin.activate([ColorMatrixFilterPlugin, GlowFilterPlugin]);

	public class Game extends Sprite {

		private var bg: Image;
		private var cont_bg: Image;

		private var cont_1: Image;
		private var cont_3: Image;

		private var cont_cash: Image;
		private var cont_mask: Image;
		private var cont_chip: Image;
		private var cont_2_: Image;

		//Buttons
		public var cont_select: Sprite = new Sprite();
		public var cont_btn_bet: Sprite = new Sprite();
		public var cont_btn_clear: Sprite = new Sprite();
		public var cont_btn_auto: Sprite = new Sprite();
		private var btn_bet_text:TextField;
		private var btn_bet: Image;
		private var btn_clear: Image;
		private var btn_auto: Image;
		private var btn_bet_lock: Image;
		private var btn_clear_lock: Image;
		private var btn_auto_lock: Image;
		private var auto_submit:Boolean = false;
		
		public var btn_next: Image;
		public var btn_back: Image;

		//Chip Settings

		public var cont_selection: Sprite = new Sprite();

		public var chip: Vector.<Image>= new Vector.<Image>();
		public var bet_chip: Vector.<Image>= new Vector.<Image>();
		public var bet_chip_mouse: Vector.<Image>= new Vector.<Image>();
		private var chip_texture: Vector.<Texture>= new Vector.<Texture>();
		private var bet_chip_texture: Vector.<Texture>= new Vector.<Texture>();
		private var cont_chip_:Sprite = new Sprite();
		private var chip_anim_up:Number = 523;
		private var chip_anim_mid:Number = 530;
		private var chip_anim_down:Number = 533;
		
		//Bet Area Settings
		public var cont_betArea: Sprite = new Sprite();
		private var bet: Vector.<Image>= new Vector.<Image>();
		private var bet_texture: Vector.<Texture>= new Vector.<Texture>();

		public var cont_hover_result: Sprite = new Sprite();
		public var bet_hover: Vector.<Image>= new Vector.<Image>();
		private var bet_hover_texture: Vector.<Texture>= new Vector.<Texture>();

		public var bet_result: Vector.<Image>= new Vector.<Image>();
		private var bet_result_texture: Vector.<Texture>= new Vector.<Texture>();

		public var cont_upper: Sprite = new Sprite();
		public var bet_upper: Vector.<Image>= new Vector.<Image>();

		private var bets: Array = new Array(0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
		private var bets_: Array = new Array(0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
		
		private var bets_Area: Array = new Array(0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
	
		private var o: Object = {bet_:{0:{0:Vector.<Image>},1:{1:Vector.<Image>},2:{2:Vector.<Image>},3:{3:Vector.<Image>},4:{4:Vector.<Image>},5:{5:Vector.<Image>},6:{6:Vector.<Image>},7:{7:Vector.<Image>},8:{8:Vector.<Image>},9:{9:Vector.<Image>}}};
		
		private var p: Object = {bet_:{0:{0:Vector.<Image>},1:{1:Vector.<Image>},2:{2:Vector.<Image>},3:{3:Vector.<Image>},4:{4:Vector.<Image>},5:{5:Vector.<Image>},6:{6:Vector.<Image>},7:{7:Vector.<Image>},8:{8:Vector.<Image>},9:{9:Vector.<Image>}}};

		private var bet_text:Array = new Array();
		private var bet_chip_text:Array = new Array();
		private var bet_chip_add_text:Array = new Array();
		private var bets_loc: Array = new Array(-20, -18, -13, -8, 0, 20, 5, 5, 10, 15);
		private var bets_chip_loc: Array = new Array(35, 25, 20, 23, 7, 7, 7, 7, 10, 12);
		private var bets_Min_X: Array = new Array(-40, -38, -40, -60, -10, 0, -50, -35, -35, -30);
		private var bets_Max_X: Array = new Array(0, 5, 7, 30, 30, 30, 30, 15, 18, 17);
		private var bets_Min_Y: Array = new Array(10, 7, 5, -15, -15, 0, -15, 5, 5, 6);
		private var bets_Max_Y: Array = new Array(15, 15, 13, 10, 10, 10, 10, 15, 12, 14);
		//Classes
		private var gameSettings: GameSettings = new GameSettings();
		private var gameAnimation: GameAnimation = new GameAnimation();
		private var connect_:Connect = new Connect()
		//private var main_:Main = new Main();
		
		private var total_balance: Number = 2000;
		private var tot_balance :Number = 0;
		private var total_balance_text: TextField ;
		private var total_bets:Number = 0;
		
		//------temp------
		private var temp_bal: Number = total_balance;
		private var temp_chip_selected: Number;
		private var temp_add_bets: Array = new Array(0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
		private var temp_bets: Array = new Array(0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
		private var temp_live:Array = new Array(0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
		private var temp_tbl: Array = new Array(0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
		//---Timer
		private var timerRefreshRate: Number = 1000;
		private var nCount: Number = 2;
		private var myTimer: Timer = new Timer(timerRefreshRate, nCount);
		private var refresh_: Timer = new Timer(1000);
	

		//---------font-----
		//private var f1:Font =new newFont;
		public function Game() {
			super();
		
			
		myTimer.addEventListener(TimerEvent.TIMER, countdown);
			myTimer.start();
			this.addEventListener(Event.ADDED_TO_STAGE, onAddedToStage);
			//connect_.socket.send(JSON.stringify({amount: 2000,bet: { 0:0,1:0,2:0,3:0,4:0,5:0,6:0,7:0,8:0,9:0}}));
		}
		
		
		private function onAddedToStage(event: Event): void {
	
				drawStage();
				this.removeEventListener(Event.ADDED_TO_STAGE, onAddedToStage);
			
		}
		var con:Boolean = false;
		private function refresh(e: TimerEvent): void {
		
			if(total_balance != 0 ||con){
				for(var i: Number = 0 ; i< gameSettings.bet_tag.length ;i++){
					
			if(connect_.Data_.bet[i]!= temp_live[i] ){		
			//	bets[i] = connect_.Data_.bet[i];
	
					chip_bet_check(i,connect_.Data_.bet[i] - temp_live[i]);
					bet_text[i].text = connect_.Data_.bet[i];
	temp_live[i] = connect_.Data_.bet[i];
	temp_tbl[i] = connect_.Data_.bet[i];
	con = true;
			}}
			
}
		
	
			}
	private function countdown(e: TimerEvent): void {
			if (connect_.status_ == "Connected") {
	//total_balance = int(connect_.Data_.amount);
		 temp_bal = total_balance;
				total_balance_text.text = total_balance.toString();
				myTimer.removeEventListener(TimerEvent.TIMER, countdown);
				myTimer.stop();
				refresh_.addEventListener(TimerEvent.TIMER, refresh);
			refresh_.start();
			}
			else{
		
				myTimer.addEventListener(TimerEvent.TIMER, countdown);
			myTimer.start();
			}


		}
	

		private function drawStage(): void {
			
			//Background
			bg = new Image(Assets.getTexture("BgLayer"));
			bg.width = stage.stageWidth;
			bg.height = stage.stageHeight;
			this.addChild(bg);
			setChildIndex(bg, 0);

			//Background panel lower
			cont_bg = new Image(Assets.getAtlas().getTexture("cont_bg instance 10000"));
			cont_bg.height = cont_bg.height - 90;
			cont_bg.width = stage.stageWidth;
			cont_bg.x = 0;
			cont_bg.y = stage.stageHeight - cont_bg.height;
			this.addChild(cont_bg);

			//Game Info
			cont_1 = new Image(Assets.getAtlas().getTexture("cont_1 instance 10000"));
			cont_1.height = cont_bg.height;
			cont_1.width = cont_1.width - 180;
			cont_1.x = 0;
			cont_1.y = stage.stageHeight - cont_1.height;
			this.addChild(cont_1);

			//Win History
			cont_2_ = new Image(Assets.getAtlas().getTexture("cont_2_ instance 10000"));
			cont_2_.height = cont_bg.height - 5;
			cont_2_.width = cont_2_.width - 20;
			cont_2_.x = cont_1.width;
			cont_2_.y = stage.stageHeight - cont_2_.height;
			this.addChild(cont_2_);


			//Game History
			cont_3 = new Image(Assets.getAtlas().getTexture("cont_3 instance 10000"));
			cont_3.height = cont_bg.height;
			cont_3.width = cont_3.width - 200;
			cont_3.x = stage.stageWidth - cont_3.width;
			cont_3.y = stage.stageHeight - cont_3.height;
			this.addChild(cont_3);

			panel_selection();

			panel_betArea();

		}
		private function panel_betArea(): void {
			//Betting Area
			var i: Number = 0;

			for (i = 0; i < gameSettings.bet_tag.length; i++) {
				bet_texture[i] = Assets.getAtlas().getTexture(gameSettings.bet_tag[i]);
				bet[i] = new Image(bet_texture[i]);
				bet[i].alignPivot("center", "bottom");
				bet[i].scaleX = gameSettings.bet_scale;
				bet[i].scaleY = gameSettings.bet_scale;
				if (i == 0) {
					bet[i].x = 90;
					bet[i].y = cont_bg.y - 20;
				}
				if (i == 1) {
					bet[i].x = bet[i - 1].x + bet[i - 1].width - (45 - gameSettings.bet_space);
					bet[i].y = bet[i - 1].y;
				}
				if (i == 2) {
					bet[i].x = bet[i - 1].x + bet[i - 1].width - (30 - gameSettings.bet_space);
					bet[i].y = bet[i - 1].y;
				}
				if (i == 3) {
					bet[i].x = bet[i - 1].x + bet[i - 1].width - (5 - gameSettings.bet_space);
					bet[i].y = bet[i - 1].y;
				}
				if (i == 4) {
					bet[i].x = bet[i - 1].x + bet[i - 1].width - (15 - gameSettings.bet_space);
					bet[i].y = bet[i - 1].y;
				}
				if (i == 5) {
					bet[i].x = bet[i - 2].x + bet[i - 1].width - (10 - gameSettings.bet_space);
					bet[i].y = bet[i - 1].y - 48;
				}
				if (i == 6) {
					bet[i].x = bet[i - 2].x + bet[i - 1].width + gameSettings.bet_space;
					bet[i].y = bet[i - 2].y;
				}
				if (i == 7) {
					bet[i].x = bet[i - 1].x + bet[i - 1].width - (35 - gameSettings.bet_space);
					bet[i].y = bet[i - 1].y;
				}
				if (i == 8) {
					bet[i].x = bet[i - 1].x + bet[i - 1].width - (15 - gameSettings.bet_space);
					bet[i].y = bet[i - 1].y;
				}
				if (i == 9) {
					bet[i].x = bet[i - 1].x + bet[i - 1].width - (25 - gameSettings.bet_space);
					bet[i].y = bet[i - 1].y;
				}
				if (i == 10) {
					bet[i].x = bet[i - 1].x + bet[i - 1].width - (45 - gameSettings.bet_space);
					bet[i].y = bet[i - 1].y;
				}
				cont_betArea.addChild(bet[i]);
			}

			//---------------------------------------------------------------------------
			for (i = 0; i < gameSettings.bet_hover_tag.length; i++) {
				bet_hover_texture[i] = Assets.getAtlas().getTexture(gameSettings.bet_hover_tag[i]);
				bet_hover[i] = new Image(bet_hover_texture[i]);
				bet_hover[i].alignPivot("center", "bottom");
				bet_hover[i].scaleX = gameSettings.bet_scale + .02;
				bet_hover[i].scaleY = gameSettings.bet_scale + .02;
				bet_hover[i].alpha = 0;
				if (i == 0) {
					bet_hover[i].x = bet[i].x + 5;
					bet_hover[i].y = bet[i].y - 4;
				}
				if (i == 1) {
					bet_hover[i].x = bet[i].x + 8;
					bet_hover[i].y = bet[i].y - 4;
				}
				if (i == 2) {
					bet_hover[i].x = bet[i].x - 3;
					bet_hover[i].y = bet[i].y - 2;
				}
				if (i == 3) {
					bet_hover[i].x = bet[i].x;
					bet_hover[i].y = bet[i].y - 4;
				}
				if (i == 4) {
					bet_hover[i].x = bet[i].x;
					bet_hover[i].y = bet[i].y - 4;
				}
				if (i == 5) {
					bet_hover[i].x = bet[i].x;
					bet_hover[i].y = bet[i].y - 2;
				}
				if (i == 6) {
					bet_hover[i].x = bet[i].x;
					bet_hover[i].y = bet[i].y - 2;
				}
				if (i == 7) {
					bet_hover[i].x = bet[i].x;
					bet_hover[i].y = bet[i].y - 2;
				}
				if (i == 8) {
					bet_hover[i].x = bet[i].x - 3;
					bet_hover[i].y = bet[i].y - 4;
				}
				if (i == 9) {
					bet_hover[i].x = bet[i].x - 10;
					bet_hover[i].y = bet[i].y - 4;
				}

				//bet_hover[i].visible = false;
				cont_hover_result.addChild(bet_hover[i]);
				cont_betArea.addChild(cont_hover_result);
			}
			//---------------------------------------------------------------

			for (i = 0; i < gameSettings.bet_result_tag.length; i++) {
				bet_result_texture[i] = Assets.getAtlas().getTexture(gameSettings.bet_result_tag[i]);
				bet_result[i] = new Image(bet_result_texture[i]);
				bet_result[i].alignPivot("center", "bottom");
				bet_result[i].scaleX = bet_hover[i].scaleX;
				bet_result[i].scaleY = bet_hover[i].scaleY;


				bet_result[i].x = bet_hover[i].x;
				bet_result[i].y = bet_hover[i].y;


				bet_result[i].visible = false;
				cont_hover_result.addChild(bet_result[i]);
				cont_betArea.addChild(cont_hover_result);
			}
			//---------------------------------------------------------------

			for (i = 0; i < gameSettings.bet_result_tag.length; i++) {
				bet_upper[i] = new Image(bet_result_texture[i]);
				bet_upper[i].alignPivot("center", "bottom");
				bet_upper[i].scaleX = bet_hover[i].scaleX;
				bet_upper[i].scaleY = bet_hover[i].scaleY;
				bet_upper[i].alpha = 0.01;

				bet_upper[i].x = bet_hover[i].x;
				bet_upper[i].y = bet_hover[i].y;
				
				//------text----
				bet_text[i] = new TextField(100,20,"0","BERNHC",12);
				//bet_text[i].fontName = "BERNHC";
				bet_text[i].color = "0xFFFFFF";
				bet_text[i].alignPivot("center", "center");
				if(i==5){
				bet_text[i].x = bet_hover[i].x + bets_loc[i];
				bet_text[i].y = bet_hover[i].y -bet_hover[i].height - 10;
				}else{
				bet_text[i].x = bet_hover[i].x + bets_loc[i];
				bet_text[i].y = bet_hover[i].y + 10;
				}

				cont_upper.addChild(bet_upper[i]);
				cont_upper.addChild(bet_text[i]);
				cont_betArea.addChild(cont_upper);
			}


			this.addChild(cont_betArea);
			this.addChild(cont_hover_result);
			setChildIndex(cont_hover_result, 1);

			this.addChild(cont_upper);
			setChildIndex(cont_upper, numChildren - 1);


		}

		private function panel_selection(): void {

			//Container Chip
			cont_chip = new Image(Assets.getAtlas().getTexture("cont_chip instance 10000"));
			cont_chip.height = cont_chip.height - 30;
			cont_chip.width = cont_chip.width - 160;
			cont_chip.x = 365;
			cont_chip.y = stage.stageHeight - 90;
			this.addChild(cont_chip);

			cont_mask = new Image(Assets.getAtlas().getTexture("cont_chip instance 10000"));
			cont_mask.height = cont_mask.height;
			cont_mask.width = cont_mask.width - 190;
			cont_mask.x = 380;
			cont_mask.y = stage.stageHeight - 110;

			this.addChild(cont_mask);

			drawCoin();
			//Buttons
			var texture_back: Texture = Assets.getAtlas().getTexture("btn_back instance 10000");
			btn_back = new Image(texture_back);
			btn_back.alignPivot("right", "center");
			btn_back.scaleX = .8;
			btn_back.scaleY = .8;
			btn_back.x = cont_chip.x + 15;
			btn_back.y = cont_chip.y + 22;
			cont_select.addChild(btn_back);

			var texture_next: Texture = Assets.getAtlas().getTexture("btn_next instance 10000");
			btn_next = new Image(texture_next);
			btn_next.alignPivot("left", "center");
			btn_next.scaleX = .8;
			btn_next.scaleY = .8;
			btn_next.x = cont_chip.x + cont_chip.width - 15;
			btn_next.y = cont_chip.y + 22;
			cont_select.addChild(btn_next);

			//Balance
			cont_cash = new Image(Assets.getAtlas().getTexture("cont_cash instance 10000"));
			cont_cash.height -= 13;
			cont_cash.width -= 110;
			cont_cash.x = cont_chip.x - 7;
			cont_cash.y = cont_bg.y + 2;
			this.addChild(cont_cash);
			
			total_balance_text = new TextField(100,20,"0","BERNHC",18);
			total_balance_text.fontName = "BERNHC";
			total_balance_text.color =0xFFFF00;
			total_balance_text.alignPivot("center", "center");
			total_balance_text.x = cont_cash.x + 50;
			total_balance_text.y = cont_cash.y + 12 ;
			total_balance_text.text = total_balance.toString();
			this.addChild(total_balance_text);
			
			//Main Buttons
			var texture_bet: Texture = Assets.getAtlas().getTexture("btn_bet instance 1")
			btn_bet = new Image(texture_bet);
			btn_bet.alignPivot("center", "center");
			btn_bet.height -= 20;
			btn_bet.width -= 50;
			btn_bet.x = cont_chip.x + 50;
			btn_bet.y = stage.stageHeight - btn_bet.height + 10 ;
			cont_btn_bet.addChild(btn_bet);
			
			btn_bet_text = new TextField(100,20,"0","newFont",16);
			btn_bet_text.color =0xFFFFFF;
			btn_bet_text.alignPivot("center", "center");
			btn_bet_text.x = btn_bet.x;
			btn_bet_text.y = btn_bet.y ;
			btn_bet_text.text = "提交";
			cont_btn_bet.addChild(btn_bet_text);
		
			
		
			var texture_bet_lock: Texture = Assets.getAtlas().getTexture("btn_submit_lock instance 1")
			btn_bet_lock = new Image(texture_bet_lock);
			btn_bet_lock.alignPivot("center", "center");
			btn_bet_lock.height -= 20;
			btn_bet_lock.width -= 50;
			btn_bet_lock.x = btn_bet.x;
			btn_bet_lock.y = btn_bet.y;
			btn_bet_lock.visible = false;
			cont_btn_bet.addChild(btn_bet_lock);
			cont_select.addChild(cont_btn_bet);

			var texture_clear: Texture = Assets.getAtlas().getTexture("btn_clear instance 10000");
			btn_clear = new Image(texture_clear);
			btn_clear.alignPivot("center", "center");			
			btn_clear.height -= 20;
			btn_clear.width -= 50;
			btn_clear.x = btn_bet.x + btn_bet.width + 5;
			btn_clear.y = stage.stageHeight - btn_clear.height + 10;
			cont_btn_clear.addChild(btn_clear);
			
			
			var texture_clear_lock: Texture = Assets.getAtlas().getTexture("btn_clear_lock instance 1");
			btn_clear_lock = new Image(texture_clear_lock);
			btn_clear_lock.alignPivot("center", "center");
			btn_clear_lock.height -= 20;
			btn_clear_lock.width -= 50;
			btn_clear_lock.x = btn_clear.x ;
			btn_clear_lock.y = btn_clear.y ;
			btn_clear_lock.visible = false;
			cont_btn_clear.addChild(btn_clear_lock);
			
			cont_select.addChild(cont_btn_clear);

			var texture_auto: Texture = Assets.getAtlas().getTexture("btn_auto instance 10000");
			btn_auto = new Image(texture_auto);
			btn_auto.alignPivot("center", "center");
			btn_auto.height -= 20;
			btn_auto.width -= 50;
			btn_auto.x = btn_clear.x + btn_clear.width + 5;
			btn_auto.y = stage.stageHeight - btn_auto.height + 10;
			cont_btn_auto.addChild(btn_auto);
			
			var texture_auto_lock: Texture = Assets.getAtlas().getTexture("btn_auto_lock instance 1");
			btn_auto_lock = new Image(texture_auto_lock);
			btn_auto_lock.alignPivot("center", "center");
			btn_auto_lock.height -= 20;
			btn_auto_lock.width -= 50;
			btn_auto_lock.x = btn_auto.x;
			btn_auto_lock.y = btn_auto.y;
			btn_auto_lock.visible = false;
			cont_btn_auto.addChild(btn_auto_lock);
			
			cont_select.addChild(cont_btn_auto);

			this.addChild(cont_select);
		}


		private function drawCoin(): void {

			for (var i: Number = 0; i < gameSettings.chip_name.length; i++) {
				chip_texture[i] = Assets.getAtlas().getTexture(gameSettings.chip_name[i]);
				bet_chip_texture[i] = Assets.getAtlas().getTexture(gameSettings.chip_blank[i]);
				chip[i] = new Image(chip_texture[i]);
				bet_chip_mouse[i] = new Image(chip_texture[i]);
				bet_chip[i] = new Image(bet_chip_texture[i]);
				chip[i].alignPivot("center", "center");
				chip[i].scaleX = gameSettings.chip_scale;
				chip[i].scaleY = gameSettings.chip_scale;
				bet_chip_mouse[i].alignPivot("left", "top");
				bet_chip_mouse[i].scaleX = gameSettings.chip_scale;
				bet_chip_mouse[i].scaleY = gameSettings.chip_scale;

				if (i == 0) {
					chip[i].x = cont_chip.x + 47;
					chip[i].y = cont_chip.y + 23;
				} else {
					chip[i].x = chip[i - 1].x + 55;
					chip[i].y = chip[i - 1].y;
				}
				cont_selection.addChild(chip[i]);
				this.addChild(bet_chip_mouse[i]);
				bet_chip_mouse[i].alpha = 0;
			}

			this.addChild(cont_selection);
			cont_selection.mask = cont_mask;
			cont_select.addChild(cont_selection);
			drawChipAnimation();
			//gameAnimation.drawChipAnimation();
		}

		private function drawChipAnimation(): void {

			drawChipDefault();
			stage.addEventListener(TouchEvent.TOUCH, onTouch);
		}

		private function drawChipDefault(): void {
			//-------DEFAULT VALUE-----
			for (var i: Number = 0; i < gameSettings.chip_value.length; i++) {
				if (gameSettings.chip_value[i] == gameSettings.chip_selected)
					TweenMax.to(chip[i], .8, {
						scaleX: gameSettings.chip_scale_anim,
						scaleY: gameSettings.chip_scale_anim,
						y: chip_anim_up,
						ease: Bounce.easeOut
					});
			}
		}


		private function onTouchBackAnim(): void {
			for (var i: Number = 0; i < gameSettings.chip_name.length; i++) {
				if (gameSettings.chip_selected == gameSettings.chip_value[i])
					TweenMax.to(chip[i], .3, {
						scaleX: gameSettings.chip_scale,
						scaleY: gameSettings.chip_scale,
						y: chip_anim_down,
						ease: Linear.easeOut
					});
			}

		}
		private function onTouch(e: TouchEvent): void {

			//------------------chip select------------------------------
			for (var i: Number = 0; i < gameSettings.chip_name.length; i++) {
				if (e.getTouch(chip[i], TouchPhase.BEGAN)) {
					if (gameSettings.chip_selected != gameSettings.chip_value[i]) {
						onTouchBackAnim();
						TweenMax.to(chip[i], .8, {
							scaleX: gameSettings.chip_scale_anim,
							scaleY: gameSettings.chip_scale_anim,
							y: chip_anim_up,
							ease: Bounce.easeOut
						});
						gameSettings.chip_selected = gameSettings.chip_value[i];
					}
				}
			}
			//------------------------------------------------------
			if (e.getTouch(btn_next, TouchPhase.BEGAN)) {
				// click code goes here
				if (cont_selection.x > (-cont_selection.width + 270)) {
					TweenMax.to(cont_selection, .5, {
						x: cont_selection.x - 50
					});
				} else {
					TweenMax.to(cont_selection, .5, {
						x: (-cont_selection.width + 270),
						ease: Elastic.easeOut
					});
				}
			}

			if (e.getTouch(btn_back, TouchPhase.BEGAN)) {
				// click code goes here
				if (cont_selection.x < 0) {
					TweenMax.to(cont_selection, .5, {
						x: cont_selection.x + 50
					});
				} else {
					TweenMax.to(cont_selection, .5, {
						x: 0,
						ease: Elastic.easeOut
					});
				}
			}



			//-----------------------Hover-----------------------------------

			for (i = 0; i < gameSettings.chip_name.length; i++) {

				if (e.getTouch(chip[i], TouchPhase.HOVER)) {
					// rollover code goes here
					TweenLite.to(chip[i], .8, {
						scaleX: gameSettings.chip_scale_anim,
						scaleY: gameSettings.chip_scale_anim,
						//y: chip_anim_mid,
						ease: Elastic.easeOut
					});


				} else {
					if (gameSettings.chip_selected != gameSettings.chip_value[i])
					// rollout code goes here
						TweenLite.to(chip[i], .5, {
							scaleX: gameSettings.chip_scale,
							scaleY: gameSettings.chip_scale,
							//y: chip_anim_down,
							ease: Elastic.easeOut
						});
				}
			}
			if (e.getTouch(btn_back, TouchPhase.HOVER)) {
				// rollover code goes here
				TweenLite.to(btn_back, .8, {
					scaleX: .9,
					scaleY: .9,
					ease: Strong.easeOut
				});

				if (cont_selection.x < 0) {
					TweenMax.to(cont_selection, .5, {
						x: cont_selection.x + 30
					});
				} else {
					TweenMax.to(cont_selection, .5, {
						x: 0,
						ease: Elastic.easeOut
					});
				}

			} else {
				// rollout code goes here
				TweenLite.to(btn_back, .5, {
					scaleX: .8,
					scaleY: .8,
					ease: Linear.easeNone
				});
			}

			if (e.getTouch(btn_next, TouchPhase.HOVER)) {
				// rollover code goes here
				TweenLite.to(btn_next, .8, {
					scaleX: .9,
					scaleY: .9,
					ease: Strong.easeOut
				});

				if (cont_selection.x > (-cont_selection.width + 270)) {
					TweenMax.to(cont_selection, .5, {
						x: cont_selection.x - 30
					});
				} else {
					TweenMax.to(cont_selection, .5, {
						x: (-cont_selection.width + 270),
						ease: Linear.easeNone
					});
				}

			} else {
				// rollout code goes here
				TweenLite.to(btn_next, .5, {
					scaleX: .8,
					scaleY: .8,
					ease: Strong.easeOut
				});
			}

			//--------------------BET AREA------------------------

			var touch: Touch = e.getTouch(stage);
			var r: Boolean = false;

			for (i = 0; i < gameSettings.bet_tag.length; i++) {
				if (e.getTouch(bet_upper[i])) {
					//bet_hover[i].visible = true;
					TweenLite.to(bet_hover[i], .3, {
						alpha: 1
					});
					r = true;
				} else {
					TweenLite.to(bet_hover[i], .3, {
						alpha: 0
					});
					//bet_hover[i].visible = false;
				}
			}

			if (e.getTouch(cont_upper)) {
				for (var t: Number = 0; t < chip.length; t++) {
					if (gameSettings.chip_value[t] == gameSettings.chip_selected) {
						//bet_chip_mouse[t].visible = true;
						bet_chip_mouse[t].x = e.getTouch(cont_upper).globalX + 10;
						bet_chip_mouse[t].y = e.getTouch(cont_upper).globalY + 5;
						bet_chip_mouse[t].alpha = 1;
						setChildIndex(bet_chip_mouse[t], this.numChildren - 1);
						r = true;
					}
				}

			} else {

				if (!r)
					for (t = 0; t < chip.length; t++) {
						//bet_chip_mouse[t].visible = true;
						bet_chip_mouse[t].x = 0;
						bet_chip_mouse[t].y = 0;
						bet_chip_mouse[t].alpha = 0;
					}

			}
			//--------------------Bet Click-----------------
			if (e.getTouch(cont_btn_clear, TouchPhase.BEGAN)) {
				clear_event();
			}
				if (e.getTouch(cont_btn_auto, TouchPhase.BEGAN)) {
					//for (i = 0; i < gameSettings.bet_tag.length; i++) {
					//reset(i);
				//	}
					//total_balance_text.text = total_balance.toString();
					auto_event();
				}
					
				
				
				if (e.getTouch(cont_btn_bet, TouchPhase.BEGAN)) {
					bet_event();
				}


			
			for (i = 0; i < gameSettings.bet_tag.length; i++) {
				
	
				if (e.getTouch(bet_upper[i], TouchPhase.ENDED)) {
				
				//	for (t = 0; t < gameSettings.chip_name.length; t++) {
						//if (gameSettings.chip_selected == gameSettings.chip_value[t]) {
							//	chip_deck(i, t);
							temp_chip_selected	= gameSettings.chip_selected;
							if(temp_bal > 0){
							
								if(temp_chip_selected >temp_bal )
								{temp_chip_selected = temp_bal;
									
									}
								temp_bal -= temp_chip_selected;
								if(temp_bal>=0){
							bets[i] += temp_chip_selected;
							temp_add_bets[i] += temp_chip_selected;
							//temp_bets[i] = bets[i];
							total_bets+=temp_chip_selected;
							btn_bet_text.text = "提交("+total_bets+")";
							total_balance_text.text = temp_bal.toString();
							//trace(bets[i] + " - total= " + total_bets);
							chip_deck_check(i, bets[i]);
									
								}
								else{
									temp_chip_selected	= gameSettings.chip_selected;
								}
							}
							if(auto_submit){
								bet_event();
							}
				}
			}
		}
		private function chip_deck(i: Number, t: Number): void {
			
			if(cont_chip_.numChildren!=0&& i == 4){
			this.cont_chip_.removeChildAt(this.cont_chip_.numChildren -2);
			this.cont_chip_.removeChildAt(this.cont_chip_.numChildren -1);
			}
			
			this.removeChild(bet_chip_text[i]);
			this.removeChild(bet_chip_add_text[i]);
		
			o.bet_[i][bets_[i]] = new Image(bet_chip_texture[t]);
			o.bet_[i][bets_[i]].scaleX = .6;
			o.bet_[i][bets_[i]].scaleY = .6;
			o.bet_[i][bets_[i]].x = bet_upper[i].x - (bet_upper[i].width / 2) +  bets_chip_loc[i];
			o.bet_[i][bets_[i]].y = bet_upper[i].y - bet_upper[i].height - (bets_[i] * 3);
			(i==4)? cont_chip_.addChild(o.bet_[i][bets_[i]]) : this.addChild(o.bet_[i][bets_[i]]);
			
				bet_chip_text[i] = new TextField(100,20,"","BERNHC",12);
			
				bet_chip_text[i].color = "0x000000";
				bet_chip_text[i].alignPivot("center", "center");
			bet_chip_text[i].x = o.bet_[i][bets_[i]].x + (o.bet_[i][bets_[i]].width/2);
			bet_chip_text[i].y =o.bet_[i][bets_[i]].y + (o.bet_[i][bets_[i]].height/2);
			bet_chip_text[i].text = bets[i];
			(i==4) ? cont_chip_.addChild(bet_chip_text[i]) : this.addChild(bet_chip_text[i]);
			
			bet_chip_add_text[i] = new TextField(100,20,"","BERNHC",14);
				bet_chip_add_text[i].color = "0xFFFF00";
				bet_chip_add_text[i].alignPivot("center", "center");
			bet_chip_add_text[i].x = o.bet_[i][bets_[i]].x + (o.bet_[i][bets_[i]].width/2);
			bet_chip_add_text[i].y =o.bet_[i][bets_[i]].y - (o.bet_[i][bets_[i]].height/4) ;
			bet_chip_add_text[i].text = "+"+temp_add_bets[i];
			
			(i==4) ? cont_chip_.addChild(bet_chip_add_text[i]) : this.addChild(bet_chip_add_text[i]);

			//TweenLite.from(o.bet_[i][bets_[i]],.2,{y:o.bet_[i][bets_[i]].y - 3 });
			bets_[i] += 1;

			if(cont_chip_.numChildren !=0){
			this.addChild(cont_chip_);				
			setChildIndex(this.cont_chip_, numChildren - 1);
			}
		
			}
		


		private function chip_deck_check(i: Number, b: Number): void {
		
			var temp: Number;
			var m: Number;
			for (m = 0; m < bets_[i]; m++) {
					this.removeChild(o.bet_[i][m] );
			}
		
		if(i  ==4 && cont_chip_.numChildren !=0){
while (this.cont_chip_.numChildren > 0) {
this.cont_chip_.removeChildAt(0);
}
	}
			bets_[i] = 0;
			for (var t: Number = gameSettings.chip_name.length - 1; t >= 0; t--) {
	        if(b>= gameSettings.chip_value[t]){
			
					temp = b / gameSettings.chip_value[t];
				if(b >= 100000){
					chip_deck(i, t);
				}
				else{
					for (var l: Number = 0; l < Math.floor(temp); l++) {
						chip_deck(i, t);
					}
				}
					b = b % gameSettings.chip_value[t];

				}
			}
		
			setChildIndex(cont_upper, numChildren - 1);
		}
		private function chip_bet(i: Number, t: Number): void {
			
			p.bet_[i][bets_Area[i]] = new Image(chip_texture[t]);
			p.bet_[i][bets_Area[i]].scaleX = .3;
			p.bet_[i][bets_Area[i]].scaleY = .3;
			p.bet_[i][bets_Area[i]].x = bet_upper[i].x + randRange(bets_Min_X[i],bets_Max_X[i]);
			p.bet_[i][bets_Area[i]].y = bet_upper[i].y - 30 +  randRange(bets_Min_Y[i],bets_Max_Y[i]);
			this.addChild(p.bet_[i][bets_Area[i]]);
		
			TweenLite.from(p.bet_[i][bets_Area[i]],.2,{alpha:0,delay:Math.random() * .6 ,y:p.bet_[i][bets_Area[i]].y - 100,ease: Bounce.easeOut});
			bets_Area[i] += 1;
			
		
			}
			
		private function chip_bet_check(i: Number, b: Number): void {
		
			var temp: Number;
			
			for (var t: Number = gameSettings.chip_name.length - 1; t >= 0; t--) {
	        if(b>= gameSettings.chip_value[t]){
			
				temp = b / gameSettings.chip_value[t];
				if(b >= 100000){
					chip_bet(i, t);
				}
				else{
					for (var l: Number = 0; l < Math.floor(temp); l++) {
						chip_bet(i, t);
					}
				}
					b = b % gameSettings.chip_value[t];

				}
			}
		
			setChildIndex(cont_upper, numChildren - 1);
		}
		
		private function randRange(minNum:Number, maxNum:Number):Number 
{
    return (Math.floor(Math.random() * (maxNum - minNum + 1)) + minNum);
}
private function reset(i:Number):void{
	
			var m: Number;
			for (m = 0; m < bets_[i]; m++) {
					this.removeChild(o.bet_[i][m] );
			}
			for (m = 0; m < bets_Area[i]; m++) {
					this.removeChild(p.bet_[i][m] );
			}
			if(i  ==4 && cont_chip_.numChildren !=0){
while (this.cont_chip_.numChildren > 0) {
this.cont_chip_.removeChildAt(0);
}
	}
	this.removeChild(bet_chip_text[i]);
			this.removeChild(bet_chip_add_text[i]);
		bet_text[i].text = 0;
		temp_add_bets[i] = 0;
		bets_Area[i] = 0;
		bets_[i] = 0;
		bets[i] = 0;
	
}

		
private function bet_event():void{
	if(temp_bal != total_balance){	
						total_balance = temp_bal;
						btn_bet_text.text = "提交";
				total_bets = 0;
		
						//remove Child
						if(cont_chip_.numChildren!=0 && bets[4] != temp_bets[4])
						this.cont_chip_.removeChildAt(this.cont_chip_.numChildren -1);
				
								
						for (var i:Number = 0; i < gameSettings.bet_tag.length; i++) {
						this.removeChild(bet_chip_add_text[i]);
						//----write
						temp_tbl[i] += temp_add_bets[i];
							//bet_text[i].text = bets[i];
							//chip_bet_check(i,temp_add_bets[i]);
							temp_add_bets[i] = 0;
							//bets[i] = 0;
						}
						//---
				
								connect_.websocket.sendUTF(JSON.stringify({bet:{0:temp_tbl[0],1:temp_tbl[1],2:temp_tbl[2],3:temp_tbl[3],4:temp_tbl[4],5:temp_tbl[5],6:temp_tbl[6],7:temp_tbl[7],8:temp_tbl[8],9:temp_tbl[9]}}));
					//	connect_.socket.send(JSON.stringify({amount: total_balance}));
						temp_bets = bets.concat();
					
						//---send to server
						//connect_.socket.send(temp_bal);
					//	if(connect_.status_ == "Connected"){
				//total_balance_text.text = connect_.data_ +"ok";
		//	} 
						
					}
					else{
					//	trace("not");
					}
}

private function clear_event():void{
	temp_bal = total_balance;
				total_balance_text.text = total_balance.toString();
				btn_bet_text.text = "提交";
				total_bets = 0;
				//temp_bets_ 
				for (var i:Number = 0; i < bets_.length; i++) {
						bets[i]  = temp_bets[i];
						temp_add_bets[i] = 0 ;
						chip_deck_check(i, temp_bets[i]);
						if(bets[i] == 0)
						this.removeChild(bet_chip_text[i]);
						this.removeChild(bet_chip_add_text[i]);
						bet_text[i].text = temp_live[i];
						bets_Area[i] = 0;
						
				}
				

				
			if(cont_chip_.numChildren!=0 )
						this.cont_chip_.removeChildAt(this.cont_chip_.numChildren -1);
}

private function auto_event():void{
		if(auto_submit){
						auto_submit = false;
						btn_bet.visible = true;
					btn_clear.visible = true;
					btn_auto.visible = true;
						btn_bet_lock.visible = false;
						btn_clear_lock.visible = false;
						btn_auto_lock.visible = false;
					//TweenMax.staggerTo([btn_bet,btn_clear,btn_auto],.3,{alpha:0});
					//	TweenMax.staggerTo([btn_bet_lock,btn_clear_lock,btn_auto_lock],.3,{alpha:1,ease: Elastic.easeOut});
					}
					else{
						auto_submit = true;
						
						
						btn_bet.visible = false;
						btn_clear.visible = false;
						btn_auto.visible = false;
						btn_bet_lock.visible = true;
						btn_clear_lock.visible = true;
						btn_auto_lock.visible = true;
					}
}
	


	

	//------
	}
}
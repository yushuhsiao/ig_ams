package screens {

	import events.GameSettings;
	import events.GameAnimation;

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
	


	import com.greensock.plugins.ColorMatrixFilterPlugin;

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
		private var btn_bet: Image;
		private var btn_clear: Image;
		private var btn_auto: Image;

		public var btn_next: Image;
		public var btn_back: Image;

		//Chip Settings

		public var cont_selection: Sprite = new Sprite();

		public var chip: Vector.<Image>= new Vector.<Image>();
		public var bet_chip: Vector.<Image>= new Vector.<Image>();
		public var bet_chip_mouse: Vector.<Image>= new Vector.<Image>();
		private var chip_texture: Vector.<Texture>= new Vector.<Texture>();
		private var bet_chip_texture: Vector.<Texture>= new Vector.<Texture>();
private var cont_chip_: Vector.<Sprite>= new Vector.<Sprite>(10);

		//private var cont_chip0: Sprite = new Sprite();
	
		//private var cs: Object = {chip_:{0:Sprite,1:Sprite,2:Sprite,3:Sprite,4:Sprite,5:Sprite,6:Sprite,7:Sprite,8:Sprite,9:Sprite}};


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
		private var bd: Array = new Array(0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
		private var o: Object = {bet_:{0:{0:Vector.<Image>},1:{1:Vector.<Image>},2:{2:Vector.<Image>},3:{3:Vector.<Image>},4:{4:Vector.<Image>},5:{5:Vector.<Image>},6:{6:Vector.<Image>},7:{7:Vector.<Image>},8:{8:Vector.<Image>},9:{9:Vector.<Image>}}};

		private var bet_text:Array = new Array();
		private var bet_chip_text:Array = new Array();
		private var bet_chip_add_text:Array = new Array();
		private var bets_loc: Array = new Array(-20, -18, -13, -8, 0, 20, 5, 5, 10, 15);
		private var bets_chip_loc: Array = new Array(35, 25, 20, 23, 7, 7, 7, 7, 10, 12);
		//Classes
		private var gameSettings: GameSettings = new GameSettings();
		private var gameAnimation: GameAnimation = new GameAnimation();

		private var total_bets: Number = 0;
		public function Game() {
			super();
			this.addEventListener(Event.ADDED_TO_STAGE, onAddedToStage);
		}

		private function onAddedToStage(event: Event): void {
			this.removeEventListener(Event.ADDED_TO_STAGE, onAddedToStage);
			drawStage();
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

			//Main Buttons
			var texture_bet: Texture = Assets.getAtlas().getTexture("btn_bet instance 10000")
			btn_bet = new Image(texture_bet);
			btn_bet.height -= 20;
			btn_bet.width -= 50;
			btn_bet.x = cont_chip.x;
			btn_bet.y = stage.stageHeight - btn_bet.height - 5;
			cont_select.addChild(btn_bet);

			var texture_clear: Texture = Assets.getAtlas().getTexture("btn_clear instance 10000");
			btn_clear = new Image(texture_clear);
			btn_clear.height -= 20;
			btn_clear.width -= 50;
			btn_clear.x = btn_bet.x + btn_bet.width + 5;
			btn_clear.y = stage.stageHeight - btn_clear.height - 5;
			cont_select.addChild(btn_clear);

			var texture_auto: Texture = Assets.getAtlas().getTexture("btn_auto instance 10000");
			btn_auto = new Image(texture_auto);
			btn_auto.height -= 20;
			btn_auto.width -= 50;
			btn_auto.x = btn_clear.x + btn_clear.width + 5;
			btn_auto.y = stage.stageHeight - btn_auto.height - 5;
			cont_select.addChild(btn_auto);

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
						y: chip[i].y - 10,
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
						y: chip[i].y + 10,
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
							y: chip[i].y - 10,
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

						ease: Elastic.easeOut
					});


				} else {
					if (gameSettings.chip_selected != gameSettings.chip_value[i])
					// rollout code goes here
						TweenLite.to(chip[i], .5, {
							scaleX: gameSettings.chip_scale,
							scaleY: gameSettings.chip_scale,

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
			if (e.getTouch(btn_clear, TouchPhase.BEGAN)) {

				for (i = 0; i < bets_.length; i++) {
					for (var m: Number = 0; m < bets_[i]; m++) {
						this.removeChild(o.bet_[i][m]);
						
					}
					bets[i] = 0;
					this.removeChild(bet_chip_text[i]);
						this.removeChild(bet_chip_add_text[i]);
				}
				total_bets = 0;
				bets_ = [0, 0, 0, 0, 0, 0, 0, 0, 0, 0];
				//while (cont_chip_[i].numChildren -1> 0) {
 // cont_chip_[i].removeChildAt(0);
//}
//cont_chip_[i].removeChildren();
		//	this.removeChild(cont_chip_[i]);


			}



			for (i = 0; i < gameSettings.bet_tag.length; i++) {
				if (e.getTouch(bet_upper[i], TouchPhase.BEGAN)) {
					cont_chip_[i] = new Sprite();
					trace(i);
				
					for (t = 0; t < gameSettings.chip_name.length; t++) {
						if (gameSettings.chip_selected == gameSettings.chip_value[t]) {
							chip_deck(i, t);
							bets[i] += gameSettings.chip_selected;
							total_bets += gameSettings.chip_selected;
							bet_text[i].text = bets[i];
							trace(bets[i] + " - total= " + total_bets);
						}
					}
						chip_deck_check(i, bets[i]);
					
				}
			}
		}
		private function chip_deck(i: Number, t: Number): void {
			//trace(cs.chip_[i]);
			this.removeChild(bet_chip_text[i]);
			this.removeChild(bet_chip_add_text[i]);
			//cont_chip_[i] = new Sprite();
			o.bet_[i][bets_[i]] = new Image(bet_chip_texture[t]);
			o.bet_[i][bets_[i]].scaleX = .6;
			o.bet_[i][bets_[i]].scaleY = .6;
			o.bet_[i][bets_[i]].x = bet_upper[i].x - (bet_upper[i].width / 2) +  bets_chip_loc[i];
			o.bet_[i][bets_[i]].y = bet_upper[i].y - bet_upper[i].height - (bets_[i] * 3);
			this.addChild(o.bet_[i][bets_[i]]);
				bet_chip_text[i] = new TextField(100,20,"","BERNHC",12);
				bet_chip_text[i].color = "0x000000";
				bet_chip_text[i].alignPivot("center", "center");
			bet_chip_text[i].x = o.bet_[i][bets_[i]].x + (o.bet_[i][bets_[i]].width/2);
			bet_chip_text[i].y =o.bet_[i][bets_[i]].y + (o.bet_[i][bets_[i]].height/2);
			bet_chip_text[i].text = bets[i];
		this.addChild(bet_chip_text[i]);
		bet_chip_add_text[i] = new TextField(100,20,"","BERNHC",14);
				bet_chip_add_text[i].color = "0xFFFF00";
				bet_chip_add_text[i].alignPivot("center", "center");
			bet_chip_add_text[i].x = o.bet_[i][bets_[i]].x + (o.bet_[i][bets_[i]].width/2);
			bet_chip_add_text[i].y =o.bet_[i][bets_[i]].y - (o.bet_[i][bets_[i]].height/4) ;
			bet_chip_add_text[i].text = "+"+bets[i];
		this.addChild(bet_chip_add_text[i]);
			//this.addChild(cont_chip_[i]);
			//TweenLite.from(o.bet_[i][bets_[i]],.2,{y:o.bet_[i][bets_[i]].y - 3 });
			bets_[i] += 1;
		
			

		}


		private function chip_deck_check(i: Number, b: Number): void {
		
			var temp: Number;
			var m: Number;
			for (m = 0; m < bets_[i]; m++) {
					this.removeChild(o.bet_[i][m] );
			}
			this.removeChild(bet_chip_add_text[i]);
				this.removeChild(bet_chip_text[i]);
			bets_[i] = 0;
			for (var t: Number = gameSettings.chip_name.length - 1; t >= 0; t--) {
	        if(b>= gameSettings.chip_value[t]){
				//if ((b % gameSettings.chip_value[t]) == 0 && b != 0) {
				
					temp = b / gameSettings.chip_value[t];
				if(b >= 100000){
					chip_deck(i, t);
				}else{
					for (var l: Number = 0; l < Math.floor(temp); l++) {
						chip_deck(i, t);
					}
				}

					b = b % gameSettings.chip_value[t];

				}

			}
			setChildIndex(cont_upper, numChildren - 1);
		}

	//------
	}
}
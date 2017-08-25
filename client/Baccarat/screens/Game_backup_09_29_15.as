package screens {

	import flash.geom.Point;

	import com.greensock.*;
	import com.greensock.easing.*;
	import flash.events.MouseEvent;
	import starling.core.Starling;
	import starling.display.Sprite;
	import starling.display.Image;
	import starling.textures.Texture;

	import starling.events.Event;
	import starling.events.Touch;
	import starling.events.TouchPhase;
	import starling.events.TouchEvent;

	import com.greensock.plugins.*;

	import starling.filters.BlurFilter;
	import starling.display.MovieClip;

	//import flash.filters.GlowFilter;


	TweenPlugin.activate([GlowFilterPlugin]);


	public class Game extends Sprite {

		private var bg: Image;
		private var cont_bg: Image;
		private var cont_1: Image;
		private var cont_3: Image;

		private var cont_cash: Image;
		private var cont_mask: Image;
		private var cont_coin: Image;
		private var cont_2_: Image;

		private var btn_bet: Image;
		private var btn_clear: Image;
		private var btn_auto: Image;

		private var btn_next: Image;
		private var btn_back: Image;

		private var cont_bettingArea: Image;

		private var coin_1: Image;
		private var coin_5: Image;
		private var coin_10: Image;
		private var coin_50: Image;
		private var coin_100: Image;
		private var coin_250: Image;
		private var coin_500: Image;
		private var coin_1000: Image;
		private var coin_2500: Image;
		private var coin_5000: Image;
		private var coin_10000: Image;
		private var coin_25000: Image;
		private var coin_50000: Image;
		private var coin_100000: Image;

		private var cont: Sprite = new Sprite();
		

		//Chip Settings
		public var chip_selected: Number = 10;
		private var chip: Vector.<Image> = new Vector.<Image>();
		private var chip_texture: Vector.<Texture> = new Vector.<Texture>();
		private var chip_name: Array = new Array("chip_1 instance 10000", "chip_5 instance 10000", "chip_10 instance 10000", "chip_50 instance 10000", "chip_100 instance 10000", "chip_250 instance 10000", "chip_500 instance 10000", "chip_1000 instance 10000", "chip_2500 instance 10000", "chip_5000 instance 10000", "chip_10000 instance 10000", "chip_20000 instance 10000", "chip_25000 instance 10000", "chip_50000 instance 10000", "chip_100000 instance 10000")
			private var chip_value: Array = new Array(1, 5, 10, 50, 100, 250, 500, 1000, 2500, 5000, 10000, 20000, 25000, 50000, 100000);
		private var chip_scale: Number = .7;
		private var chip_scale_anim: Number = .8;

		public function Game() {

			super();
			this.addEventListener(Event.ADDED_TO_STAGE, onAddedToStage);
		}

		private function onAddedToStage(event: Event): void {
			this.removeEventListener(Event.ADDED_TO_STAGE, onAddedToStage);
			drawGame();
		}

		private function drawGame(): void {

			bg = new Image(Assets.getTexture("BgLayer"));
			bg.width = stage.stageWidth;
			bg.height = stage.stageHeight;
			this.addChild(bg);

			cont_bg = new Image(Assets.getAtlas().getTexture("cont_bg instance 10000"));
			cont_bg.height = cont_bg.height - 90;
			cont_bg.width = stage.stageWidth;
			cont_bg.x = 0;
			cont_bg.y = stage.stageHeight - cont_bg.height;
			this.addChild(cont_bg);

			cont_1 = new Image(Assets.getAtlas().getTexture("cont_1 instance 10000"));
			cont_1.height = cont_bg.height;
			cont_1.width = cont_1.width - 180;
			cont_1.x = 0;
			cont_1.y = stage.stageHeight - cont_1.height;
			this.addChild(cont_1);

			cont_3 = new Image(Assets.getAtlas().getTexture("cont_3 instance 10000"));
			cont_3.height = cont_bg.height;
			cont_3.width = cont_3.width - 200;
			cont_3.x = stage.stageWidth - cont_3.width;
			cont_3.y = stage.stageHeight - cont_3.height;
			this.addChild(cont_3);

			cont_coin = new Image(Assets.getAtlas().getTexture("cont_coin instance 10000"));
			cont_coin.height = cont_coin.height - 30;
			cont_coin.width = cont_coin.width - 160;
			cont_coin.x = 365;
			cont_coin.y = stage.stageHeight - 90;
			this.addChild(cont_coin);

			cont_mask = new Image(Assets.getAtlas().getTexture("cont_coin instance 10000"));
			cont_mask.height = cont_mask.height;
			cont_mask.width = cont_mask.width - 190;
			cont_mask.x = 380;
			cont_mask.y = stage.stageHeight - 110;

			this.addChild(cont_mask);


			drawCoin();


			cont_cash = new Image(Assets.getAtlas().getTexture("cont_cash instance 10000"));
			cont_cash.height -= 13;
			cont_cash.width -= 110;
			cont_cash.x = cont_coin.x - 7;
			cont_cash.y = cont_bg.y + 2;
			this.addChild(cont_cash);


			cont_2_ = new Image(Assets.getAtlas().getTexture("cont_2_ instance 10000"));
			cont_2_.height = cont_bg.height - 5;
			cont_2_.width = cont_2_.width - 20;
			cont_2_.x = cont_1.width;
			cont_2_.y = stage.stageHeight - cont_2_.height;
			this.addChild(cont_2_);

			//Buttons
			btn_bet = new Image(Assets.getAtlas().getTexture("btn_bet instance 10000"));
			btn_bet.height -= 20;
			btn_bet.width -= 50;
			btn_bet.x = cont_coin.x;
			btn_bet.y = stage.stageHeight - btn_bet.height - 5;
			this.addChild(btn_bet);

			btn_clear = new Image(Assets.getAtlas().getTexture("btn_clear instance 10000"));
			btn_clear.height -= 20;
			btn_clear.width -= 50;
			btn_clear.x = btn_bet.x + btn_bet.width + 5;
			btn_clear.y = stage.stageHeight - btn_clear.height - 5;
			this.addChild(btn_clear);

			btn_auto = new Image(Assets.getAtlas().getTexture("btn_auto instance 10000"));
			btn_auto.height -= 20;
			btn_auto.width -= 50;
			btn_auto.x = btn_clear.x + btn_clear.width + 5;
			btn_auto.y = stage.stageHeight - btn_auto.height - 5;
			this.addChild(btn_auto);


			//Betting Area
			cont_bettingArea = new Image(Assets.getAtlas().getTexture("cont_bettingArea instance 10000"));
			cont_bettingArea.height -= 30;
			cont_bettingArea.width -= 120;
			cont_bettingArea.x = cont_1.x + 30;
			cont_bettingArea.y = cont_bg.y - cont_bg.height + 50;
			this.addChild(cont_bettingArea);


		}
		private function drawCoin(): void {

			var texture_back: Texture = Assets.getAtlas().getTexture("btn_back instance 10000");
			btn_back = new Image(texture_back);
			btn_back.alignPivot("right", "center");
			btn_back.scaleX = .8;
			btn_back.scaleY = .8;
			btn_back.x = cont_coin.x + 15;
			btn_back.y = cont_coin.y + 22;
			this.addChild(btn_back);
			//coin[0]= btn_back;
			//coin[0].x = 100;
			//coin[0].y = 100; ;)
			//	this.addChild(coin[0]);
			var texture_next: Texture = Assets.getAtlas().getTexture("btn_next instance 10000");
			btn_next = new Image(texture_next);
			btn_next.alignPivot("left", "center");
			btn_next.scaleX = .8;
			btn_next.scaleY = .8;
			btn_next.x = cont_coin.x + cont_coin.width - 15;
			btn_next.y = cont_coin.y + 22;
			this.addChild(btn_next);

			
			for(var i:Number  = 0 ;i< chip_name.length;i++){
			chip_texture[i] =  Assets.getAtlas().getTexture(chip_name[i]);
			chip[i] = new Image(chip_texture[i]);
			chip[i].alignPivot("center", "center");
			chip[i].scaleX = chip_scale;			
			chip[i].scaleY = chip_scale;
				if(i == 0){
				chip[i].x = cont_coin.x + 47;
				chip[i].y = cont_coin.y + 23;
				}
				else{
					chip[i].x = chip[i  - 1].x + 55;
				chip[i].y = chip[i - 1].y;
				}
				cont.addChild(chip[i]);
			}
			/*
			var texture_1: Texture = Assets.getAtlas().getTexture("coin_1 instance 10000");
			coin_1 = new Image(texture_1);
			coin_1.alignPivot("center", "center");
			coin_1.scaleX = .7;
			coin_1.scaleY = .7;
			coin_1.x = cont_coin.x + 47;
			coin_1.y = cont_coin.y + 23;
			cont.addChild(coin_1);


			var texture_2: Texture = Assets.getAtlas().getTexture("coin_5 instance 10000");
			coin_5 = new Image(texture_2);
			coin_5.alignPivot("center", "center");
			coin_5.scaleX = .7;
			coin_5.scaleY = .7;
			coin_5.x = coin_1.x + 55;
			coin_5.y = coin_1.y;
			cont.addChild(coin_5);


			var texture_3: Texture = Assets.getAtlas().getTexture("coin_10 instance 10000");
			coin_10 = new Image(texture_3);
			coin_10.alignPivot("center", "center");
			coin_10.scaleX = .7;
			coin_10.scaleY = .7;
			coin_10.x = coin_5.x + 55;
			coin_10.y = coin_5.y;
			cont.addChild(coin_10);


			var texture_4: Texture = Assets.getAtlas().getTexture("coin_50 instance 10000");
			coin_50 = new Image(texture_4);
			coin_50.alignPivot("center", "center");
			coin_50.scaleX = .7;
			coin_50.scaleY = .7;
			coin_50.x = coin_10.x + 55;
			coin_50.y = coin_10.y;
			cont.addChild(coin_50);

			var texture_5: Texture = Assets.getAtlas().getTexture("coin_100 instance 10000")
			coin_100 = new Image(texture_5);
			coin_100.alignPivot("center", "center");
			coin_100.scaleX = .7;
			coin_100.scaleY = .7;
			coin_100.x = coin_50.x + 55;
			coin_100.y = coin_50.y;
			cont.addChild(coin_100);

			var texture_6: Texture = Assets.getAtlas().getTexture("coin_250 instance 10000");
			coin_250 = new Image(texture_6);
			coin_250.alignPivot("center", "center");
			coin_250.scaleX = .7;
			coin_250.scaleY = .7;
			coin_250.x = coin_100.x + 55;
			coin_250.y = coin_100.y;
			cont.addChild(coin_250);

			var texture_7: Texture = Assets.getAtlas().getTexture("coin_500 instance 10000")
			coin_500 = new Image(texture_7);
			coin_500.alignPivot("center", "center");
			coin_500.scaleX = .7;
			coin_500.scaleY = .7;
			coin_500.x = coin_250.x + 55;
			coin_500.y = coin_250.y;
			cont.addChild(coin_500);

			var texture_8: Texture = Assets.getAtlas().getTexture("coin_1000 instance 10000");
			coin_1000 = new Image(texture_8);
			coin_1000.alignPivot("center", "center");
			coin_1000.scaleX = .7;
			coin_1000.scaleY = .7;
			coin_1000.x = coin_500.x + 55;
			coin_1000.y = coin_500.y;
			cont.addChild(coin_1000);

			var texture_9: Texture = Assets.getAtlas().getTexture("coin_2500 instance 10000");
			coin_2500 = new Image(texture_9);
			coin_2500.alignPivot("center", "center");
			coin_2500.scaleX = .7;
			coin_2500.scaleY = .7;
			coin_2500.x = coin_1000.x + 55;
			coin_2500.y = coin_1000.y;
			cont.addChild(coin_2500);


			var texture_10: Texture = Assets.getAtlas().getTexture("coin_5000 instance 10000");
			coin_5000 = new Image(texture_10);
			coin_5000.alignPivot("center", "center");
			coin_5000.scaleX = .7;
			coin_5000.scaleY = .7;
			coin_5000.x = coin_2500.x + 55;
			coin_5000.y = coin_2500.y;
			cont.addChild(coin_5000);


			var texture_11: Texture = Assets.getAtlas().getTexture("coin_10000 instance 10000");
			coin_10000 = new Image(texture_11);
			coin_10000.alignPivot("center", "center");
			coin_10000.scaleX = .7;
			coin_10000.scaleY = .7;
			coin_10000.x = coin_5000.x + 55;
			coin_10000.y = coin_5000.y;
			cont.addChild(coin_10000);


			var texture_12: Texture = Assets.getAtlas().getTexture("coin_25000 instance 10000");
			coin_25000 = new Image(texture_12);
			coin_25000.alignPivot("center", "center");
			coin_25000.scaleX = .7;
			coin_25000.scaleY = .7;
			coin_25000.x = coin_10000.x + 55;
			coin_25000.y = coin_10000.y;
			cont.addChild(coin_25000);


			var texture_13: Texture = Assets.getAtlas().getTexture("coin_50000 instance 10000");
			coin_50000 = new Image(texture_13);
			coin_50000.alignPivot("center", "center");
			coin_50000.scaleX = .7;
			coin_50000.scaleY = .7;
			coin_50000.x = coin_25000.x + 55;
			coin_50000.y = coin_25000.y;
			cont.addChild(coin_50000);


			var texture_14: Texture = Assets.getAtlas().getTexture("coin_100000 instance 10000")
			coin_100000 = new Image(texture_14);
			coin_100000.alignPivot("center", "center");
			coin_100000.scaleX = .7;
			coin_100000.scaleY = .7;
			coin_100000.x = coin_50000.x + 55;
			coin_100000.y = coin_50000.y;
			cont.addChild(coin_100000);
*/

			addChild(cont);
			cont.mask = cont_mask;
			drawCoinAnimation();
		}

		private function drawCoinAnimation(): void {
			stage.addEventListener(TouchEvent.TOUCH, onTouchAnimation);
			stage.addEventListener(TouchEvent.TOUCH, onTouch);
		}
		private function onTouchBackAnim(): void {
				for(var i:Number  = 0 ;i< chip_name.length;i++){
						if (chip_selected == chip_value[i])
				TweenMax.to(chip[i], .3, {
					scaleX: chip_scale,
					scaleY: chip_scale,
					y: chip[i].y + 10,
					ease: Linear.easeOut
				});
				}
				
				/*
			if (coin_selected == 1)
				TweenMax.to(coin_1, .3, {
					scaleX: .7,
					scaleY: .7,
					y: coin_1.y + 10,
					ease: Bounce.easeOut
				});
			else if (coin_selected == 5)
				TweenMax.to(coin_5, .3, {
					scaleX: .7,
					scaleY: .7,
					y: coin_5.y + 10,
					ease: Bounce.easeOut
				});
			else if (coin_selected == 10)
				TweenMax.to(coin_10, .3, {
					scaleX: .7,
					scaleY: .7,
					y: coin_10.y + 10,
					ease: Bounce.easeOut
				});
			else if (coin_selected == 50)
				TweenMax.to(coin_50, .3, {
					scaleX: .7,
					scaleY: .7,
					y: coin_50.y + 10,
					ease: Bounce.easeOut
				});
			else if (coin_selected == 100)
				TweenMax.to(coin_100, .3, {
					scaleX: .7,
					scaleY: .7,
					y: coin_100.y + 10,
					ease: Bounce.easeOut
				});
			else if (coin_selected == 250)
				TweenMax.to(coin_250, .3, {
					scaleX: .7,
					scaleY: .7,
					y: coin_250.y + 10,
					ease: Bounce.easeOut
				});
			else if (coin_selected == 500)
				TweenMax.to(coin_500, .3, {
					scaleX: .7,
					scaleY: .7,
					y: coin_500.y + 10,
					ease: Bounce.easeOut
				});
			else if (coin_selected == 1000)
				TweenMax.to(coin_1000, .3, {
					scaleX: .7,
					scaleY: .7,
					y: coin_1000.y + 10,
					ease: Bounce.easeOut
				});
			else if (coin_selected == 2500)
				TweenMax.to(coin_2500, .3, {
					scaleX: .7,
					scaleY: .7,
					y: coin_2500.y + 10,
					ease: Bounce.easeOut
				});
			else if (coin_selected == 5000)
				TweenMax.to(coin_5000, .3, {
					scaleX: .7,
					scaleY: .7,
					y: coin_5000.y + 10,
					ease: Bounce.easeOut
				});
			else if (coin_selected == 10000)
				TweenMax.to(coin_10000, .3, {
					scaleX: .7,
					scaleY: .7,
					y: coin_10000.y + 10,
					ease: Bounce.easeOut
				});
			else if (coin_selected == 25000)
				TweenMax.to(coin_25000, .3, {
					scaleX: .7,
					scaleY: .7,
					y: coin_25000.y + 10,
					ease: Bounce.easeOut
				});
			else if (coin_selected == 50000)
				TweenMax.to(coin_50000, .3, {
					scaleX: .7,
					scaleY: .7,
					y: coin_50000.y + 10,
					ease: Bounce.easeOut
				});
			else if (coin_selected == 100000)
				TweenMax.to(coin_100000, .3, {
					scaleX: .7,
					scaleY: .7,
					y: coin_100000.y + 10,
					ease: Bounce.easeOut
				});
			else {}*/
		}
		private function onTouch(e: TouchEvent): void {
			
				for(var i:Number  = 0 ;i< chip_name.length;i++){
					if (e.getTouch(chip[i], TouchPhase.BEGAN)) {
				if (chip_selected != chip_value[i]) {
					onTouchBackAnim();
					TweenMax.to(chip[i], .5, {
						scaleX: chip_scale_anim + .1,
						scaleY: chip_scale_anim + .1,
						y: chip[i].y - 10,
						ease: Bounce.easeOut
					});
					chip_selected = chip_value[i];
				}
			}
				}
				/*
				
				
			if (e.getTouch(coin_1, TouchPhase.BEGAN)) {
				if (coin_selected != 1) {
					onTouchBackAnim();
					TweenMax.to(coin_1, .5, {
						scaleX: .9,
						scaleY: .9,
						y: coin_1.y - 10,
						ease: Bounce.easeOut
					});
					coin_selected = 1;
				}
			}
			if (e.getTouch(coin_5, TouchPhase.BEGAN)) {
				if (coin_selected != 5) {
					onTouchBackAnim();
					TweenMax.to(coin_5, .5, {
						scaleX: .9,
						scaleY: .9,
						y: coin_5.y - 10,
						ease: Bounce.easeOut
					});
					coin_selected = 5;
				}
			}
			if (e.getTouch(coin_10, TouchPhase.BEGAN)) {
				if (coin_selected != 10) {
					onTouchBackAnim();
					TweenMax.to(coin_10, .5, {
						scaleX: .9,
						scaleY: .9,
						y: coin_10.y - 10,
						ease: Bounce.easeOut
					});
					coin_selected = 10;
				}
			}
			if (e.getTouch(coin_50, TouchPhase.BEGAN)) {
				if (coin_selected != 50) {
					onTouchBackAnim();
					TweenMax.to(coin_50, .5, {
						scaleX: .9,
						scaleY: .9,
						y: coin_50.y - 10,
						ease: Bounce.easeOut
					});
					coin_selected = 50;
				}
			}
			if (e.getTouch(coin_100, TouchPhase.BEGAN)) {
				if (coin_selected != 100) {
					onTouchBackAnim();
					TweenMax.to(coin_100, .5, {
						scaleX: .9,
						scaleY: .9,
						y: coin_100.y - 10,
						ease: Bounce.easeOut
					});
					coin_selected = 100;
				}
			}
			if (e.getTouch(coin_250, TouchPhase.BEGAN)) {
				if (coin_selected != 250) {
					onTouchBackAnim();
					TweenMax.to(coin_250, .5, {
						scaleX: .9,
						scaleY: .9,
						y: coin_250.y - 10,
						ease: Bounce.easeOut
					});
					coin_selected = 250;
				}
			}
			if (e.getTouch(coin_500, TouchPhase.BEGAN)) {
				if (coin_selected != 500) {
					onTouchBackAnim();
					TweenMax.to(coin_500, .5, {
						scaleX: .9,
						scaleY: .9,
						y: coin_500.y - 10,
						ease: Bounce.easeOut
					});
					coin_selected = 500;
				}
			}

			if (e.getTouch(coin_1000, TouchPhase.BEGAN)) {
				if (coin_selected != 1000) {
					onTouchBackAnim();
					TweenMax.to(coin_1000, .5, {
						scaleX: .9,
						scaleY: .9,
						y: coin_1000.y - 10,
						ease: Bounce.easeOut
					});
					coin_selected = 1000;
				}
			}
			if (e.getTouch(coin_2500, TouchPhase.BEGAN)) {
				if (coin_selected != 2500) {
					onTouchBackAnim();
					TweenMax.to(coin_2500, .5, {
						scaleX: .9,
						scaleY: .9,
						y: coin_2500.y - 10,
						ease: Bounce.easeOut
					});
					coin_selected = 2500;
				}
			}
			if (e.getTouch(coin_5000, TouchPhase.BEGAN)) {
				if (coin_selected != 5000) {
					onTouchBackAnim();
					TweenMax.to(coin_5000, .5, {
						scaleX: .9,
						scaleY: .9,
						y: coin_5000.y - 10,
						ease: Bounce.easeOut
					});
					coin_selected = 5000;
				}
			}
			if (e.getTouch(coin_10000, TouchPhase.BEGAN)) {
				if (coin_selected != 10000) {
					onTouchBackAnim();
					TweenMax.to(coin_10000, .5, {
						scaleX: .9,
						scaleY: .9,
						y: coin_10000.y - 10,
						ease: Bounce.easeOut
					});
					coin_selected = 10000;
				}
			}
			if (e.getTouch(coin_25000, TouchPhase.BEGAN)) {
				if (coin_selected != 25000) {
					onTouchBackAnim();
					TweenMax.to(coin_25000, .5, {
						scaleX: .9,
						scaleY: .9,
						y: coin_25000.y - 10,
						ease: Bounce.easeOut
					});
					coin_selected = 25000;
				}
			}
			if (e.getTouch(coin_50000, TouchPhase.BEGAN)) {
				if (coin_selected != 50000) {
					onTouchBackAnim();
					TweenMax.to(coin_50000, .5, {
						scaleX: .9,
						scaleY: .9,
						y: coin_50000.y - 10,
						ease: Bounce.easeOut
					});
					coin_selected = 50000;
				}
			}
			if (e.getTouch(coin_100000, TouchPhase.BEGAN)) {
				if (coin_selected != 100000) {
					onTouchBackAnim();
					TweenMax.to(coin_100000, .5, {
						scaleX: .9,
						scaleY: .9,
						y: coin_100000.y - 10,
						ease: Bounce.easeOut
					});
					coin_selected = 100000;
				}
			}

		*/
			if (e.getTouch(btn_next, TouchPhase.BEGAN)) {
				// click code goes here
				if (cont.x > -550) {
					TweenMax.to(cont, .5, {
						x: cont.x - 50
					});
				} else {
					TweenMax.to(cont, .5, {
						x: -550,
						ease: Elastic.easeOut
					});
				}
			}

			if (e.getTouch(btn_back, TouchPhase.BEGAN)) {

				// click code goes here
				if (cont.x < 0) {
					TweenMax.to(cont, .5, {
						x: cont.x + 50
					});
				} else {
					TweenMax.to(cont, .5, {
						x: 0,
						ease: Elastic.easeOut
					});
				}
			}

		}
		private function onTouchAnimation(e: TouchEvent): void {

for(var i:Number  = 0 ;i< chip_name.length;i++){
	if (e.getTouch(chip[i], TouchPhase.HOVER)) {
				// rollover code goes here
				TweenLite.to(chip[i], .8, {
					scaleX: chip_scale_anim,
					scaleY: chip_scale_anim,
					ease: Elastic.easeOut
				});
			} else {
				if (chip_selected != chip_value[i])
				// rollout code goes here
					TweenLite.to(chip[i], .5, {
						scaleX: chip_scale,
						scaleY: chip_scale,
						ease: Elastic.easeOut
					});

			}
	
}/*
			if (e.getTouch(coin_1, TouchPhase.HOVER)) {
				// rollover code goes here
				TweenLite.to(coin_1, 2, {
					scaleX: .8,
					scaleY: .8,
					ease: Elastic.easeOut
				});
			} else {
				if (coin_selected != 1)
				// rollout code goes here
					TweenLite.to(coin_1, 1, {
						scaleX: .7,
						scaleY: .7,
						//		y: coin_1.y + 10,
						ease: Elastic.easeOut
					});

			}

			if (e.getTouch(coin_5, TouchPhase.HOVER)) {
				// rollover code goes here
				TweenLite.to(coin_5, 2, {
					scaleX: .8,
					scaleY: .8,
					ease: Elastic.easeOut
				});
			} else {
				if (coin_selected != 5)
				// rollout code goes here
					TweenLite.to(coin_5, 1, {
						scaleX: .7,
						scaleY: .7,
						//	y: coin_5.y + 10,
						ease: Elastic.easeOut
					});
			}

			if (e.getTouch(coin_10, TouchPhase.HOVER)) {
				// rollover code goes here
				TweenLite.to(coin_10, 2, {
					scaleX: .8,
					scaleY: .8,
					ease: Elastic.easeOut
				});
			} else {
				if (coin_selected != 10)
				// rollout code goes here
					TweenLite.to(coin_10, 1, {
						scaleX: .7,
						scaleY: .7,
						//	y: coin_10.y + 10,
						ease: Elastic.easeOut
					});
			}


			if (e.getTouch(coin_50, TouchPhase.HOVER)) {
				// rollover code goes here
				TweenLite.to(coin_50, 2, {
					scaleX: .8,
					scaleY: .8,
					ease: Elastic.easeOut
				});
			} else {
				if (coin_selected != 50)
				// rollout code goes here
					TweenLite.to(coin_50, 1, {
						scaleX: .7,
						scaleY: .7,
						//	y: coin_50.y + 10,
						ease: Elastic.easeOut
					});
			}

			if (e.getTouch(coin_100, TouchPhase.HOVER)) {
				// rollover code goes here
				TweenLite.to(coin_100, 2, {
					scaleX: .8,
					scaleY: .8,
					ease: Elastic.easeOut
				});
			} else {
				if (coin_selected != 100)
				// rollout code goes here
					TweenLite.to(coin_100, 1, {
						scaleX: .7,
						scaleY: .7,
						//	y: coin_100.y + 10,
						ease: Elastic.easeOut
					});
			}
			if (e.getTouch(coin_250, TouchPhase.HOVER)) {
				// rollover code goes here
				TweenLite.to(coin_250, 2, {
					scaleX: .8,
					scaleY: .8,
					ease: Elastic.easeOut
				});
			} else {
				if (coin_selected != 250)
				// rollout code goes here
					TweenLite.to(coin_250, 1, {
						scaleX: .7,
						scaleY: .7,
						//	y: coin_250.y + 10,
						ease: Elastic.easeOut
					});
			}

			if (e.getTouch(coin_500, TouchPhase.HOVER)) {
				// rollover code goes here
				TweenLite.to(coin_500, 2, {
					scaleX: .8,
					scaleY: .8,
					ease: Elastic.easeOut
				});
			} else {
				if (coin_selected != 500)
				// rollout code goes here
					TweenLite.to(coin_500, 1, {
						scaleX: .7,
						scaleY: .7,
						//	y: coin_500.y + 10,
						ease: Elastic.easeOut
					});
			}
			if (e.getTouch(coin_1000, TouchPhase.HOVER)) {
				// rollover code goes here
				TweenLite.to(coin_1000, 2, {
					scaleX: .8,
					scaleY: .8,
					ease: Elastic.easeOut
				});
			} else {
				if (coin_selected != 1000)
				// rollout code goes here
					TweenLite.to(coin_1000, 1, {
						scaleX: .7,
						scaleY: .7,
						//	y: coin_1000.y + 10,
						ease: Elastic.easeOut
					});
			}

			if (e.getTouch(coin_2500, TouchPhase.HOVER)) {
				// rollover code goes here
				TweenLite.to(coin_2500, 2, {
					scaleX: .8,
					scaleY: .8,
					ease: Elastic.easeOut
				});
			} else {
				if (coin_selected != 2500)
				// rollout code goes here
					TweenLite.to(coin_2500, 1, {
						scaleX: .7,
						scaleY: .7,
						//	y: coin_2500.y + 10,
						ease: Elastic.easeOut
					});
			}

			if (e.getTouch(coin_5000, TouchPhase.HOVER)) {
				// rollover code goes here
				TweenLite.to(coin_5000, 2, {
					scaleX: .8,
					scaleY: .8,
					ease: Elastic.easeOut
				});
			} else {
				if (coin_selected != 5000)
				// rollout code goes here
					TweenLite.to(coin_5000, 1, {
						scaleX: .7,
						scaleY: .7,
						//	y: coin_5000.y + 10,
						ease: Elastic.easeOut
					});
			}
			if (e.getTouch(coin_10000, TouchPhase.HOVER)) {
				// rollover code goes here
				TweenLite.to(coin_10000, 2, {
					scaleX: .8,
					scaleY: .8,
					ease: Elastic.easeOut
				});
			} else {
				if (coin_selected != 10000)
				// rollout code goes here
					TweenLite.to(coin_10000, 1, {
						scaleX: .7,
						scaleY: .7,
						//y: coin_10000.y + 10,
						ease: Elastic.easeOut
					});
			}
			if (e.getTouch(coin_25000, TouchPhase.HOVER)) {
				// rollover code goes here
				TweenLite.to(coin_25000, 2, {
					scaleX: .8,
					scaleY: .8,
					ease: Elastic.easeOut
				});
			} else {
				if (coin_selected != 25000)
				// rollout code goes here
					TweenLite.to(coin_25000, 1, {
						scaleX: .7,
						scaleY: .7,
						//	y: coin_25000.y + 10,
						ease: Elastic.easeOut
					});
			}
			if (e.getTouch(coin_50000, TouchPhase.HOVER)) {
				// rollover code goes here
				TweenLite.to(coin_50000, 2, {
					scaleX: .8,
					scaleY: .8,
					ease: Elastic.easeOut
				});
			} else {
				if (coin_selected != 50000)
				// rollout code goes here
					TweenLite.to(coin_50000, 1, {
						scaleX: .7,
						scaleY: .7,
						//y: coin_50000.y + 10,
						ease: Elastic.easeOut
					});
			}
			if (e.getTouch(coin_100000, TouchPhase.HOVER)) {
				// rollover code goes here
				TweenLite.to(coin_100000, 2, {
					scaleX: .8,
					scaleY: .8,
					ease: Elastic.easeOut
				});
			} else {
				if (coin_selected != 100000)
				// rollout code goes here
					TweenLite.to(coin_100000, 1, {
						scaleX: .7,
						scaleY: .7,
						//y: coin_100000.y + 10,
						ease: Elastic.easeOut
					});
			}
*/
			if (e.getTouch(btn_back, TouchPhase.HOVER)) {
				// rollover code goes here
				TweenLite.to(btn_back, .8, {
					scaleX: .9,
					scaleY: .9,
					ease: Strong.easeOut
				});

				if (cont.x < 0) {
					TweenMax.to(cont, .5, {
						x: cont.x + 30
					});
				} else {
					TweenMax.to(cont, .5, {
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

				if (cont.x > -550) {
					TweenMax.to(cont, .5, {
						x: cont.x - 30
					});
				} else {
					TweenMax.to(cont, .5, {
						x: -550,
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




		}



	}
}
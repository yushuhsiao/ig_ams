package screens {

	import starling.display.graphics.*;
	import events.GameSettings;
	import events.GameAnimation;
	import events.Connect;
import screens.StarlingVideo;
	
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

import starling.utils.HAlign;

	import starling.events.EnterFrameEvent;
	

	import starling.errors.AbstractClassError;
	import starling.display.Quad;

	//TweenPlugin.activate([ColorMatrixFilterPlugin, GlowFilterPlugin]);

	public class Game extends Sprite {
	
		private var bg: Image;
		private var cont_bg: Image;
		
			private var back_texture:Texture;
			private var next_texture:Texture;
		private var back_disabled_texture:Texture;
			private var next_disabled_texture:Texture;
		
		private var cont_3: Image;

		private var cont_cash: Image;
		private var cont_mask: Quad;
		private var cont_chip: Image;
		private var score: Image;
		private var result: Image;

		private var win_counter: Image;
		private var timer: Image;

		private var marker_road: Image;
			private var marker_road_mask: Quad;
			private var big_eye_road: Image;
			private var big_eye_road_mask: Quad;
			private var small_road: Image;
			private var small_road_mask: Quad;
			private var cockroach_road: Image;
			private var cockroach_road_mask: Quad;
		
			public var cont_marker_road_all: Sprite = new Sprite();
		public var cont_marker_road: Sprite = new Sprite();
		public var cont_marker_road_: Sprite = new Sprite();
			public var cont_marker_road_btn: Sprite = new Sprite();
		private var mr_btn_back_: Image;
				private var mr_btn_next_: Image;
			
				public var cont_big_road_all: Sprite = new Sprite();	
			public var cont_big_road: Sprite = new Sprite();	
		public var cont_big_road_: Sprite = new Sprite();
		public var cont_big_road_btn: Sprite = new Sprite();
		

		
		private var big_road_mask: Quad;
			private var br_btn_back_: Image;
				private var br_btn_next_: Image;
				
			
		public var player_pre: Vector.<Image>= new Vector.<Image>();
		public var banker_pre: Vector.<Image>= new Vector.<Image>();
			
			public var dot: Vector.<Image>= new Vector.<Image>();
		private var dot_texture: Vector.<Texture>= new Vector.<Texture>();
		private var player_pre_texture: Vector.<Texture>= new Vector.<Texture>();
		private var banker_pre_texture: Vector.<Texture>= new Vector.<Texture>();
		private var tie_texture:Texture;
		
				public var cont_big_eye_all: Sprite = new Sprite();	
				public var cont_big_eye: Sprite = new Sprite();	
				public var cont_big_eye_: Sprite = new Sprite();
				public var cont_big_eye_btn: Sprite = new Sprite();
		
		private var be_btn_back_: Image;
				private var be_btn_next_: Image;
		
			private var be_win:Array = new Array();
		private var be_col:Number = 0;
			private var be_row:Array = new Array();
			private var be_count: Number = 0;
				private var be_right:Boolean = false;
		private var be_dots:Array = new Array();
				private var be_dots_placeX:Array = new Array();
	private var be_dots_placeY:Array = new Array();
	
				private var be: Object = {dot: {0: {0:Image},1: {1:Image},2: {2:Image},3: {3:Image},4: {4:Image},5: {5:Image}},space: {0: {0: false},1: {0: false},2: {0: false},3: {0:false},4: {0: false},5: {0:false}}};
					
						public var cont_small_road_all: Sprite = new Sprite();	
				public var cont_small_road: Sprite = new Sprite();	
		public var cont_small_road_: Sprite = new Sprite();
		public var cont_small_road_btn: Sprite = new Sprite();
					
					private var sr_btn_back_: Image;
				private var sr_btn_next_: Image;
		
			private var sr_win:Array = new Array();
		private var sr_col:Number = 0;
			private var sr_row:Array = new Array();
			private var sr_count: Number = 0;
				private var sr_right:Boolean = false;
		private var sr_dots:Array = new Array();
				private var sr_dots_placeX:Array = new Array();
	private var sr_dots_placeY:Array = new Array();
	
				private var sr: Object = {dot: {0: {0:Image},1: {1:Image},2: {2:Image},3: {3:Image},4: {4:Image},5: {5:Image}},space: {0: {0: false},1: {0: false},2: {0: false},3: {0:false},4: {0: false},5: {0:false}}};
					
						public var cont_cockroach_road_all: Sprite = new Sprite();	
				public var cont_cockroach_road: Sprite = new Sprite();	
		public var cont_cockroach_road_: Sprite = new Sprite();
		public var cont_cockroach_road_btn: Sprite = new Sprite();
					
					private var cr_btn_back_: Image;
				private var cr_btn_next_: Image;
		
			private var cr_win:Array = new Array();
		private var cr_col:Number = 0;
			private var cr_row:Array = new Array();
			private var cr_count: Number = 0;
				private var cr_right:Boolean = false;
		private var cr_dots:Array = new Array();
				private var cr_dots_placeX:Array = new Array();
	private var cr_dots_placeY:Array = new Array();
	
				private var cr: Object = {dot: {0: {0:Image},1: {1:Image},2: {2:Image},3: {3:Image},4: {4:Image},5: {5:Image}},space: {0: {0: false},1: {0: false},2: {0: false},3: {0:false},4: {0: false},5: {0:false}}};
					
					private var mr_col:Number = 0;
			private var mr_row:Array = new Array();
			private var mr_count: Number = 0;
		private var mr_dots:Array = new Array();
				private var mr_dots_placeX:Array = new Array();
	private var mr_dots_placeY:Array = new Array();
			private var mr_texture: Vector.<Texture>= new Vector.<Texture>();
					
					
			private var mr: Object = {dot: {0: {0:Image},1: {1:Image},2: {2:Image},3: {3:Image},4: {4:Image},5: {5:Image}},pair_p: {0: {0:Image},1: {1:Image},2: {2:Image},3: {3:Image},4: {4:Image},5: {5:Image}},pair_b: {0: {0:Image},1: {1:Image},2: {2:Image},3: {3:Image},4: {4:Image},5: {5:Image}},tie: {0: {0:Image},1: {1:Image},2: {2:Image},3: {3:Image},4: {4:Image},5: {5:Image}}};
			
		private var br_win:Array = new Array();
		private var br_tie:Number = 0;
			private var br_col:Number = 0;
			private var br_row:Array = new Array();
				
			private var br_last_win: String = "";
	private var br_next:Boolean = false;
	private var br_right:Boolean = false;
		private var br_count: Number = 0;
		private var br_dots:Array = new Array();
	

		private var dot_tie: Image;
private var br: Object = {dot: {0: {0:Image},1: {1:Image},2: {2:Image},3: {3:Image},4: {4:Image},5: {5:Image}},num: {0: {0:TextField},1: {1:TextField},2: {2:TextField},3: {3:TextField},4: {4:TextField},5: {5:TextField}},pair_p: {0: {0:Image},1: {1:Image},2: {2:Image},3: {3:Image},4: {4:Image},5: {5:Image}},pair_b: {0: {0:Image},1: {1:Image},2: {2:Image},3: {3:Image},4: {4:Image},5: {5:Image}},tie: {0: {0:Image},1: {1:Image},2: {2:Image},3: {3:Image},4: {4:Image},5: {5:Image}},tie_: {0: {0:TextField},1: {1:TextField},2: {2:TextField},3: {3:TextField},4: {4:TextField},5: {5:TextField}},pair_p_: {0: {0:false},1: {1:false},2: {2: false},3: {3: false},4: {4:false},5: {5: false}},pair_b_: {0: {0: false},1: {1: false},2: {2: false},3: {3:false},4: {4: false},5: {5:false}},space: {0: {0: false},1: {0: false},2: {0: false},3: {0:false},4: {0: false},5: {0:false}}};
	private var dots_placeX:Array = new Array();
	private var dots_placeY:Array = new Array();
		
	
		private var dot_red:Array = new Array("red_out instance 10000","red_in instance 10000","red_slash instance 10000","red_dot instance 10000");
		private var dot_blue:Array = new Array("blue_out instance 10000","blue_in instance 10000","blue_slash instance 10000","blue_dot instance 10000");
	
		private var banker_pre_:Array = new Array();
private var player_pre_:Array = new Array();
		public var cont_prediction: Sprite = new Sprite();

		
		public var cont_score: Sprite = new Sprite();
		public var cont_timer: Sprite = new Sprite();
		public var cont_select: Sprite = new Sprite();
		public var cont_btn_bet: Sprite = new Sprite();
		public var cont_btn_clear: Sprite = new Sprite();
		public var cont_btn_auto: Sprite = new Sprite();
		public var cont_btn_back: Sprite = new Sprite();
		public var cont_btn_next: Sprite = new Sprite();
		//private var btn_bet_text_: TextField;
		private var btn_bet: Image;
		private var btn_clear: Image;
		private var btn_auto: Image;
		private var btn_bet_text: Image;
		private var btn_clear_text: Image;
		private var btn_auto_text: Image;
		private var btn_bet_lock: Image;
		private var btn_clear_lock: Image;
		private var btn_auto_lock: Image;
		private var auto_submit: Boolean = false;

		public var btn_next: Image;
		public var btn_back: Image;
		public var btn_next_disabled: Image;
		public var btn_back_disabled: Image;

		//Cards
		public var cards: Vector.<Image>= new Vector.<Image>();
		private var cards_texture: Vector.<Texture>= new Vector.<Texture>();
		//Chip Settings

		public var cont_selection: Sprite = new Sprite();

		public var chip: Vector.<Image>= new Vector.<Image>();
		public var bet_chip: Vector.<Image>= new Vector.<Image>();
		public var bet_chip_mouse: Vector.<Image>= new Vector.<Image>();
		private var chip_texture: Vector.<Texture>= new Vector.<Texture>();
		private var bet_chip_texture: Vector.<Texture>= new Vector.<Texture>();
		private var cont_chip_: Sprite = new Sprite();
		private var chip_anim_up: Number = 520;
		private var chip_anim_mid: Number = 530;
		private var chip_anim_down: Number = 538;

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
private var bet_name: Array = new Array("Either-Pair","Big","Player-Pair","Player","Tie","Super 6","Banker","Banker-Pair","Small","Perfect-Pair");
private var bet_name_trans: Array = new Array("任意一对","大","闲对","闲","和","超级六","庄","庄对","小","完美对子");
private var bet_value: Array = new Array(5,0.53,11,1,8,12,1,11,1.5,1.25);

		private var o: Object = {bet_: {0: {0: Vector.<Image>},1: {1: Vector.<Image>},2: {2: Vector.<Image>},3: {3: Vector.<Image>},4: {4: Vector.<Image>},5: {5: Vector.<Image>},6: {6: Vector.<Image>},7: {7: Vector.<Image>},8: {8: Vector.<Image>},9: {9: Vector.<Image>}}};

		private var p: Object = {bet_: {0: {0: Vector.<Image>},1: {1: Vector.<Image>},2: {2: Vector.<Image>},3: {3: Vector.<Image>},4: {4: Vector.<Image>},5: {5: Vector.<Image>},6: {6: Vector.<Image>},7: {7: Vector.<Image>},8: {8: Vector.<Image>},9: {9: Vector.<Image>}}};

		
		
		private var bet_text: Array = new Array();
		private var bet_chip_text: Array = new Array();
		private var bet_chip_add_text: Array = new Array();
		private var bets_loc: Array = new Array(-20, -18, -13, -8, 0, 20, 5, 5, 10, 15);
		private var bets_chip_loc: Array = new Array(35, 25, 20, 23, 7, 7, 7, 7, 10, 12);
		private var bets_Min_X: Array = new Array(-40, -38, -40, -60, -10, 0, -50, -35, -35, -30);
		private var bets_Max_X: Array = new Array(0, 5, 7, 30, 30, 30, 30, 15, 18, 17);
		private var bets_Min_Y: Array = new Array(10, 7, 5, -15, -15, 0, -15, 5, 5, 6);
		private var bets_Max_Y: Array = new Array(15, 15, 13, 10, 10, 10, 10, 15, 12, 14);
		//Classes
		private var gameSettings: GameSettings = new GameSettings();
		private var gameAnimation: GameAnimation = new GameAnimation();
		private var connect_: Connect = new Connect()
		//private var main_:Main = new Main();

			private var total_balance: Number = 0;
		private var tot_balance: Number = 0;
		private var total_balance_text: TextField;
		private var total_bets: Number = 0;

		//------temp------
		private var temp_bal: Number = total_balance;
		private var temp_chip_selected: Number;
		private var temp_add_bets: Array = new Array(0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
		private var temp_bets: Array = new Array(0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
		private var temp_live: Array = new Array(0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
		private var temp_tbl: Array = new Array(0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
		private var temp_win: Array = new Array(0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
	
		private var temp_game: String;	

	
		//game
		//private var player:Array = new Array();
		//private var banker:Array = new Array();
		private var player_score: Number = 0;
		private var banker_score: Number = 0;

		private var cardx_holder: Array = new Array(-110, -63, -23, 25, 65, 112);
		private var cardy_holder: Array = new Array(12, 5, 5, 5, 5, 12);

		private var con: Boolean = false;


		private var win_status: Boolean = false;
	private var interval_count: Number = 0;
		//---------font-----
		private var con_timer: Number = 0;
		private var timer_text: TextField;

		private var player_text: TextField;
		private var banker_text: TextField;
		private var result_text: TextField;
		
			public var cont_win_counter: Sprite = new Sprite();
			private var win_count: Array = new Array(0, 0, 0, 0, 0, 0, 0);
		private var win_count_text:Vector.<TextField> = new Vector.<TextField>();
		


		private var _video:StarlingVideo;
		private var cont_video:Sprite = new Sprite();
		
		public function Game() {
				
			super();
			this.addEventListener(Event.ADDED_TO_STAGE, onAddedToStage);
		}


		private function onAddedToStage(event: Event): void {
	
			//Starling.current.nativeOverlay.setChildIndex(cont_video,0);
			drawStage();
			this.removeEventListener(Event.ADDED_TO_STAGE, onAddedToStage);
			this.addEventListener(EnterFrameEvent.ENTER_FRAME, onUpdate);
				
		}
		
		private function onUpdate(e: EnterFrameEvent): void {
			if (connect_.status_ == "Connected") {
			if(connect_.account_f){
				total_balance = connect_.account_details.amount;
				temp_bal = total_balance;
			total_balance_text.text = total_balance.toString();
			connect_.account_f = false;
			}
					if(connect_.reset_f){
					resetAll();
			
						connect_.reset_f = false;
					}
				if(connect_.win_counter_f){
					connect_.win_counter_f = false;
					for(var p:Number=0; p < win_count.length;p++){
					win_count_text[p].text = connect_.win_counter_details.win_count[p].toString();
					}
				}
				if(connect_.big_road_f){
					for(var q:Number = 1 ;q<connect_.big_road_details.won_count;q++){
						big_road_num(connect_.big_road_details.won_all[q].win,connect_.big_road_details.won_all[q].win_num);
						marker_num(connect_.big_road_details.won_all[q].win);
						}
						connect_.big_road_f = false;
				}
				if(connect_.interval_f){
					interval_count = connect_.interval_details.count;
				
					if(interval_count == 1){
						if(connect_.win_f && !connect_.big_road_f){
				 win_event(connect_.win_details.win );
							big_road_num(connect_.win_details.win,connect_.win_details.win_num);
							marker_num(connect_.win_details.win);
						connect_.win_f = false;
			}
			
					}
					if(interval_count == 1){
						win_get(interval_count);
						win_anim();
					}
					if(interval_count > 1 && interval_count <6){
						if(result_text.text =="")
						win_anim();
					}
					if(interval_count >= 2){
						win_get(interval_count);
					}
					if(interval_count == 3){
						win_release(interval_count);
					}
					if(interval_count > 3 && interval_count <6){
						win_release(interval_count);
					}
					if(interval_count >= 6){
						win_end();
					}
						connect_.interval_f = false;
				}
				
				
			if (connect_.countdown_f) {
					
				if(connect_.countdown_details.status == false){
					cont_timer.visible = true;
					con_timer = connect_.countdown_details.countdown;
					timer_text.text = con_timer.toString();
					cont_score.visible = false;
					cont_score.alpha = 1;
					if(con_timer >=0 && con_timer <=3){
						timer_text.color = 0xFF0000;
						TweenLite.from(timer_text,.2,{scaleX:timer_text.scaleX +1,scaleY:timer_text.scaleY +1,alpha:0});
						connect_.countdown_f = false;
						
					}
				
					else{
						timer_text.color = 0xFFFFFF;
						TweenLite.from(timer_text,.3,{alpha:0});
					connect_.countdown_f = false;
				}
					if(!auto_submit){btn_bet.visible = true;
			btn_bet_text.visible = true;
			btn_bet_lock.visible = false;}
				} else {
					cont_timer.visible = false;
					timer_text.text = "-";
					cont_score.visible = true;
					if(connect_.drew_f){
					for(var k:Number = 0 ;k<connect_.drew_details.draw;k++){
						drawCards(connect_.drew_details.cards_drew[k].draws, connect_.drew_details.cards_drew[k].card, 					connect_.drew_details.cards_drew[k].card_value, connect_.drew_details.cards_drew[k].draw);
					}
					if(connect_.won_f){
						win_event(connect_.won_details.win);
						//if(!connect_.big_road_f)
						//big_road_num(connect_.won_details.win,connect_.won_details.win_num);
						connect_.won_f = false;
					}
							connect_.drew_f = false;
						}
						if (connect_.draw_f) {
						drawCards(connect_.draw_details.draws, connect_.draw_details.card, connect_.draw_details.card_value, connect_.draw_details.draw);
						if(connect_.draw_details.draw==0 )
						clear_event();
						connect_.draw_f = false;
					}
				}
				//connect_.countdown_f = false;
			}
		
			
			
				if (connect_.bet_f||con) {
					temp_game = connect_.bet_details.Game;
					connect_.bet_f = false;					
					for (var i: Number = 0; i < gameSettings.bet_tag.length; i++) {

						if (connect_.bet_details.bet[i] != temp_live[i]) {
							//	bets[i] = connect_.bet_details.bet[i];
							win_status = false;
							chip_bet_check(i, connect_.bet_details.bet[i] - temp_live[i]);
							bet_text[i].text = connect_.bet_details.bet[i];
							temp_live[i] = connect_.bet_details.bet[i];
							temp_tbl[i] = connect_.bet_details.bet[i];
							con = true;
					
						}
					}
				}
			}
			
		}


private function resetAll(): void {
		for(var w = 0 ;w <=br_count;w++){
							cont_big_road_.removeChild(br_dots[w]);
							br_dots[w] = null;
						}
						for(w = 0 ;w <=mr_count;w++){
							cont_marker_road_.removeChild(mr_dots[w]);
							mr_dots[w] = null;
						}
						for(w = 0 ;w <=be_count;w++){
							cont_big_eye_.removeChild(be_dots[w]);
							be_dots[w] = null;
						}
						for(w = 0 ;w <=sr_count;w++){
							cont_small_road_.removeChild(sr_dots[w]);
							sr_dots[w] = null;
						}
						
							for(w = 0 ;w <=cr_count;w++){
							cont_cockroach_road_.removeChild(cr_dots[w]);
							cr_dots[w] = null;
						}
					
			for(w = 0 ; w <=200;w++){
				for(var o:Number = 0; o <6;o++){
					br.space[o][w] = false;
					be.space[o][w] = false;
					sr.space[o][w] = false;
					cr.space[o][w] = false;
				}
				
				br_btn_back_.alpha = 0;
				br_btn_next_.alpha = 0;
				be_btn_back_.alpha = 0;
				be_btn_next_.alpha = 0;
				sr_btn_back_.alpha = 0;
				sr_btn_next_.alpha = 0;
				cr_btn_back_.alpha = 0;
				cr_btn_next_.alpha = 0;
			}
			
				while (this.cont_prediction.numChildren > 0) {
				this.cont_prediction.removeChildAt(0);
			}
						br_dots = [];
						br_win = [];
						br_tie= 0;
						br_col = 0;
						br_row =[];
						br_row[br_col] = 0;
						br_count = 0;
						cont_big_road_.x = 0;
			
						mr_dots = [];
						mr_col = 0;
						mr_row[mr_col] = 0;
						mr_row[mr_col] = 0;
						mr_count = 0;
						cont_marker_road_.x = 0;
			
						be_dots = [];
						be_win = [];
						be_col = 0;
						be_row =[];
						be_row[be_col] = 0;
						be_count = 0;
						cont_big_eye_.x = 0;
			
						sr_dots = [];
						sr_win = [];
						sr_col = 0;
						sr_row =[];
						sr_row[sr_col] = 0;
						sr_count = 0;
						cont_small_road_.x = 0;
						
						cr_dots = [];
						cr_win = [];
						cr_col = 0;
						cr_row =[];
						cr_row[cr_col] = 0;
						cr_count = 0;
						cont_cockroach_road_.x = 0;
}
		private function drawStage(): void {
		
		
	
			//Starling.current.nativeOverlay.addChild(_video);
			//Background
			bg = new Image(Assets.getTexture("BgLayer"));
			bg.width = stage.stageWidth;
			bg.height = stage.stageHeight;
			this.addChild(bg);
			//setChildIndex(bg, 0);
			
		
			
			//_video = new StarlingVideo();
			
			
			//cont_video.addChild(_video);
			//this.addChild(cont_video);
			
			//Background panel lower
			cont_bg = new Image(Assets.getAtlas().getTexture("cont_bg instance 10000"));
			cont_bg.height = cont_bg.height - 90;
			cont_bg.width = stage.stageWidth;
			cont_bg.x = 0;
			cont_bg.y = stage.stageHeight - cont_bg.height;
			this.addChild(cont_bg);
			
			///var Rect:Quad = new Quad(stage.stageWidth, stage.stageHeight - cont_bg.height, 0xFFFFFF);
			//_video.mask = Rect;

			preload();
			panel_big_road();
			panel_win_counter();
			panel_selection();

			panel_betArea();

			panel_timer();
			panel_score();
			panel_marker_road();
			panel_big_eye();
			panel_small_road();
			panel_cockroach_road();
		}

		private function preload(): void {
			for (var i:Number=0; i < dot_red.length ; i++) {
					banker_pre_texture[i] = Assets.getAtlas().getTexture(dot_red[i]);
					player_pre_texture[i] = Assets.getAtlas().getTexture(dot_blue[i]);
			}
			mr_texture[0] = Assets.getAtlas().getTexture("Player instance 1");
			mr_texture[1] = Assets.getAtlas().getTexture("Banker instance 1");
			mr_texture[2] = Assets.getAtlas().getTexture("Super6 instance 1");
			tie_texture = Assets.getAtlas().getTexture("green_tie instance 10000");
			
			back_texture = Assets.getAtlas().getTexture("btn_back instance 10000");
			next_texture = Assets.getAtlas().getTexture("btn_next instance 10000");
			back_disabled_texture = Assets.getAtlas().getTexture("btn_back_disabled instance 10000");
			next_disabled_texture = Assets.getAtlas().getTexture("btn_next_disabled instance 10000");
			
			br_row.push(br_col);
			dots_placeX.push(11);
			dots_placeY.push(501.5);
			
			mr_row.push(mr_col);
			mr_dots_placeX.push(698);
			mr_dots_placeY.push(501);
			
			be_row.push(be_col);
			be_dots_placeX.push(851);
			be_dots_placeY.push(496);
			
			sr_row.push(sr_col);
			sr_dots_placeX.push(851);
			sr_dots_placeY.push(531);
			
			cr_row.push(cr_col);
			cr_dots_placeX.push(851);
			cr_dots_placeY.push(566);
			
			for(i = 0 ; i <=200;i++){
				dots_placeX[i+1] =  dots_placeX[i]+ 17.6;
				mr_dots_placeX[i+1] =  mr_dots_placeX[i]+ 17.7;
				be_dots_placeX[i+1] =  be_dots_placeX[i]+ 5.8;
				sr_dots_placeX[i+1] =  sr_dots_placeX[i]+ 5.8;
				cr_dots_placeX[i+1] =  cr_dots_placeX[i]+ 5.8;
				for(var o:Number = 0; o <6;o++){
					br.space[o][i] = false;
						be.space[o][i] = false;
						sr.space[o][i] = false;
					cr.space[o][i] = false;
					dots_placeY[o+1] = dots_placeY[o] +17.5;
					mr_dots_placeY[o+1] = mr_dots_placeY[o] +17.5;
					be_dots_placeY[o+1] = be_dots_placeY[o] +5.8;
					sr_dots_placeY[o+1] = sr_dots_placeY[o] +5.8;
					cr_dots_placeY[o+1] = cr_dots_placeY[o] +5.8;
				}
			}
		
		}
		private function panel_cockroach_road(): void {
			cockroach_road = new Image(Assets.getAtlas().getTexture("Cockroach Road instance 10000"));
			cockroach_road.scaleX = .64;
			cockroach_road.scaleY = .64;
			cockroach_road.x = marker_road_mask.x +marker_road_mask.width ;
			cockroach_road.y = small_road_mask.y +small_road_mask.height ;
			//cont_cockroach_road.addChild(cockroach_road);
			
			
			cockroach_road_mask = new Quad(174, 35, 0xFFFFFF);
			cockroach_road_mask.scaleX = .64;
			cockroach_road_mask.scaleY = .64;
			cockroach_road_mask.x =cockroach_road.x;
			cockroach_road_mask.y =cockroach_road.y;
			cont_cockroach_road.addChild(cockroach_road_mask);
			
			cr_btn_back_ = new Image(Assets.getAtlas().getTexture("btn_back_ instance 10000"));
			cr_btn_back_.scaleX = .4;
			cr_btn_back_.scaleY = .4;
			cr_btn_back_.x = cockroach_road.x+(cockroach_road.width /2)  - cr_btn_back_.width;
			cr_btn_back_.y = cockroach_road.y + cockroach_road.height  - cr_btn_back_.height;
			cr_btn_back_.alpha = 0;
			cont_cockroach_road_btn.addChild(cr_btn_back_);
			
			cr_btn_next_ = new Image(Assets.getAtlas().getTexture("btn_next_ instance 10000"));
			cr_btn_next_.scaleX = .4;
			cr_btn_next_.scaleY = .4;
			cr_btn_next_.x = cockroach_road.x +(cockroach_road.width /2) + cr_btn_next_.width;
			cr_btn_next_.y = cockroach_road.y + cockroach_road.height - cr_btn_next_.height;
			cr_btn_next_.alpha = 0;
			cont_cockroach_road_btn.addChild(cr_btn_next_);
			
			
			cont_cockroach_road_all.addChild(cont_cockroach_road);
			cont_cockroach_road_all.addChild(cont_cockroach_road_);
			cont_cockroach_road_all.addChild(cont_cockroach_road_btn);
			this.addChild(cont_cockroach_road_all);
			cont_cockroach_road_.mask = cockroach_road_mask ;
			
		}
		private function panel_small_road(): void {
	
			
				small_road_mask = new Quad(174, 35, 0xFFFFFF);
			small_road_mask.x =marker_road_mask.x +marker_road_mask.width ;
			small_road_mask.y =marker_road_mask.y +small_road_mask.height ;
			cont_small_road.addChild(small_road_mask);
			
			sr_btn_back_ = new Image(Assets.getAtlas().getTexture("btn_back_ instance 10000"));
			sr_btn_back_.scaleX = .4;
			sr_btn_back_.scaleY = .4;
			sr_btn_back_.x = small_road_mask.x+(small_road_mask.width /2)  - sr_btn_back_.width;
			sr_btn_back_.y = small_road_mask.y + small_road_mask.height  - sr_btn_back_.height;
			sr_btn_back_.alpha = 0;
			cont_small_road_btn.addChild(sr_btn_back_);
			
			sr_btn_next_ = new Image(Assets.getAtlas().getTexture("btn_next_ instance 10000"));
			sr_btn_next_.scaleX = .4;
			sr_btn_next_.scaleY = .4;
			sr_btn_next_.x = small_road_mask.x +(small_road_mask.width /2) + sr_btn_next_.width;
			sr_btn_next_.y = small_road_mask.y + small_road_mask.height - sr_btn_next_.height;
			sr_btn_next_.alpha = 0;
			cont_small_road_btn.addChild(sr_btn_next_);
			
			cont_small_road_all.addChild(cont_small_road);
			cont_small_road_all.addChild(cont_small_road_);
			cont_small_road_all.addChild(cont_small_road_btn);
			this.addChild(cont_small_road_all);
			cont_small_road_.mask = small_road_mask ;
		}
		private function panel_big_eye(): void {

			big_eye_road_mask = new Quad(174, 35, 0xFFFFFF);
			big_eye_road_mask.x = marker_road_mask.x +marker_road_mask.width ;
			big_eye_road_mask.y = marker_road_mask.y;
			cont_big_eye.addChild(big_eye_road_mask);
			
			be_btn_back_ = new Image(Assets.getAtlas().getTexture("btn_back_ instance 10000"));
			be_btn_back_.scaleX = .4;
			be_btn_back_.scaleY = .4;
			be_btn_back_.x = big_eye_road_mask.x+(big_eye_road_mask.width /2)  - be_btn_back_.width;
			be_btn_back_.y = big_eye_road_mask.y + big_eye_road_mask.height  - be_btn_back_.height;
			be_btn_back_.alpha = 0;
			cont_big_eye_btn.addChild(be_btn_back_);
			
			be_btn_next_ = new Image(Assets.getAtlas().getTexture("btn_next_ instance 10000"));
			be_btn_next_.scaleX = .4;
			be_btn_next_.scaleY = .4;
			be_btn_next_.x = big_eye_road_mask.x +(big_eye_road_mask.width /2) + be_btn_next_.width;
			be_btn_next_.y = big_eye_road_mask.y + big_eye_road_mask.height - be_btn_next_.height;
			be_btn_next_.alpha = 0;
			cont_big_eye_btn.addChild(be_btn_next_);
			
			cont_big_eye_all.addChild(cont_big_eye);
			cont_big_eye_all.addChild(cont_big_eye_);
			cont_big_eye_all.addChild(cont_big_eye_btn);
			this.addChild(cont_big_eye_all);
			cont_big_eye_.mask = big_eye_road_mask ;
		}
		private function panel_marker_road(): void {
			
			marker_road_mask = new Quad(159, 105.60, 0xFFFFFF);
			marker_road_mask.x = btn_next.x +btn_next.width ;
			marker_road_mask.y = stage.stageHeight - marker_road_mask.height -2;
			cont_marker_road.addChild(marker_road_mask);
			
			mr_btn_back_ = new Image(Assets.getAtlas().getTexture("btn_back_ instance 10000"));
			mr_btn_back_.scaleX = .64;
			mr_btn_back_.scaleY = .64;
			mr_btn_back_.x = marker_road_mask.x+(marker_road_mask.width /2)  - mr_btn_back_.width;
			mr_btn_back_.y = marker_road_mask.y +75;
			mr_btn_back_.alpha = 0;
			cont_marker_road_btn.addChild(mr_btn_back_);
			
			mr_btn_next_ = new Image(Assets.getAtlas().getTexture("btn_next_ instance 10000"));
			mr_btn_next_.scaleX = .64;
			mr_btn_next_.scaleY = .64;
			mr_btn_next_.x = marker_road_mask.x+(marker_road_mask.width /2) + mr_btn_next_.width;
			mr_btn_next_.y = marker_road_mask.y +75;
			mr_btn_next_.alpha = 0;
			cont_marker_road_btn.addChild(mr_btn_next_);
			
			cont_marker_road_all.addChild(cont_marker_road);
			cont_marker_road_all.addChild(cont_marker_road_);
			cont_marker_road_all.addChild(cont_marker_road_btn);
			this.addChild(cont_marker_road_all);
			cont_marker_road_.mask = marker_road_mask ;
		
			
			
		}
		private function panel_big_road(): void {
	
			big_road_mask = new Quad(299.52, 108.16, 0xFFFFFF);
			big_road_mask.x = 3 ;
			big_road_mask.y = stage.stageHeight - big_road_mask.height;
			cont_big_road.addChild(big_road_mask);
			
			br_btn_back_ = new Image(Assets.getAtlas().getTexture("btn_back_ instance 10000"));
			br_btn_back_.scaleX = .64;
			br_btn_back_.scaleY = .64;
			br_btn_back_.x = (big_road_mask.width /2)  - br_btn_back_.width;
			br_btn_back_.y = big_road_mask.y +75;
			br_btn_back_.alpha = 0;
			cont_big_road_btn.addChild(br_btn_back_);
			
				br_btn_next_ = new Image(Assets.getAtlas().getTexture("btn_next_ instance 10000"));
			br_btn_next_.scaleX = .64;
			br_btn_next_.scaleY = .64;
			br_btn_next_.x = (big_road_mask.width /2) + br_btn_next_.width;
			br_btn_next_.y = big_road_mask.y +75;
			br_btn_next_.alpha = 0;
		cont_big_road_btn.addChild(br_btn_next_);
			
			cont_big_road_all.addChild(cont_big_road);
			cont_big_road_all.addChild(cont_big_road_);
			cont_big_road_all.addChild(cont_big_road_btn);
			this.addChild(cont_big_road_all);
			cont_big_road_.mask = big_road_mask ;
			
		}		
		private function cockroach_road_num(row:Number,col:Number): void {
		
			var win_:String;
				var temp_sprite:Sprite = new Sprite();
					var temp:Number = cr_row[cr_col];
					
				if(row == 0){
					if(br_row[col-4] == br_row[col -1]){
						win_ = 'R';
					}
					else{
						win_ = 'B';
					}
				}
				else{
						if(br.space[row][col - 3] == br.space[row - 1][col - 3]){
							win_ = 'R';
						}
					else{
							win_ = 'B';
						}
				}
				
				cr_win[cr_count] = win_;
				
				if(cr_count!=0){
					if(cr_win[cr_count] != cr_win[cr_count -1]&& !cr_right){
						cr_col +=1;
						cr_row[cr_col] = 0;
							cr_right  = false;
					}
				
					if( cr_win[cr_count] != cr_win[cr_count -1] && cr_right ){
					for(var i = 0 ;i <=100;i++ ){
								if(cr.space[0][i] == false){
									cr_col = i;
									cr_row[cr_col] = 0;
							i = 100;
								}
					}
					cr_right  = false;
			}
				
				if(cr_right && cr_win[cr_count] == cr_win[cr_count -1]){
					cr_col +=1;
					cr_row[cr_col] = temp - 1;
				}
				if(cr_row[cr_col] > 5 && cr_win[cr_count] == cr_win[cr_count -1] && !cr_right){
					cr_col +=1;
					cr_row[cr_col] = 5;
					cr_right  = true;
					}
				if(cr.space[cr_row[cr_col]][cr_col]&& cr_win[cr_count] == cr_win[cr_count -1] && !cr_right ){
					cr_col +=1;
					cr_row[cr_col] = temp - 1;
					cr_right  = true;
				}
				}
				
				
				
						
					if(cr_col >=24){
					cont_cockroach_road_.x =-(5.8 * (cr_col - 23));
					}
					if(win_ == 'R'){
						cr.dot[cr_row[cr_col]][cr_col] =  new Image(banker_pre_texture[2]);
					}
					else{
						cr.dot[cr_row[cr_col]][cr_col] =  new Image(player_pre_texture[2]);
					}
					cr.dot[cr_row[cr_col]][cr_col].alignPivot("center", "center");
				cr.dot[cr_row[cr_col]][cr_col].scaleX = .22;
				cr.dot[cr_row[cr_col]][cr_col].scaleY = .22;
			
				cr.dot[cr_row[cr_col]][cr_col].x = cr_dots_placeX[cr_col]  ;
				cr.dot[cr_row[cr_col]][cr_col].y = cr_dots_placeY[cr_row[cr_col]] ;
				temp_sprite.addChild(cr.dot[cr_row[cr_col]][cr_col]);
				
				cr.space[cr_row[cr_col]][cr_col]=true;
				cr_dots[cr_count]=temp_sprite;
				
				cont_cockroach_road_.addChild(cr_dots[cr_count]);
				if(!connect_.big_road_f){
				TweenLite.from(cr_dots[cr_count],.5,{alpha:0});
			}
		
				cr_count +=1;
				cr_row[cr_col]+=1;
			
			
			
		}
		private function small_road_num(row:Number,col:Number): void {
		
			var win_:String;
				var temp_sprite:Sprite = new Sprite();
					var temp:Number = sr_row[sr_col];
					
					if(row == 0){
					if(br_row[col-3] == br_row[col -1]){
						win_ = 'R';
					}
					else{
						win_ = 'B';
					}
				
				}
				else{
						if(br.space[row][col - 2] == br.space[row - 1][col - 2]){
							win_ = 'R';
						}
					else{
							win_ = 'B';
						}
						
				}
			
				sr_win[sr_count] = win_;
				
				if(sr_count!=0){
					if(sr_win[sr_count] != sr_win[sr_count -1]&& !sr_right){
						sr_col +=1;
						sr_row[sr_col] = 0;
							sr_right  = false;
					}
				
					if( sr_win[sr_count] != sr_win[sr_count -1] && sr_right ){
					for(var i = 0 ;i <=100;i++ ){
								if(sr.space[0][i] == false){
									sr_col = i;
									sr_row[sr_col] = 0;
							i = 100;
								}
					}
					sr_right  = false;
			}
				
				if(sr_right && sr_win[sr_count] == sr_win[sr_count -1]){
					sr_col +=1;
					sr_row[sr_col] = temp - 1;
				}
				if(sr_row[sr_col] > 5 && sr_win[sr_count] == sr_win[sr_count -1] && !sr_right){
					sr_col +=1;
					sr_row[sr_col] = 5;
					sr_right  = true;
					}
				if(sr.space[sr_row[sr_col]][sr_col]&& sr_win[sr_count] == sr_win[sr_count -1] && !sr_right ){
					sr_col +=1;
					sr_row[sr_col] = temp - 1;
					sr_right  = true;
				}
				}
	
					if(sr_col >=24){
					cont_small_road_.x =-(5.8 * (sr_col - 23));
					}
					if(win_ == 'R'){
						sr.dot[sr_row[sr_col]][sr_col] =  new Image(banker_pre_texture[1]);
					}
					else{
						sr.dot[sr_row[sr_col]][sr_col] =  new Image(player_pre_texture[1]);
					}
					sr.dot[sr_row[sr_col]][sr_col].alignPivot("center", "center");
				sr.dot[sr_row[sr_col]][sr_col].scaleX = .22;
				sr.dot[sr_row[sr_col]][sr_col].scaleY = .22;
			
				sr.dot[sr_row[sr_col]][sr_col].x = sr_dots_placeX[sr_col]  ;
				sr.dot[sr_row[sr_col]][sr_col].y = sr_dots_placeY[sr_row[sr_col]] ;
				temp_sprite.addChild(sr.dot[sr_row[sr_col]][sr_col]);
				
				sr.space[sr_row[sr_col]][sr_col]=true;
				sr_dots[sr_count]=temp_sprite;
				
				cont_small_road_.addChild(sr_dots[sr_count]);
				if(!connect_.big_road_f){
				TweenLite.from(sr_dots[sr_count],.5,{alpha:0});
			}
				
				sr_count +=1;
				sr_row[sr_col]+=1;
			
			
			
		}
		
		private function big_eye_num(row:Number,col:Number): void {
	
			var win_:String;
			
				var temp_sprite:Sprite = new Sprite();
					var temp:Number = be_row[be_col];
					
				if(row == 0){
					if(br_row[col-2] == br_row[col -1]){
						win_ = 'R';
					}
					else{
						win_ = 'B';
					}	
				
				}
				else{
						if(br.space[row][col - 1] == br.space[row - 1][col - 1]){
							win_ = 'R';
						}
						else{
							win_ = 'B';
						}
						
				}
			

				
				be_win[be_count] = win_;
			
				
				if(be_count!=0){
					if(be_win[be_count] != be_win[be_count -1]&& !be_right){
						be_col +=1;
						be_row[be_col] = 0;
							be_right  = false;
					}
				
					if( be_win[be_count] != be_win[be_count -1] && be_right ){
					for(var i = 0 ;i <=100;i++ ){
								if(be.space[0][i] == false){
									be_col = i;
									be_row[be_col] = 0;
							i = 100;
								}
					}
					be_right  = false;
				}
				
				if(be_right && be_win[be_count] == be_win[be_count -1]){
					be_col +=1;
					be_row[be_col] = temp - 1;
				}
				if(be_row[be_col] > 5 && be_win[be_count] == be_win[be_count -1] && !be_right){
					be_col +=1;
					be_row[be_col] = 5;
					be_right  = true;
					}
				if(be.space[be_row[be_col]][be_col]&& be_win[be_count] == be_win[be_count -1] && !be_right ){
					be_col +=1;
					be_row[be_col] = temp - 1;
					be_right  = true;
				}
				}
				
				
				
						
					if(be_col >=24){
					cont_big_eye_.x =-(5.8 * (be_col - 23));
					}
					if(win_ == 'R'){
						be.dot[be_row[be_col]][be_col] =  new Image(banker_pre_texture[0]);
					}
					else{
						be.dot[be_row[be_col]][be_col] =  new Image(player_pre_texture[0]);
					}
					be.dot[be_row[be_col]][be_col].alignPivot("center", "center");
				be.dot[be_row[be_col]][be_col].scaleX = .22;
				be.dot[be_row[be_col]][be_col].scaleY = .22;
			
				be.dot[be_row[be_col]][be_col].x = be_dots_placeX[be_col]  ;
				be.dot[be_row[be_col]][be_col].y = be_dots_placeY[be_row[be_col]] ;
				temp_sprite.addChild(be.dot[be_row[be_col]][be_col]);
				
				be.space[be_row[be_col]][be_col]=true;
				be_dots[be_count]=temp_sprite;
				
				cont_big_eye_.addChild(be_dots[be_count]);
				if(!connect_.big_road_f){
				TweenLite.from(be_dots[be_count],.5,{alpha:0});
			}
				be_count +=1;
				be_row[be_col]+=1;
			
		}
		private function marker_num(win:Array): void {
			var tie:Boolean = false;
			var pair_player:Boolean = false;
			var pair_banker:Boolean = false;
			
			var temp_sprite:Sprite = new Sprite();
		
			if(mr_col >5){
				cont_marker_road_.x = -(17.7 * (mr_col -6));
			}
				if(mr_row[mr_col] > 5){
					mr_col +=1;
					mr_row[mr_col] = 0;
				}
		
			for(var i = 0;i<win.length;i++){
				
			if(win[i] == "Player"){
				mr.dot[mr_row[mr_col]][mr_col] =  new Image(mr_texture[0]);	
			}
			if(win[i] == "Banker"){
				mr.dot[mr_row[mr_col]][mr_col] =  new Image(mr_texture[1]);
			}
			if(win[i] == "Super 6"){
				mr.dot[mr_row[mr_col]][mr_col] =  new Image(mr_texture[2]);				
			}
			if(win[i] == "Tie"){
				tie = true;
			}
			if(win[i] == "Player-Pair"){
				pair_player = true;
			}
			if(win[i] == "Banker-Pair" ){
				 pair_banker = true;
			}
			}
			
			if(!tie){
			mr.dot[mr_row[mr_col]][mr_col].alignPivot("center", "center");
			mr.dot[mr_row[mr_col]][mr_col].scaleX = .65;
			mr.dot[mr_row[mr_col]][mr_col].scaleY = .65;

			}
			
			if(pair_player){
				mr.pair_p[mr_row[mr_col]][mr_col] = new Image(player_pre_texture[3]);
				mr.pair_p[mr_row[mr_col]][mr_col].alignPivot("center", "center");
				mr.pair_p[mr_row[mr_col]][mr_col].scaleX = .65;
				mr.pair_p[mr_row[mr_col]][mr_col].scaleY = .65;
			}
			if(pair_banker){
				mr.pair_b[mr_row[mr_col]][mr_col] =  new Image(banker_pre_texture[3]);
				mr.pair_b[mr_row[mr_col]][mr_col].alignPivot("center", "center");
				mr.pair_b[mr_row[mr_col]][mr_col].scaleX = .65;
				mr.pair_b[mr_row[mr_col]][mr_col].scaleY = .65;
			}
			if(tie){
				mr.tie[mr_row[mr_col]][mr_col] =  new Image(Assets.getAtlas().getTexture("Tie instance 1"));
				mr.tie[mr_row[mr_col]][mr_col].alignPivot("center", "center");
				mr.tie[mr_row[mr_col]][mr_col].scaleX = .65;
				mr.tie[mr_row[mr_col]][mr_col].scaleY = .65;
			}
			
				if(!tie){
				mr.dot[mr_row[mr_col]][mr_col].x = mr_dots_placeX[mr_col]  ;
				mr.dot[mr_row[mr_col]][mr_col].y =mr_dots_placeY[mr_row[mr_col]];

				temp_sprite.addChild(mr.dot[mr_row[mr_col]][mr_col]);
				}
				
				if(tie){
			
				mr.tie[mr_row[mr_col]][mr_col].x = mr_dots_placeX[mr_col] ;
				mr.tie[mr_row[mr_col]][mr_col].y =  mr_dots_placeY[mr_row[mr_col]]+2;
		
				temp_sprite.addChild(mr.tie[mr_row[mr_col]][mr_col]);
				}
				
				if(pair_banker){
				mr.pair_b[mr_row[mr_col]][mr_col].x = mr_dots_placeX[mr_col] -5;
				mr.pair_b[mr_row[mr_col]][mr_col].y = mr_dots_placeY[mr_row[mr_col]] -5;

				temp_sprite.addChild(mr.pair_b[mr_row[mr_col]][mr_col]);
					}
				if(pair_player){
				mr.pair_p[mr_row[mr_col]][mr_col].x = mr_dots_placeX[mr_col]+5;
				mr.pair_p[mr_row[mr_col]][mr_col].y = mr_dots_placeY[mr_row[mr_col]] +5;
				
				temp_sprite.addChild(mr.pair_p[mr_row[mr_col]][mr_col]);
					}
				
			
				mr_dots[mr_count]=temp_sprite;
				
				cont_marker_road_.addChild(mr_dots[mr_count]);
			
				if(!connect_.big_road_f)
				TweenLite.from(mr_dots[mr_count],.5,{alpha:0});
					
				mr_count +=1;
				mr_row[mr_col]+=1;
		}
		
		
		private function big_road_num(win:Array,num:Number): void {
			var tie:Boolean = false;
			var tie_multi:Boolean = false;
			var pair_player:Boolean = false;
			var pair_banker:Boolean = false;
			var win_:String;
			var color_:String;
			var temp_sprite:Sprite = new Sprite();
	
			var temp:Number =br_row[br_col];
			for(var i:Number = 0;i<win.length;i++){
					if(win[i] == "Tie"){
						win_ = "Tie";
						tie = true;
						br_tie +=1;
					}
					if(win[i] == "Player"){
							win_= "Player";
						br_tie = 0;
					}
						if(win[i] == "Banker"){
							win_="Banker";
								br_tie = 0;
						}
						if(win[i] == "Super 6"){
								win_="Banker";
								br_tie = 0;
							
						}
			}
			br_win[br_count] = win_;
			if(br_col >14){
			cont_big_road_.x = -(17.6 * (br_col -14));
			}
			
			if(br_row[br_col] != 0 || br_col != 0){
		
				if(br_last_win  != win_ && br_last_win !="" && !tie && !br_next ){
					br_col +=1;
					br_row[br_col] = 0;
					br_right  = false;
				}
				
				if(br_win[br_count-1] != win_ && br_last_win =="" && br_win[br_count-1] =="Tie" && !tie && !br_next &&br_row[br_col] == 1&&br_col == 0 ){
					br_col +=1;
					br_row[br_col] = 0;
					br_right  = false;
				}
				
				if(tie){
					br_row[br_col] -=1;
					if(br_win[br_win.length -2]  == win_ &&tie)
					tie_multi= true;
				}
				if(br_row[br_col] > 5 &&br_last_win  == win_){
					br_col +=1;
					br_row[br_col] = 5;
					br_next = true;
				}
				if( br_last_win  != win_ && br_last_win !="" && !tie ){
					//br_row[br_col] = 0;
					for(i = 0 ;i <=50;i++ ){
								if(br.space[0][i] == false){
									br_col = i;
									br_row[br_col] = 0;
							i = 50;	
								}
					}
					br_next = false;
					br_right  = false;
			}
				if(br_right &&br_last_win  == win_){
					br_col +=1;
					br_row[br_col] = temp - 1;
			}
			if(br.space[br_row[br_col]][br_col]&&br_last_win  == win_ ){
					br_col +=1;
					br_row[br_col] = temp - 1;
					br_right  = true;
				}
			}	
		
			if(win_ == "Player" || win_=="Banker"){
			br_last_win = win_;
			}
			
			if(!br.space[br_row[br_col]][br_col] || br.space[br_row[br_col]][br_col] && tie){
			for( i = 0;i<win.length;i++){
				
			if(win[i] == "Player"){
				br.dot[br_row[br_col]][br_col] =  new Image(player_pre_texture[0]);
				color_ = "0x0000FF";
			
				
			}
			if(win[i] == "Banker"){
				br.dot[br_row[br_col]][br_col] =  new Image(banker_pre_texture[0]);
				color_ = "0xFF0000";
				
					
			}
			if(win[i] == "Super 6"){
				br.dot[br_row[br_col]][br_col] =  new Image(banker_pre_texture[1]);
				color_ = "0xFFFFFF";
				
			}
			if(win[i] == "Player-Pair" && !br.pair_p_[br_row[br_col]][br_col]){
				br.pair_p_[br_row[br_col]][br_col] = true;
				pair_player = true;
			}
			if(win[i] == "Banker-Pair" && !br.pair_b_[br_row[br_col]][br_col]){
				br.pair_b_[br_row[br_col]][br_col] = true;
				 pair_banker = true;
			}
				
			}

			if(!tie){
			br.dot[br_row[br_col]][br_col].alignPivot("center", "center");
			br.dot[br_row[br_col]][br_col].scaleX = .7;
			br.dot[br_row[br_col]][br_col].scaleY = .73;
				
			br.num[br_row[br_col]][br_col] = new TextField(10, 10, num.toString(), "BERNHC", 10);
			br.num[br_row[br_col]][br_col].color = color_;
			br.num[br_row[br_col]][br_col].alignPivot("center", "center");
			}
			
			if(pair_player){
				br.pair_p[br_row[br_col]][br_col] = new Image(player_pre_texture[3]);
				br.pair_p[br_row[br_col]][br_col].alignPivot("center", "center");
				br.pair_p[br_row[br_col]][br_col].scaleX = .7;
				br.pair_p[br_row[br_col]][br_col].scaleY = .73;
			}
			if(pair_banker){
				br.pair_b[br_row[br_col]][br_col] =  new Image(banker_pre_texture[3]);
				br.pair_b[br_row[br_col]][br_col].alignPivot("center", "center");
				br.pair_b[br_row[br_col]][br_col].scaleX = .7;
				br.pair_b[br_row[br_col]][br_col].scaleY = .73;
			}
			if(tie && !tie_multi){
				br.tie[br_row[br_col]][br_col] =  new Image(tie_texture);
				br.tie[br_row[br_col]][br_col].alignPivot("center", "center");
				br.tie[br_row[br_col]][br_col].scaleX = .7;
				br.tie[br_row[br_col]][br_col].scaleY = .73;
			}
			if(tie_multi){
						if(br_tie == 2){
				br.tie_[br_row[br_col]][br_col] = new TextField(10, 10, br_tie.toString(), "BERNHC", 10);
			br.tie_[br_row[br_col]][br_col].color = "0x000000";
			br.tie_[br_row[br_col]][br_col].alignPivot("center", "center");
						}
						else{
							br.tie_[br_row[br_col]][br_col].text = br_tie.toString();
						}
			}

					
				if(!tie){
				br.dot[br_row[br_col]][br_col].x = dots_placeX[br_col]  ;
				br.dot[br_row[br_col]][br_col].y =dots_placeY[br_row[br_col]];
				br.num[br_row[br_col]][br_col].x =  dots_placeX[br_col] -1;
				br.num[br_row[br_col]][br_col].y =dots_placeY[br_row[br_col]] -1;
				//cont_big_road_.addChild(br.dot[br_row[br_col]][br_col]);
				//cont_big_road_.addChild(br.num[br_row[br_col]][br_col]);
				temp_sprite.addChild(br.dot[br_row[br_col]][br_col]);
				temp_sprite.addChild(br.num[br_row[br_col]][br_col]);
				}
				
				
				if(pair_banker){
				br.pair_b[br_row[br_col]][br_col].x = dots_placeX[br_col] -5;
				br.pair_b[br_row[br_col]][br_col].y = dots_placeY[br_row[br_col]] -5;
				//cont_big_road_.addChild(br.pair_b[br_row[br_col]][br_col]);
				temp_sprite.addChild(br.pair_b[br_row[br_col]][br_col]);
					}
				if(pair_player){
				br.pair_p[br_row[br_col]][br_col].x = dots_placeX[br_col]+5;
				br.pair_p[br_row[br_col]][br_col].y = dots_placeY[br_row[br_col]] +5;
				//cont_big_road_.addChild(br.pair_p[br_row[br_col]][br_col]);
				temp_sprite.addChild(br.pair_p[br_row[br_col]][br_col]);
					}
				if(tie && !tie_multi){
			
				br.tie[br_row[br_col]][br_col].x = dots_placeX[br_col] ;
				br.tie[br_row[br_col]][br_col].y =  dots_placeY[br_row[br_col]]+2;
				//cont_big_road_.addChild(br.tie[br_row[br_col]][br_col]);
				temp_sprite.addChild(br.tie[br_row[br_col]][br_col]);
					}
				if(tie_multi){
				br.tie_[br_row[br_col]][br_col].x = dots_placeX[br_col] -6;
				br.tie_[br_row[br_col]][br_col].y =  dots_placeY[br_row[br_col]] +3;
				//cont_big_road_.addChild(br.tie_[br_row[br_col]][br_col]);
				temp_sprite.addChild(br.tie_[br_row[br_col]][br_col]);
				}
				br.space[br_row[br_col]][br_col]=true;
				br_dots[br_count]=temp_sprite;
				if(br_col>= 2 && !tie){
				big_eye_num(br_row[br_col],br_col);
						if(br_row[br_col] == 5){
				prediction_("big_eye",br_row[br_col],br_col+1);
						}
				else{
					prediction_("big_eye",br_row[br_col]+1,br_col);
				}
				}
				if(br_col>= 3 && !tie){
				small_road_num(br_row[br_col],br_col);
				
						if(br_row[br_col] == 5){
				prediction_("small_road",br_row[br_col],br_col+1);
						}
				else{
					prediction_("small_road",br_row[br_col]+1,br_col);
				}
				}
				if(br_col>= 4 && !tie){
				cockroach_road_num(br_row[br_col],br_col);
					if(br_row[br_col] == 5){
				prediction_("cockroach_road",br_row[br_col],br_col+1);
						}
				else{
					prediction_("cockroach_road",br_row[br_col]+1,br_col);
				}
				}
				
				cont_big_road_.addChild(br_dots[br_count]);
				if(!connect_.big_road_f){
				TweenLite.from(br_dots[br_count],.5,{alpha:0});
			}
				
				br_count +=1;
				br_row[br_col]+=1;
			}
		
		}
			private function prediction_(road:String,row:Number,col:Number): void {
					
				if(road == "big_eye"){
						if(row == 0){
					if(br_row[col-2] == br_row[col -1]){
						banker_pre_[0] = 'B';
					}
					else{
					banker_pre_[0] = 'R';
					}	
				
				}
				else{
						if(br.space[row][col - 1] == br.space[row - 1][col - 1]){
						banker_pre_[0]  = 'B';
						}
						else{
							banker_pre_[0]  = 'R';
						}
						
				}
			}
			if(road == "small_road"){
				if(row == 0){
					if(br_row[col-3] == br_row[col -1]){
						banker_pre_[1] = 'B';
					}
					else{
						banker_pre_[1] = 'R';
					}
				
				}
				else{
						if(br.space[row][col - 2] == br.space[row - 1][col - 2]){
							banker_pre_[1] = 'B';
						}
					else{
							banker_pre_[1]= 'R';
						}
						
				}
			}
			if(road == "cockroach_road"){
			if(row == 0){
					if(br_row[col-4] == br_row[col -1]){
						banker_pre_[2] = 'B';
					}
					else{
						banker_pre_[2] = 'R';
					}
				}
				else{
						if(br.space[row][col - 3] == br.space[row - 1][col - 3]){
							banker_pre_[2] = 'B';
						}
					else{
							banker_pre_[2] = 'R';
						}
				}
			}
			prediction();
			}
		private function prediction(): void {
			
				var temp:Number = 32;
			
			for (var i: Number = 0;i<3;i++){
				if(banker_pre_[i]=="R"){
					banker_pre[i] = new Image(banker_pre_texture[i]);
					player_pre[i] = new Image(player_pre_texture[i]);
				}
				else{
					banker_pre[i] = new Image(player_pre_texture[i]);
					player_pre[i] = new Image(banker_pre_texture[i]);
				}
			}
		while (this.cont_prediction.numChildren > 0) {
				this.cont_prediction.removeChildAt(0);
			}
			for (i=0; i < banker_pre_.length; i++) {
			
				banker_pre[i].alignPivot("center", "center");
				banker_pre[i].scaleX = .5;
				banker_pre[i].scaleY = .5;
				banker_pre[i].x  = temp;
				banker_pre[i].y  =  465 +banker_pre[i].height +2;
				cont_prediction.addChild(banker_pre[i]);
				temp +=banker_pre[i].width + 3;
					
				}
				
				temp = 107;
			
				for (i=0; i < banker_pre_.length; i++) {
				player_pre[i].alignPivot("center", "center");
			player_pre[i].scaleX = .5;
				player_pre[i].scaleY = .5;
					player_pre[i].x  = temp;
					player_pre[i].y  = 465 +player_pre[i].height +2;
					cont_prediction.addChild(player_pre[i]);
				temp +=player_pre[i].width + 3;
			
			}
					cont_big_road.addChild(cont_prediction);
		}
		
		
		
		
private function panel_win_counter(): void {
		//Win History
	
			
	for(var i:Number = 0 ; i < win_count.length ; i++){
			win_count_text[i] = new TextField(100, 19, "0", "BERNHC", 14);
			win_count_text[i].color = 0xFFFFFF;
			win_count_text[i].alignPivot("center", "center");
		win_count_text[i].hAlign = HAlign.CENTER;
			win_count_text[i].x = 345 ;
if(i == 0){			
		win_count_text[i].y = 476 ;
}
else{
			win_count_text[i].y = win_count_text[i-1].y + win_count_text[i].height ;
}
			cont_win_counter.addChild(win_count_text[i]);
	}
	this.addChild(cont_win_counter);
}
		private function panel_score(): void {
		
			score = new Image(Assets.getAtlas().getTexture("cont_score instance 10000"));
			score.alignPivot("center", "center");
			score.scaleX = .7;
			score.scaleY = .7;
			
			cont_score.addChild(score);
			
			
			result = new Image(Assets.getAtlas().getTexture("result instance 10000"));
			result.alignPivot("center", "center");
			result.scaleX = .7;
			result.scaleY = .5;
			result.x = score.x ;
			result.y = score.y + 45;
			result.alpha = 0;
			cont_score.addChild(result);

			
			result_text = new TextField(500, 25, "", "newFont", 15);
			result_text.color = 0x000000;
			result_text.alignPivot("center", "center");
			result_text.x = result.x ;
			result_text.y = result.y ;
			result_text.alpha = 0;
			cont_score.addChild(result_text);
			
			player_text = new TextField(100, 20, "0", "BERNHC", 22);
			player_text.color = 0xFFFFFF;
			player_text.alignPivot("center", "center");
			player_text.x = score.x - 35;
			player_text.y = score.y - 42;

			cont_score.addChild(player_text);

			banker_text = new TextField(100, 20, "0", "BERNHC", 22);
			banker_text.color = 0xFFFFFF;
			banker_text.alignPivot("center", "center");
			banker_text.x = score.x + 35;
			banker_text.y = score.y - 42;
		
			cont_score.addChild(banker_text);
			cont_score.visible = false;
			cont_score.alignPivot("center", "center");
			cont_score.x = 150;
			cont_score.y = 100;
			
			this.addChild(cont_score);

			//TweenLite.from(cont_score,.5,{alpha:0})
			//drawCards("player","heart",9,0);
		}

		private function drawCards(draw: String, card: String, n: Number, i: Number): void {
			if (n >= 10 && n <= 13) {
				if (draw == "Player") {
					//player.push(gameSettings.cards_value[9]);
					player_score += gameSettings.cards_value[9];
				} else {
					//banker.push(gameSettings.cards_value[9]);
					banker_score += gameSettings.cards_value[9];
				}
			} else {
				if (draw == "Player") {
					//player.push(gameSettings.cards_value[n-1]);
					player_score += gameSettings.cards_value[n - 1];
				} else {
					//banker.push(gameSettings.cards_value[n-1]);
					banker_score += gameSettings.cards_value[n - 1];
				}
			}
			if (player_score > 9) {
				player_score -= 10;
			}
			if (banker_score > 9) {
				banker_score -= 10;
			}

			player_text.text = player_score.toString();
			banker_text.text = banker_score.toString();

			if (card == "heart") {
				cards_texture[i] = Assets.getAtlas1().getTexture(gameSettings.h_cards_name[n - 1]);
				cards[i] = new Image(cards_texture[i]);
			} else if (card == "spade") {

				cards_texture[i] = Assets.getAtlas1().getTexture(gameSettings.s_cards_name[n - 1]);
				cards[i] = new Image(cards_texture[i]);

			} else if (card == "diamond") {
				cards_texture[i] = Assets.getAtlas1().getTexture(gameSettings.d_cards_name[n - 1]);
				cards[i] = new Image(cards_texture[i]);

			} else if (card == "club") {
				cards_texture[i] = Assets.getAtlas1().getTexture(gameSettings.c_cards_name[n - 1]);
				cards[i] = new Image(cards_texture[i]);
			}
			cards[i].alignPivot("center", "center");
			cards[i].scaleX = gameSettings.cards_scale;
			cards[i].scaleY = gameSettings.cards_scale;
			cards[i].alpha = 1;
			cards[i].x = 900;
			cards[i].y = 250;

			if (i == 0 && draw == "Player") {
				cards[i].x = cardx_holder[2];
				cards[i].y = cardy_holder[2];
			} else if (i == 1 && draw == "Banker") {
				cards[i].x = cardx_holder[3];
				cards[i].y = cardy_holder[3];
			} else if (i == 2 && draw == "Player") {
				cards[i].x = cardx_holder[1];
				cards[i].y = cardy_holder[1];
			} else if (i == 3 && draw == "Banker") {
				cards[i].x = cardx_holder[4];
				cards[i].y = cardy_holder[4];
			} else if (i == 4 && draw == "Player") {
				cards[i].alpha = 0;
				cards[i].x = 900;
				cards[i].y = 250;
				if(!connect_.drew_f){
				TweenLite.to(cards[i], .5, {
					x: cardx_holder[0],
					y: cardy_holder[0],
					alpha: 1,
					rotation: -1.57
				});
						}
			else{
				cards[i].alpha = 1;		
				cards[i].rotation = -1.57;
				cards[i].x = cardx_holder[0];
				cards[i].y = cardy_holder[0];
			}
			} else if (i == 4 && draw == "Banker") {
				cards[i].alpha = 0;
				cards[i].x = 900;
				cards[i].y = 250;
if(!connect_.drew_f){
				TweenLite.to(cards[i], .5, {
					x: cardx_holder[5],
					y: cardy_holder[5],
					alpha: 1,
					rotation: -1.57
				});
					}
			else{
				cards[i].alpha = 1;			
				cards[i].rotation = -1.57;
				cards[i].x = cardx_holder[5];
				cards[i].y = cardy_holder[5];
			}
			} else if (i == 5 && draw == "Banker") {
				cards[i].alpha = 0;
				cards[i].x = 900;
				cards[i].y = 250;
			if(!connect_.drew_f){
				TweenLite.to(cards[i], .5, {
					x: cardx_holder[5],
					y: cardy_holder[5],
					alpha: 1,
					rotation: -1.57
				});
			}
			else{
				cards[i].alpha = 1;		
				cards[i].rotation = -1.57;
				cards[i].x = cardx_holder[5];
				cards[i].y = cardy_holder[5];
			}
			}
			if(!connect_.drew_f){
			if (i >= 0 && i <= 3)
				TweenLite.from(cards[i], .5, {
					x: 900,
					y: 250,
					alpha: 0
				});
			}
			cont_score.addChild(cards[i]);
		}
		
		private function panel_timer(): void {
			timer = new Image(Assets.getAtlas().getTexture("cont_timer instance 10000"));
			timer.alignPivot("center", "center");
			timer.height = 100;
			timer.width = 100;
			timer.alpha = .5;
			cont_timer.addChild(timer);

			timer_text = new TextField(100, 100, "", "BERNHC", 50, 0xFFFFFF, true);
			timer_text.alignPivot("center", "center");

			cont_timer.addChild(timer_text);
			cont_timer.scaleX = .9;
			cont_timer.scaleY = .9;
			cont_timer.x = 950;
			cont_timer.y = 300;
			cont_timer.visible = false;
			this.addChild(cont_timer);

			//TweenLite.from(cont_timer,.3,{alpha:0, ease: Linear.easeIn,scaleX:cont_timer.scaleX +.2,scaleY:cont_timer.scaleY +.2})

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


				bet_result[i].alpha = 0;
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
				bet_text[i] = new TextField(100, 20, "0", "BERNHC", 12);
				bet_text[i].color = "0xFFFFFF";
				bet_text[i].alignPivot("center", "center");
				if (i == 5) {
					bet_text[i].x = bet_hover[i].x + bets_loc[i];
					bet_text[i].y = bet_hover[i].y - bet_hover[i].height - 10;
				} else {
					bet_text[i].x = bet_hover[i].x + bets_loc[i];
					bet_text[i].y = bet_hover[i].y + 10;
				}

				cont_upper.addChild(bet_upper[i]);
				cont_upper.addChild(bet_text[i]);
				cont_betArea.addChild(cont_upper);
			}


			this.addChild(cont_betArea);
			this.addChild(cont_hover_result);
			setChildIndex(cont_hover_result, 2);

			this.addChild(cont_upper);
			//setChildIndex(cont_upper, numChildren - 1);


		}

		private function panel_selection(): void {

			cont_mask = new Quad(278,65,0xffffff); 
			cont_mask.x = 382;
			cont_mask.y = stage.stageHeight - 105;

			this.addChild(cont_mask);

			drawCoin();
			//Buttons
		
			btn_back = new Image(back_texture);
			btn_back.alignPivot("right", "center");
			btn_back.scaleX = .8;
			btn_back.scaleY = .8;
			btn_back.x = cont_mask.x ;
			btn_back.y = cont_mask.y + (cont_mask.height/2);
			btn_back.visible = false;
			cont_btn_back.addChild(btn_back);
			
			
			btn_back_disabled = new Image(back_disabled_texture);
			btn_back_disabled.alignPivot("right", "center");
			btn_back_disabled.scaleX = .8;
			btn_back_disabled.scaleY = .8;
			btn_back_disabled.x = cont_mask.x ;
			btn_back_disabled.y = cont_mask.y + (cont_mask.height/2);
			cont_btn_back.addChild(btn_back_disabled);
			cont_select.addChild(cont_btn_back);
		
			btn_next = new Image(next_texture);
			btn_next.alignPivot("left", "center");
			btn_next.scaleX = .8;
			btn_next.scaleY = .8;
			btn_next.x = cont_mask.x + cont_mask.width ;
			btn_next.y = cont_mask.y + (cont_mask.height/2);
			cont_btn_next.addChild(btn_next);
			
			
			btn_next_disabled = new Image(next_disabled_texture);
			btn_next_disabled.alignPivot("left", "center");
			btn_next_disabled.scaleX = .8;
			btn_next_disabled.scaleY = .8;
			btn_next_disabled.x = cont_mask.x + cont_mask.width ;
			btn_next_disabled.y = cont_mask.y + (cont_mask.height/2);
			btn_next_disabled.visible = false;
			cont_btn_next.addChild(btn_next_disabled);
			cont_select.addChild(cont_btn_next);

			//Balance

			total_balance_text = new TextField(200, 50, "0", "BERNHC", 18);
			total_balance_text.color = 0xFFFF00;
			total_balance_text.alignPivot("left", "center");
			total_balance_text.x = 388 ;
			total_balance_text.y = 482 ;
			total_balance_text.text = total_balance.toString();
			total_balance_text.hAlign = HAlign.LEFT;
			this.addChild(total_balance_text);

			//Main Buttons
			
			btn_bet = new Image(Assets.getAtlas().getTexture("send3.png instance 1"));
			btn_bet.alignPivot("center", "center");
			btn_bet.scaleX = .6;
			btn_bet.scaleY = .56;
			btn_bet.x = btn_back.x + 37;
			btn_bet.y = stage.stageHeight - btn_bet.height + 8;
			cont_btn_bet.addChild(btn_bet);
			
			btn_bet_text = new Image(Assets.getAtlas().getTexture("send2.png instance 1"));
			btn_bet_text.alignPivot("center", "center");
			btn_bet_text.scaleX = .6;
			btn_bet_text.scaleY = .56;
			btn_bet_text.x = btn_bet.x;
			btn_bet_text.y = btn_bet.y;
			btn_bet_text.visible = true;
			cont_btn_bet.addChild(btn_bet_text);
			cont_select.addChild(cont_btn_bet);

			/*btn_bet_text_ = new TextField(100, 20, "0", "newFont", 16);
			btn_bet_text_.color = 0xFFFFFF;
			btn_bet_text_.alignPivot("center", "center");
			btn_bet_text_.x = btn_bet.x;
			btn_bet_text_.y = btn_bet.y;
			btn_bet_text_.text = "提交";
			cont_btn_bet.addChild(btn_bet_text_);*/
		
			btn_bet_lock = new Image(Assets.getAtlas().getTexture("send1.png instance 1"));
			btn_bet_lock.alignPivot("center", "center");
			btn_bet_lock.scaleX = .6;
			btn_bet_lock.scaleY = .56;
			btn_bet_lock.x = btn_bet.x;
			btn_bet_lock.y = btn_bet.y;
			btn_bet_lock.visible = false;
			cont_btn_bet.addChild(btn_bet_lock);
			cont_select.addChild(cont_btn_bet);

			
			btn_clear = new Image(Assets.getAtlas().getTexture("clean3.png instance 1"));
			btn_clear.alignPivot("center", "center");
			btn_clear.scaleX = .6;
			btn_clear.scaleY = .56;
			btn_clear.x = btn_bet.x + btn_bet.width +2 ;
			btn_clear.y = stage.stageHeight - btn_clear.height + 8;
			btn_clear.visible = false;
			cont_btn_clear.addChild(btn_clear);
			
			btn_clear_text = new Image(Assets.getAtlas().getTexture("clean2.png instance 1"));
			btn_clear_text.alignPivot("center", "center");
			btn_clear_text.scaleX = .6;
			btn_clear_text.scaleY = .56;
			btn_clear_text.x = btn_clear.x;
			btn_clear_text.y = btn_clear.y;
			btn_clear_text.visible = false;
			cont_btn_clear.addChild(btn_clear_text);


			btn_clear_lock = new Image(Assets.getAtlas().getTexture("clean1.png instance 1"));
			btn_clear_lock.alignPivot("center", "center");
			btn_clear_lock.scaleX = .6;
			btn_clear_lock.scaleY = .56;
			btn_clear_lock.x = btn_clear.x;
			btn_clear_lock.y = btn_clear.y;
			btn_clear_lock.visible = true;
			cont_btn_clear.addChild(btn_clear_lock);

			cont_select.addChild(cont_btn_clear);

		
			btn_auto = new Image(Assets.getAtlas().getTexture("auto3.png instance 1"));
			btn_auto.alignPivot("center", "center");
			btn_auto.scaleX = .6;
			btn_auto.scaleY = .56;
			btn_auto.x = btn_clear.x + btn_clear.width + 1;
			btn_auto.y = stage.stageHeight - btn_auto.height + 8;
			cont_btn_auto.addChild(btn_auto);
			
			btn_auto_text = new Image(Assets.getAtlas().getTexture("auto1.png instance 1"));
			btn_auto_text.alignPivot("center", "center");
			btn_auto_text.scaleX = .6;
			btn_auto_text.scaleY = .56;
			btn_auto_text.x = btn_auto.x;
			btn_auto_text.y = btn_auto.y;
			btn_auto_text.visible = true;
			cont_btn_auto.addChild(btn_auto_text);
			
			btn_auto_lock = new Image(Assets.getAtlas().getTexture("auto2.png instance 1"));
			btn_auto_lock.alignPivot("center", "center");
			btn_auto_lock.scaleX = .6;
			btn_auto_lock.scaleY = .56;
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
					chip[i].x = cont_mask.x + 30;
					chip[i].y = cont_mask.y + 43;
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
				if (gameSettings.chip_value[i] == gameSettings.chip_selected){
					TweenLite.to(chip[i], .3, {
						//scaleX: gameSettings.chip_scale_anim,
						//scaleY: gameSettings.chip_scale_anim,
						y: chip_anim_up
						//ease: Bounce.easeOut
					});
					
				}
			}
		}


		private function onTouchBackAnim(): void {
			for (var i: Number = 0; i < gameSettings.chip_name.length; i++) {
				if (gameSettings.chip_selected == gameSettings.chip_value[i])
					TweenLite.to(chip[i], .3, {
						scaleX: gameSettings.chip_scale,
						scaleY: gameSettings.chip_scale,
						y: chip_anim_down
						//ease: Linear.easeOut
					});
			}

		}
		private function onTouch(e: TouchEvent): void {

			//------------------chip select------------------------------
			for (var i: Number = 0; i < gameSettings.chip_name.length; i++) {
				if (e.getTouch(chip[i], TouchPhase.BEGAN)) {
					if (gameSettings.chip_selected != gameSettings.chip_value[i]) {
						onTouchBackAnim();
						TweenLite.to(chip[i], .1, {
							//scaleX: gameSettings.chip_scale_anim,
							//scaleY: gameSettings.chip_scale_anim,
							y: chip_anim_up
							//ease: Bounce.easeOut
						});
						gameSettings.chip_selected = gameSettings.chip_value[i];
					}
				}
			}
			//------------------------------------------------------
			
			
			if (e.getTouch(cont_btn_next, TouchPhase.BEGAN)) {
				// click code goes here
				if (cont_selection.x > (-cont_selection.width + 270)) {
					TweenLite.to(cont_selection, .5, {
						x: cont_selection.x - 50
					});
				
					
				} else {
					TweenLite.to(cont_selection, .5, {
						x: (-cont_selection.width + 270),
						ease: Elastic.easeOut
					});
					btn_next.visible = false;
					btn_next_disabled.visible = true;
					
				}
			}

			if (e.getTouch(cont_btn_back, TouchPhase.BEGAN)) {
				// click code goes here
		
				if (cont_selection.x <0) {
					TweenLite.to(cont_selection, .5, {
						x: cont_selection.x + 50
					});
				
				} else {
					TweenLite.to(cont_selection, .5, {
						x: 0,
						ease: Elastic.easeOut
					});
					btn_back.visible = false;
					btn_back_disabled.visible = true;
				}
			}
			
			if(cont_selection.x<0 && cont_selection.x > (-cont_selection.width + 270)){
					btn_next.visible = true;
					btn_next_disabled.visible = false;
					btn_back.visible = true;
					btn_back_disabled.visible = false;
			}
			if(cont_selection.x >0){
				TweenLite.to(cont_selection, .5, {
						x: 0,
						ease: Elastic.easeOut
					});
					btn_back.visible = false;
					btn_back_disabled.visible = true;
			}
			
			if(cont_selection.x < (-cont_selection.width + 270)){
					TweenLite.to(cont_selection, .5, {
						x: (-cont_selection.width + 270),
						ease: Elastic.easeOut
					});
					btn_next.visible = false;
					btn_next_disabled.visible = true;
				
			}


			//-----------------------Hover-----------------------------------

			for (i = 0; i < gameSettings.chip_name.length; i++) {

				if (e.getTouch(chip[i], TouchPhase.HOVER)) {
					// rollover code goes here	
					if (gameSettings.chip_selected != gameSettings.chip_value[i]) {
					TweenLite.to(chip[i], .5, {
						//scaleX: gameSettings.chip_scale_anim,
						//scaleY: gameSettings.chip_scale_anim,
						y: chip_anim_mid,
						ease: Elastic.easeOut
					});
				}

				} else {
					if (gameSettings.chip_selected != gameSettings.chip_value[i])
					// rollout code goes here
						TweenLite.to(chip[i], .5, {
							//scaleX: gameSettings.chip_scale,
							//scaleY: gameSettings.chip_scale,
							y: chip_anim_down,
							ease: Elastic.easeOut
						});
				}
			}
			//if (e.getTouch(btn_back, TouchPhase.HOVER)) {
			//	// rollover code goes here
			//	TweenLite.to(btn_back, .8, {
			//		scaleX: .9,
			//		scaleY: .9,
			//		ease: Strong.easeOut
			//	});

			//	if (cont_selection.x < 0) {
			//		TweenMax.to(cont_selection, .5, {
			//			x: cont_selection.x + 30
			//		});
			//	} else {
			//		TweenMax.to(cont_selection, .5, {
			//			x: 0,
			//			ease: Elastic.easeOut
			//		});
			//	}

			//} else {
			//	// rollout code goes here
			//	TweenLite.to(btn_back, .5, {
			//		scaleX: .8,
			//		scaleY: .8,
			//		ease: Linear.easeNone
			//	});
			//}

			//if (e.getTouch(btn_next, TouchPhase.HOVER)) {
			//	// rollover code goes here
			//	TweenLite.to(btn_next, .8, {
			//		scaleX: .9,
			//		scaleY: .9,
			//		ease: Strong.easeOut
			//	});

			//	if (cont_selection.x > (-cont_selection.width + 270)) {
			//		TweenMax.to(cont_selection, .5, {
			//			x: cont_selection.x - 30
			//		});
			//	} else {
			//		TweenMax.to(cont_selection, .5, {
			//			x: (-cont_selection.width + 270),
			//			ease: Linear.easeNone
			//		});
			//	}

			//} else {
			//	// rollout code goes here
			//	TweenLite.to(btn_next, .5, {
			//		scaleX: .8,
			//		scaleY: .8,
			//		ease: Strong.easeOut
			//	});
			//}
			
			if(br_col >15){
				if (e.getTouch(cont_big_road_all, TouchPhase.HOVER)) {
				TweenLite.to(br_btn_next_, .5, {
					alpha: 1
				});

			TweenLite.to(br_btn_back_, .5, {
					alpha: 1
				});

			}
				else {
				// rollout code goes here
					TweenLite.to(br_btn_next_, .5, {
					alpha: 0
				});

			TweenLite.to(br_btn_back_, .5, {
					alpha: 0
				});
			}
			
				if (e.getTouch(br_btn_next_, TouchPhase.BEGAN)) {
					if(cont_big_road_.x >=((dots_placeX[br_col] * -1)+260) )
					cont_big_road_.x -=17.6;
						else 
							br_btn_next_.alpha = 0;
				}
				if (e.getTouch(br_btn_back_, TouchPhase.BEGAN)) {
	if(cont_big_road_.x <=dots_placeX[0] - 17)					
					cont_big_road_.x +=17.6;
	else
		br_btn_back_.alpha = 0;
				}
		}
		
			if(mr_col >6){
				
				if (e.getTouch(cont_marker_road_all, TouchPhase.HOVER)) {
				// rollover code goes here
				TweenLite.to(mr_btn_next_, .5, {
					alpha: 1
				});

			TweenLite.to(mr_btn_back_, .5, {
					alpha: 1
				});
				
			} 
				else {
					TweenLite.to(mr_btn_next_, .5, {
					alpha: 0
				});

			TweenLite.to(mr_btn_back_, .5, {
					alpha: 0
				});
				}
					if (e.getTouch(mr_btn_next_, TouchPhase.BEGAN)) {	
					if(cont_marker_road_.x >((18.2 * (mr_col -6)) * -1) )
					cont_marker_road_.x -=17.7;
					else
						mr_btn_next_.alpha = 0;
				}
				
						
		if (e.getTouch(mr_btn_back_, TouchPhase.BEGAN)) {
	if(cont_marker_road_.x <0)					
					cont_marker_road_.x +=17.7;
					else
						mr_btn_back_.alpha = 0;
				}
		}
		
		if(be_col >24){
			
				if (e.getTouch(cont_big_eye_all, TouchPhase.HOVER)) {
				// rollover code goes here
				TweenLite.to(be_btn_next_, .5, {
					alpha: 1
				});

			TweenLite.to(be_btn_back_, .5, {
					alpha: 1
				});
				
			} 
				else {
					TweenLite.to(be_btn_next_, .5, {
					alpha: 0
				});

			TweenLite.to(be_btn_back_, .5, {
					alpha: 0
				});
				}
					if (e.getTouch(be_btn_next_, TouchPhase.BEGAN)) {
					if(cont_big_eye_.x >= -(5.9 *  (be_col - 24)) )
					cont_big_eye_.x -=5.8;
					else
						be_btn_next_.alpha = 0;
				}
				
						
		if (e.getTouch(be_btn_back_, TouchPhase.BEGAN)) {
	if(cont_big_eye_.x <-5.9)					
					cont_big_eye_.x +=5.8;
					else
						be_btn_back_.alpha = 0;
				}
		}
		
		if(sr_col >24){
			
				if (e.getTouch(cont_small_road_all, TouchPhase.HOVER)) {
				// rollover code goes here
				TweenLite.to(sr_btn_next_, .5, {
					alpha: 1
				});

			TweenLite.to(sr_btn_back_, .5, {
					alpha: 1
				});
				
			} 
				else {
					TweenLite.to(sr_btn_next_, .5, {
					alpha: 0
				});

			TweenLite.to(sr_btn_back_, .5, {
					alpha: 0
				});
				}
					if (e.getTouch(sr_btn_next_, TouchPhase.BEGAN)) {
					if(cont_small_road_.x >= -(5.9 *  (sr_col - 24)) )
					cont_small_road_.x -=5.8;
					else
						sr_btn_next_.alpha = 0;
				}
				
						
		if (e.getTouch(sr_btn_back_, TouchPhase.BEGAN)) {
	if(cont_small_road_.x <-5.9)					
					cont_small_road_.x +=5.8;
					else
						sr_btn_back_.alpha = 0;
				}
		}
		if(cr_col >24){
			
				if (e.getTouch(cont_cockroach_road_all, TouchPhase.HOVER)) {
				// rollover code goes here
				TweenLite.to(cr_btn_next_, .5, {
					alpha: 1
				});

			TweenLite.to(cr_btn_back_, .5, {
					alpha: 1
				});
				
			} 
				else {
					TweenLite.to(cr_btn_next_, .5, {
					alpha: 0
				});

			TweenLite.to(cr_btn_back_, .5, {
					alpha: 0
				});
				}
					if (e.getTouch(cr_btn_next_, TouchPhase.BEGAN)) {
					if(cont_cockroach_road_.x >= -(5.9 *  (cr_col - 24)) )
					cont_cockroach_road_.x -=5.8;
					else
						cr_btn_next_.alpha = 0;
				}
				
						
		if (e.getTouch(cr_btn_back_, TouchPhase.BEGAN)) {
	if(cont_cockroach_road_.x <-5.9)					
					cont_cockroach_road_.x +=5.8;
					else
						cr_btn_back_.alpha = 0;
				}
		}
		
			//--------------------BET AREA------------------------
if (con_timer != 0) {
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

				if (!r){
					for (t = 0; t < chip.length; t++) {
						//bet_chip_mouse[t].visible = true;
						bet_chip_mouse[t].x = 0;
						bet_chip_mouse[t].y = 0;
						bet_chip_mouse[t].alpha = 0;
					}

			}
		
		else{for (i = 0; i < gameSettings.bet_tag.length; i++) {
				TweenLite.to(bet_hover[i], .3, {
						alpha: 0
					});
				}
					for (t = 0; t < chip.length; t++) {
						//bet_chip_mouse[t].visible = true;
						bet_chip_mouse[t].x = 0;
						bet_chip_mouse[t].y = 0;
						bet_chip_mouse[t].alpha = 0;
					}
		}
	}
}
if (con_timer == 0) {
	for (t = 0; t < chip.length; t++) {
						//bet_chip_mouse[t].visible = true;
						bet_chip_mouse[t].x = 0;
						bet_chip_mouse[t].y = 0;
						bet_chip_mouse[t].alpha = 0;
					}
						
					
						btn_bet.visible = false;
			btn_bet_text.visible = false;
			btn_bet_lock.visible = true;
					
}

			//--------------------Bet Click-----------------
		
			if (e.getTouch(cont_btn_auto, TouchPhase.BEGAN)) {
				auto_event();
			}
		
			if (con_timer != 0) {
				
			if (e.getTouch(cont_btn_bet, TouchPhase.BEGAN)) {
				if (connect_.status_ == "Connected" && con_timer != 0) 
					bet_event();
			}
				if (e.getTouch(cont_btn_clear, TouchPhase.BEGAN)) {
				clear_event();
			}
				for (i = 0; i < gameSettings.bet_tag.length; i++) {


					if (e.getTouch(bet_upper[i], TouchPhase.ENDED)) {

						//	for (t = 0; t < gameSettings.chip_name.length; t++) {
						//if (gameSettings.chip_selected == gameSettings.chip_value[t]) {
						//	chip_deck(i, t);
						btn_clear.visible = true;
						btn_clear_text.visible = true;
						btn_clear_lock.visible = false;
						temp_chip_selected = gameSettings.chip_selected;
						if (temp_bal > 0) {

							if (temp_chip_selected > temp_bal) {
								temp_chip_selected = temp_bal;

							}
							temp_bal -= temp_chip_selected;
							if (temp_bal >= 0) {
								bets[i] += temp_chip_selected;
								temp_add_bets[i] += temp_chip_selected;
								//temp_bets[i] = bets[i];
								total_bets += temp_chip_selected;
								//btn_bet_text_.text = "提交(" + total_bets + ")";
								total_balance_text.text = temp_bal.toString();
								//trace(bets[i] + " - total= " + total_bets);
								chip_deck_check(i, bets[i]);
								
							} else {
								temp_chip_selected = gameSettings.chip_selected;
							}
						}
						if (auto_submit) {
							bet_event();
						}
					}
				}
			}
		}
	
		private function chip_deck(i: Number, t: Number): void {
			
			if (cont_chip_.numChildren != 0 && i == 4) {
				this.cont_chip_.removeChildAt(this.cont_chip_.numChildren - 2);
				this.cont_chip_.removeChildAt(this.cont_chip_.numChildren - 1);
			}
		
			this.removeChild(bet_chip_text[i]);
			bet_chip_text[i] = null;
			this.removeChild(bet_chip_add_text[i]);
			bet_chip_add_text[i] = null;


			o.bet_[i][bets_[i]] = new Image(bet_chip_texture[t]);
			o.bet_[i][bets_[i]].scaleX = .6;
			o.bet_[i][bets_[i]].scaleY = .6;
			o.bet_[i][bets_[i]].x = bet_upper[i].x - (bet_upper[i].width / 2) + bets_chip_loc[i];
			o.bet_[i][bets_[i]].y = bet_upper[i].y - bet_upper[i].height - (bets_[i] * 3);
			(i == 4) ? cont_chip_.addChild(o.bet_[i][bets_[i]]) : this.addChild(o.bet_[i][bets_[i]]);

			bet_chip_text[i] = new TextField(100, 20, "", "BERNHC", 12);
			bet_chip_text[i].color = "0x000000";
			bet_chip_text[i].alignPivot("center", "center");
			bet_chip_text[i].x = o.bet_[i][bets_[i]].x + (o.bet_[i][bets_[i]].width / 2);
			bet_chip_text[i].y = o.bet_[i][bets_[i]].y + (o.bet_[i][bets_[i]].height / 2);
			(win_status) ? bet_chip_text[i].text = (bets[i] + (bet_value[i] *bets[i])).toString() :  bet_chip_text[i].text = bets[i] ;
			
			(i == 4) ? cont_chip_.addChild(bet_chip_text[i]) : this.addChild(bet_chip_text[i]);

			bet_chip_add_text[i] = new TextField(100, 20, "", "BERNHC", 14);
			bet_chip_add_text[i].color = "0xFFFF00";
			bet_chip_add_text[i].alignPivot("center", "center");
			bet_chip_add_text[i].x = o.bet_[i][bets_[i]].x + (o.bet_[i][bets_[i]].width / 2);
			bet_chip_add_text[i].y = o.bet_[i][bets_[i]].y - (o.bet_[i][bets_[i]].height / 4);
			
			(win_status) ? bet_chip_add_text[i].text = "+" +(bet_value[i] *bets[i]).toString() :  bet_chip_add_text[i].text ="+" + temp_add_bets[i];
			(i == 4) ? cont_chip_.addChild(bet_chip_add_text[i]) : this.addChild(bet_chip_add_text[i]);

			//TweenLite.from(o.bet_[i][bets_[i]],.2,{y:o.bet_[i][bets_[i]].y - 3 });
			
		if(interval_count==1 && win_status){
			TweenLite.from(o.bet_[i][bets_[i]],.3,{x:500,y: 200,alpha:0});
			TweenLite.from(bet_chip_text[i],.3,{x:500,y: 200,alpha:0});
			TweenLite.from(bet_chip_add_text[i],.3,{x:500,y: 200,alpha:0});
		}
		
			bets_[i] += 1;

			if (cont_chip_.numChildren != 0) {
				this.addChild(cont_chip_);
				setChildIndex(this.cont_chip_, numChildren - 1);
			}

		}



		private function chip_deck_check(i: Number, b: Number): void {

			var temp: Number;
			var m: Number;
			var l:Number;
					if( !win_status){
			for (m = 0; m < bets_[i]; m++) {
				this.removeChild(o.bet_[i][m]);
				o.bet_[i][m] = null;
			}
				if (i == 4 && cont_chip_.numChildren != 0) {
				while (this.cont_chip_.numChildren > 0) {
					this.cont_chip_.removeChildAt(0);
				}
			}
				bets_[i] = 0;
		}

		
		
			for (var t: Number = gameSettings.chip_name.length - 1; t >= 0; t--) {
				if (b >= gameSettings.chip_value[t]) {

					temp = b / gameSettings.chip_value[t];
					if (b >= 100000) {
						chip_deck(i, t);
					} else {

								for ( l = 0; l < Math.floor(temp); l++) {
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
			p.bet_[i][bets_Area[i]].x = bet_upper[i].x + randRange(bets_Min_X[i], bets_Max_X[i]);
			p.bet_[i][bets_Area[i]].y = bet_upper[i].y - 30 + randRange(bets_Min_Y[i], bets_Max_Y[i]);
			
			if(!win_status && con_timer!=0){
			TweenLite.from(p.bet_[i][bets_Area[i]], .2, {
				alpha: 0,
				delay: Math.random() * .6,
				y: p.bet_[i][bets_Area[i]].y - 100,
				ease: Bounce.easeOut
			});
		}
		
		if(interval_count==1 && win_status){
			TweenLite.from(p.bet_[i][bets_Area[i]],.5,{x:500,y: 200,alpha:0,delay: .5+Math.random() * .6});
		}
		
		this.addChild(p.bet_[i][bets_Area[i]]);
			bets_Area[i] += 1;
			
		}

		private function chip_bet_check(i: Number, b: Number): void {
			
			var temp: Number;

			for (var t: Number = gameSettings.chip_name.length - 1; t >= 0; t--) {
				if (b >= gameSettings.chip_value[t]) {

					temp = b / gameSettings.chip_value[t];
					if (b >= 100000) {
						chip_bet(i, t);
					} else {
						for (var l: Number = 0; l < Math.floor(temp); l++) {
							chip_bet(i, t);
						}
					}
					b = b % gameSettings.chip_value[t];

				}
			}

			setChildIndex(cont_upper, numChildren - 1);
		}

		private function randRange(minNum: Number, maxNum: Number): Number {
			return (Math.floor(Math.random() * (maxNum - minNum + 1)) + minNum);
		}
	


		private function bet_event(): void {
			
		if (temp_bal != total_balance) {
			 btn_clear.visible = false;
			btn_clear_text.visible = false;
			btn_clear_lock.visible = true;
			total_balance = temp_bal ;
			total_balance_text.text = total_balance.toString();
			//btn_bet_text_.text = "提交";
			
				
				//remove Child
				if (cont_chip_.numChildren != 0 && bets[4] != temp_bets[4])
					this.cont_chip_.removeChildAt(this.cont_chip_.numChildren - 1);

				for (var i: Number = 0; i < gameSettings.bet_tag.length; i++) {
					this.removeChild(bet_chip_add_text[i]);
					//----write
					temp_tbl[i] += temp_add_bets[i];
					//bet_text[i].text = bets[i];
					//chip_bet_check(i,temp_add_bets[i]);
					temp_add_bets[i] = 0;
					//bets[i] = 0;
				}
				//---

				connect_.websocket.sendUTF(JSON.stringify({
					function_: "Bet",
					ID:connect_.guid,
					total_bet:total_bets,
					bet: {
						0: temp_tbl[0],
						1: temp_tbl[1],
						2: temp_tbl[2],
						3: temp_tbl[3],
						4: temp_tbl[4],
						5: temp_tbl[5],
						6: temp_tbl[6],
						7: temp_tbl[7],
						8: temp_tbl[8],
						9: temp_tbl[9]
					}
				}));
				total_bets = 0;
				//	connect_.socket.send(JSON.stringify({amount: total_balance}));
				temp_bets = bets.concat();

				//---send to server
				//connect_.socket.send(temp_bal);
				//	if(connect_.status_ == "Connected"){
				//total_balance_text.text = connect_.bet_details +"ok";
				//	} 

			} else {
				//	trace("not");
			}
		}

		private function clear_event(): void {
			btn_clear.visible = false;
			btn_clear_text.visible = false;
			btn_clear_lock.visible = true;
			temp_bal = total_balance;
			total_balance_text.text = total_balance.toString();
			//btn_bet_text_.text = "提交";
			total_bets = 0;
			//temp_bets_ 
			for (var i: Number = 0; i < bets_.length; i++) {
				bets[i] = temp_bets[i];
				temp_add_bets[i] = 0;
				chip_deck_check(i, temp_bets[i]);
				if (bets[i] == 0){
					this.removeChild(bet_chip_text[i]);
					bet_chip_text[i] = null;
				}
				this.removeChild(bet_chip_add_text[i]);
				bet_chip_add_text[i] = null;
				bet_text[i].text = temp_live[i];
			//	bets_Area[i] = 0;

			}

			if (cont_chip_.numChildren != 0)
				this.cont_chip_.removeChildAt(this.cont_chip_.numChildren - 1);
			

		}

		private function auto_event(): void {
			if (auto_submit) {
				auto_submit = false;
				if(con_timer !=0){
				btn_bet.visible = true;
					btn_bet_text.visible = true;
					btn_bet_lock.visible = false;
				}
				//btn_clear.visible = true;
				//btn_auto.visible = true;
				
			
				//btn_clear_text.visible = true;
				btn_auto_text.visible = true;
				
				//btn_clear_lock.visible = false;
				btn_auto_lock.visible = false;
				//TweenMax.staggerTo([btn_bet,btn_clear,btn_auto],.3,{alpha:0});
				//	TweenMax.staggerTo([btn_bet_lock,btn_clear_lock,btn_auto_lock],.3,{alpha:1,ease: Elastic.easeOut});
			} else {
				auto_submit = true;
				if(con_timer !=0){
					btn_bet.visible = false;
					btn_bet_text.visible = false;
				}
				
				btn_clear.visible = false;
				//btn_auto.visible = false;
				
				btn_clear_text.visible = false;
				btn_auto_text.visible = false;
				btn_bet_lock.visible = true;
				//btn_clear_lock.visible = true;
				btn_auto_lock.visible = true;
				if(total_balance != temp_bal)
				bet_event();
			}
		}
		
			private function reset(): void {
			player_text.text = "0";
			banker_text.text = "0";
			player_score = 0;
			banker_score = 0;

			//btn_bet_text_.text = "提交";
			total_bets = 0;
			temp_bal  = total_balance;
				
			total_balance_text.text = total_balance.toString();
				
				win_status = false;
			for (var mm: Number = 0; mm < cards.length; mm++) {
				cont_score.removeChild(cards[mm]);
				cards[mm] = null;
			}
			total_balance = temp_bal;
			for (var i: Number = 0; i < gameSettings.bet_tag.length; i++) {
				temp_win[i] = 0;
				var m: Number;
				
				for (m = 0; m < bets_[i]; m++) {
					this.removeChild(o.bet_[i][m]);
					o.bet_[i][m] = null;
				}
				for (m = 0; m < bets_Area[i]; m++) {
					this.removeChild(p.bet_[i][m]);
					p.bet_[i][m] = null;
				}
				
				//if (i == 4 && cont_chip_.numChildren != 0)
				this.removeChild(bet_chip_text[i]);
				bet_chip_text[i] = null;
				this.removeChild(bet_chip_add_text[i]);
				bet_chip_add_text[i] = null;
					
					bet_text[i].text = 0;
				temp_add_bets[i] = 0;
				bets_Area[i] = 0;
				bets_[i] = 0;
				bets[i] = 0;
				temp_live[i] = 0;
				temp_tbl[i] = 0;
					temp_bets[i] = 0;
			TweenLite.killTweensOf(bet_result[i]);
				bet_result[i].alpha = 0;
			}
			while (this.cont_chip_.numChildren > 0) {
				this.cont_chip_.removeChildAt(0);
			}
	
		}

		private function win_event(win:Array):void{
			for (var i: Number = 0; i < bet_name.length; i++) {
					//trace (connect_.win_details.win, bet_name[i]);
							for (var l: Number = 0; l <win.length; l++) {
						if(win[l] == bet_name[i]){
							temp_win[i] = 1;
						}
					}
				}
			
			}
		
			private function win_get(ic:Number):void{
					for (var i:Number= 0; i < bet_name.length; i++) {
						if(temp_win[i] == 0){
							for (var m:Number = 0; m < bets_Area[i]; m++) {
							if(ic == 1){
								TweenLite.to(p.bet_[i][m],.5,{x:500,y:200,alpha:0,delay: Math.random() * .6});
							}
							else{
								p.bet_[i][m].x = 500;
									p.bet_[i][m].y = 200;
									p.bet_[i][m].alpha = 0;
							}
						}
					}
						
					}
			}
			
			private function win_anim():void{
					
						var p:Array = new Array(); for ( var i:Number= 0; i < bet_name.length; i++) {
							if(temp_win[i]==1){win_status = true;
								chip_bet_check(i, Math.ceil(connect_.bet_details.bet[i] * bet_value[i]));
								chip_deck_check(i,Math.ceil(bet_value[i] *bets[i]));
								TweenMax.to(bet_result[i],.5,{alpha:1,repeat:-1,yoyo:true});
								TweenLite.to(result,.3,{alpha:1});
								p.push(bet_name_trans[i]);
								TweenLite.to(result_text,.3,{alpha:1});
								bet_text[i].text =((bet_value[i] *bets[i]) + bets[i]).toString();
								
								}
								if(temp_win[i]==0){
								bet_text[i].text = 0;
								}
							
					}
					result_text.text = p.toString();
			}
			private function win_release(ic:Number):void{
				for ( var i:Number= 0; i < bet_name.length; i++) {
							if(temp_win[i]==1){
							for (var m:Number = 0; m < bets_Area[i]; m++) {
								if(ic ==3){
								TweenLite.to(p.bet_[i][m],.8,{x:20,y:420,alpha:0,delay: Math.random() * .6});
									}
								else{
									p.bet_[i][m].x = 20;
									p.bet_[i][m].y = 420 ;
									p.bet_[i][m].alpha =0 ;
								}
					}
		if(bets_[i] !=0){
								for ( m = 0; m < bets_[i]; m++) {
										TweenLite.to(o.bet_[i][m],.5,{x:total_balance_text.x ,y:total_balance_text.y,alpha:0,scaleX:.2,scaleY:.2});
										TweenLite.to(bet_chip_text[i],.5,{x:total_balance_text.x ,y:total_balance_text.y,alpha:0,scaleX:.2,scaleY:.2});
										TweenLite.to(bet_chip_add_text[i],.5,{x:total_balance_text.x ,y:total_balance_text.y,alpha:0,scaleX:.2,scaleY:.2});
									}
								}
					
					}
					
					}
			}
			private function win_end():void{
				for (var i:Number= 0; i < bet_name.length; i++) {
						for (var m:Number = bets_[i] -1; m >=0; m--) {
					TweenLite.to(o.bet_[i][m],.5,{alpha:0});
					}
					if(bets_[i] !=0)
					TweenLite.to(bet_chip_text[i],.5,{alpha:0});
				
					}
					TweenLite.to(result,.5,{alpha:0});
					TweenLite.to(result_text,.5,{alpha:0});
					result_text.text = "";
					TweenLite.to(cont_score,.5,{alpha:0,onComplete:reset});
					con = false;
			}
			
			
		//------
	}
}
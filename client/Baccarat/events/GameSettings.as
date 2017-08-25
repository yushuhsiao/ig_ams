package events{

	
	public class GameSettings  {
			
		//Chip Settings
		public var chip_selected: Number = 10;
//public var tot_bal: Number = 2000;
		public var chip_blank: Array = new Array("coin_1_ instance 1","coin_5_ instance 1","coin_10_ instance 1","coin_50_ instance 1","coin_100_ instance 1","coin_250_ instance 1","coin_500_ instance 1","coin_1000_ instance 1","coin_2500_ instance 1","coin_5000_ instance 1","coin_10000_ instance 1","coin_20000_ instance 1","coin_25000_ instance 1","coin_50000_ instance 1","coin_100000_ instance 1");
		public var chip_name: Array = new Array("chip_1 instance 10000", "chip_5 instance 10000", "chip_10 instance 10000", "chip_50 instance 10000", "chip_100 instance 10000", "chip_250 instance 10000", "chip_500 instance 10000", "chip_1000 instance 10000", "chip_2500 instance 10000", "chip_5000 instance 10000", "chip_10000 instance 10000", "chip_20000 instance 10000", "chip_25000 instance 10000", "chip_50000 instance 10000", "chip_100000 instance 10000")
		public var chip_value: Array = new Array(1, 5, 10, 50, 100, 250, 500, 1000, 2500, 5000, 10000, 20000, 25000, 50000, 100000);
		public var chip_scale: Number = .7;
		public var chip_scale_anim: Number = .8;
		
		//Bet Area
		public var bet_tag: Array = new Array("bet_either_pair instance 10000","bet_big instance 10000","bet_player_pair instance 10000","bet_player instance 10000","bet_tie instance 10000","bet_super6 instance 10000","bet_banker instance 10000","bet_banker_pair instance 10000","bet_small instance 10000","bet_perfect_pair instance 10000");
		public var bet_scale: Number = .7;
		public var bet_space: Number = 5;

		public var bet_hover_tag: Array = new Array("bet_hover_1 instance 1","bet_hover_2 instance 1","bet_hover_3 instance 1","bet_hover_4 instance 1","bet_hover_6 instance 1","bet_hover_5 instance 1","bet_hover_7 instance 1","bet_hover_8 instance 1","bet_hover_9 instance 1","bet_hover_10 instance 1");
	
		public var bet_result_tag: Array = new Array("bet_result_1 instance 1","bet_result_2 instance 1","bet_result_3 instance 1","bet_result_4 instance 1","bet_result_6 instance 1","bet_result_5 instance 1","bet_result_7 instance 1","bet_result_8 instance 1","bet_result_9 instance 1","bet_result_10 instance 1");
		
		public var c_cards_name: Array = new Array("c1 instance 10000", "c2 instance 10000", "c3 instance 10000", "c4 instance 10000","c5 instance 10000", "c6 instance 10000", "c7 instance 10000", "c8 instance 10000", "c9 instance 10000", "c10 instance 10000", "c11 instance 10000", "c12 instance 10000", "c13 instance 10000");
		public var d_cards_name: Array = new Array("d1 instance 10000", "d2 instance 10000", "d3 instance 10000", "d4 instance 10000","d5 instance 10000", "d6 instance 10000", "d7 instance 10000", "d8 instance 10000", "d9 instance 10000", "d10 instance 10000", "d11 instance 10000", "d12 instance 10000", "d13 instance 10000");
		
		public var s_cards_name: Array = new Array("s1 instance 10000", "s2 instance 10000", "s3 instance 10000", "s4 instance 10000","s5 instance 10000", "s6 instance 10000", "s7 instance 10000", "s8 instance 10000", "s9 instance 10000", "s10 instance 10000", "s11 instance 10000", "s12 instance 10000", "s13 instance 10000");
		
		public var h_cards_name: Array = new Array("h1 instance 10000", "h2 instance 10000", "h3 instance 10000", "h4 instance 10000","h5 instance 10000", "h6 instance 10000", "h7 instance 10000", "h8 instance 10000", "h9 instance 10000", "h10 instance 10000", "h11 instance 10000", "h12 instance 10000", "h13 instance 10000");
		
		public var cards_scale: Number = .55;
		public var cards_value: Array = new Array(1, 2, 3, 4, 5, 6, 7, 8, 9,0);
		
	
		public function GameSettings() {
			// constructor code
			
		}

	}
	
}

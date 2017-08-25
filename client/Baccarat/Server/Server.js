#!/usr/bin/env node
var WebSocketServer = require('websocket').server;
var http = require('http');
//var io = require('socket.io');

var clients = [];
 var temp =  {};
var t = false;
var draw_ = false;
var drew_ = false;
var countdown = 10;  
var cards = ["diamond","heart","club","spade"];
var Player_score = 0;
var Banker_score = 0;
var draws_ = 0;
var p1 = 0 ;
var p2 = 0 ;
var p3 = 0 ;
var b1 = 0 ;
var b2 = 0 ;
var b3 = 0 ;
var tot1 = 0;
var tot2 = 0;
var get_ = true;
var tim = false;
var interval = 0;

var game_num = 1;	
var game_ = "Game " + game_num;	
var temp_amount = {};
var id;

var card_face=[];
var card_value=[];

var cards_drew={};
var won={};
var won_ = false;

 
var server = http.createServer(function(request, response) {
    console.log((new Date()) + ' Received request for ' + request.url);
    response.writeHead(404);
    response.end();

});
server.listen(8080, function() {
    console.log((new Date()) + ' Server is listening on port 8080');
});
 

wsServer = new WebSocketServer({
    httpServer: server,
    // You should not use autoAcceptConnections for production 
    // applications, as it defeats all standard cross-origin protection 
    // facilities built into the protocol and the browser.  You should 
    // *always* verify the connection's origin and decide whether or not 
    // to accept it. 
    autoAcceptConnections: false
});
 
function originIsAllowed(origin) {
  // put logic here to detect whether the specified origin is allowed. 
  return true;
}
   var count = 0;
var clients = {};
var clients_data = {};
var clients_data_bet = {};
var total_bets = {};
wsServer.on('request', function(request) {
  

    if (!originIsAllowed(request.origin)) {
      // Make sure we only accept requests from an allowed origin 
      request.reject();
      console.log((new Date()) + ' Connection from origin ' + request.origin + ' rejected.');
      return;
    }
    
    var connection = request.accept('echo-protocol', request.origin);

	

// Specific id for this client & increment count
id = count++;
// Store the connection method so we can loop through & contact all clients
clients[id] = connection;
clients_data[id] = {function_:"Account",ID:id,amount: 2000};
clients_data_bet[id] = {function_:"Account_bet",ID:id,total_bet:0,bet:{ 0:0,1:0,2:0,3:0,4:0,5:0,6:0,7:0,8:0,9:0}};
temp_amount[id] = {amount:0};
 console.log((new Date()) + ' Connection accepted [' + id + ']');
  
    connection.on('message', function(message) {
        if (message.type === 'utf8') {
 var p =  {};
p = JSON.parse(message.utf8Data);
if(p.Server == "Server_1"){
	clients[id].sendUTF(JSON.stringify(clients_data[id]));	
	if(t){
clients[id].sendUTF(JSON.stringify(temp));
}else{
   	clients[id].sendUTF(JSON.stringify({function_:"Bet",Game:game_,bet:{ 0:0,1:0,2:0,3:0,4:0,5:0,6:0,7:0,8:0,9:0}}));
}
clients[id].sendUTF(JSON.stringify({function_:"Countdown",countdown:countdown,status:draw_}));
	if(drew_ && draws_ !=0){
		clients[id].sendUTF(JSON.stringify({function_:"Drew",draw:draws_,cards_drew}));
		drew_ = false;
	}
		if(won_){
			clients[id].sendUTF(JSON.stringify(won));
			won_ = false;
		}
}
else{
t = true;
temp = JSON.parse(message.utf8Data);
if(temp.function_ == "Bet"){
clients_data_bet[temp.ID] = JSON.parse(message.utf8Data);
temp_amount[temp.ID].amount += clients_data_bet[temp.ID].total_bet;
console.log("Betting");
}
//console.log(clients_data[id]);
 for(var i in clients){
            	console.log('Received Message: ' + message.utf8Data);
 clients[i].sendUTF(message.utf8Data);
 	 
		}}
        }


        else if (message.type === 'binary') {
 for(var i in clients){
            console.log('Received Binary Message of ' + message.binaryData.length + ' bytes');
           clients[i].sendBytes(message.binaryData);
        }}
    });
	
	

    connection.on('close', function(reasonCode, description) {
        console.log((new Date()) + ' Peer ' + connection.remoteAddress + ' disconnected.');
    });
});
	 
	setInterval(function() {  

  if(countdown == 0){
	  draw_ = true;
	   countdown = 10;
	   for(var i in clients){
    	clients[i].sendUTF(JSON.stringify({function_:"Countdown",countdown:countdown,status:draw_}));
   }
	}
	
	if(draw_ && get_){
		game();
		draws_++;
			drew_ = true;
	}

	 if(tim){
		 for(var i in clients){
    	clients[i].sendUTF(JSON.stringify({function_:"Interval",count:interval}));
   }
		 interval++;
		 if(interval == 8){
				draw_ = false;
				get_= true;
				 cards_drew = {};
				 won_ = false;
				 won = {};
				  draws_ = 0;
				  interval = 0;
				game_ = "Game " + game_num;
temp = {function_:"Bet",Game:game_,bet:{ 0:0,1:0,2:0,3:0,4:0,5:0,6:0,7:0,8:0,9:0}};
 for(var i in clients){
    	clients[i].sendUTF(JSON.stringify({function_:"Bet",Game:game_,bet:{ 0:0,1:0,2:0,3:0,4:0,5:0,6:0,7:0,8:0,9:0}}));
   }
		 }
	 }
	
	if(!draw_ && interval == 0){
 countdown--;
	  tim = false;
   for(var i in clients){
    	clients[i].sendUTF(JSON.stringify({function_:"Countdown",countdown:countdown,status:draw_}));
   }
	}
	}, 1000);

function game(){
	var drawing; 
	var a ; 
	var b ;
	var c ;
	var f = false;
var g = false;
	a= cards[randomIntFromInterval(0,3)];
	b =randomIntFromInterval(1,13);
	c = b;
	if(b >=10 && b<=13){
		c = 0;
	}
	if(draws_ == 0){
		drawing = "Player";
		p1 = c;
		tot1 = p1;
		card_face.push(a);
	card_value.push(b);
	}
	else if(draws_ == 1){
		drawing = "Banker";
			b1 = c;
			tot2 = b1;
			card_face.push(a);
	card_value.push(b);
	}
	else if(draws_ == 2){
		drawing = "Player";
			p2 = c;
			tot1 += p2;
			card_face.push(a);
	card_value.push(b);
	}
	else if(draws_ == 3){
		drawing = "Banker";
			b2 = c;
				tot2 += b2;
				g = true;
	card_face.push(a);
	card_value.push(b);
}
else{}
	
if(g){
	if(tot1 >9){
		tot1 -= 10;
	}
	if(tot2 >9){
		tot2 -= 10;
	}
	if(tot1 == 9 && tot2 == 9  || tot1 == 8 && tot1 == 8 ){
	
		f = true;
	}
	else if(tot1 == 9  || tot1 == 8 ){
	
		f = true;
	}
	else if (tot2 == 9 || tot2 == 8 ){
		
		f = true;
	}
	
	else{
	if(tot1 >=6 && tot1<=7 && tot2 >=6 && tot2<=7 ){
		f = true;
	}
}
}
	
	if(draws_ == 4){
		if(tot1 >=0 && tot1 <= 5 && tot2 != 8 && tot2 != 9){
		drawing = "Player";
		p3 = c;
		tot1 += p3;
		}
		if(tot1 == 6 || tot1 == 7){
			if( tot2 >=0 && tot2 <= 5){
		drawing = "Banker";
		b3 = c;
		tot2 += b3;
		f = true;
			}
	}
 if(tot2 == 6 || tot2 == 7){
		f = true;
	}
	
		
	
	}
	if(draws_ == 5 ){
		drawing = "Banker";
		if(p3 == 9  && tot2 >=0 && tot2 <=3 || p3 == 1 && tot2 >=0 && tot2 <=3|| p3 == 0 && tot2 >=0 && tot2 <=3){
				b3 = c;
				tot2 += b3;
		}
		else if(p3 == 8 && tot2 >= 0 && tot2 <= 2){
				b3 = c;
				tot2 += b3;
		}
		else if (p3 == 6 && tot2 >=0 && tot2 <=6 || p3 == 7 && tot2 >=0 && tot2 <=6){
				b3 = c;
				tot2 += b3;
		}
		else if(p3 == 4 && tot2 >=0 && tot2 <=5 || p3 == 5 && tot2 >=0 && tot2 <=5){
				b3 = c;
				tot2 += b3;
		}
		else if (p3 == 2 && tot2 >=0 && tot2 <=4 || p3 == 3 && tot2 >=0 && tot2 <=4){
				b3 = c;
				tot2 += b3;
		}
		else{
			f = true;
		}
		f = true;
	}

	
	
	for(var i in clients){
    	clients[i].sendUTF(JSON.stringify({function_:"Draw",draws:drawing,card:a,card_value:b,draw: draws_}));
   }
   cards_drew[draws_] = {function_:"Drew",draws:drawing,card:a,card_value:b,draw: draws_}; 
	
	if(f ||draws_ == 5){
		if(tot1 >9){
		tot1 -= 10;
	}
	if(tot2 >9){
		tot2 -= 10;
	}
		calc(tot1,tot2);
		f = false;
	}
}
function calc(o,p){
	var win =[];
	console.log(o,p);
	if(o == 9 && p == 9 || o == 8 && p == 8 ){
		win.push("Tie");
	}
	else if(o == 9 || o == 8){
		win.push("Player");
	}
	else if(p == 9 || p == 8){
		win.push("Banker");
	}
	else{
		if( o == p){
				win.push("Tie");
		}
		else if(o>p){
			win.push("Player");
		}
		else if (o<p){
				win.push("Banker");
		}
		
	else{
	}
	}
	if( card_value[0] == card_value[2]  ||  card_value[1] == card_value[3]){
		win.push("Either-Pair");
	}
	if( card_value[0] == card_value[2]){
		win.push("Player-Pair");
	}
	if( card_value[1] == card_value[3]){
		win.push("Banker-Pair");
	}
	if(card_face[0] == card_face[2] && card_value[0] == card_value[2] || card_face[1] == card_face[3] && card_value[1] == card_value[3]){
		win.push("Perfect-Pair");
	}
	if(((tot1 + tot2)%10)==4){
		win.push("Small");
	}
	if(((tot1 + tot2)%10)==5 ||((tot1 + tot2)%10)==6){
		win.push("Big");
	}
	if(tot2 == 6 && tot1 <6){
		win.push("Super 6");
	}
	console.log(win);
	bets(win);
	 
	
	for(var i in clients){
    	clients[i].sendUTF(JSON.stringify({function_:"Win",win:win}));
   }
   won = {function_:"Won",win:win};
   won_ = true;
   clear();
  
}
function randomIntFromInterval(min,max)
{
    return Math.floor(Math.random()*(max-min+1)+min);
}
function bets(win){
	var i=["Either-Pair","Big","Player-Pair","Player","Tie","Super 6","Banker","Banker-Pair","Small","Perfect-Pair"];
	var u=[5,0.53,11,1,8,12,1,11,1.5,1.25];


		for(o = 0 ; o <= id;o++){
			if(temp_amount[o].amount !=0){
			clients_data[o].amount -= temp_amount[o].amount;
			temp_amount[o].amount = 0;
		for(p = 0 ; p < i.length; p++){
			
			for(l = 0 ; l <win.length;l++){
			
			if(win[l] == i[p]){
			temp_amount[o].amount +=  clients_data_bet[o].bet[p] + (clients_data_bet[o].bet[p] * u[p]);
			}
			}
		}
		clients[o].sendUTF(JSON.stringify({function_:"Account",ID:o,amount:clients_data[o].amount+temp_amount[o].amount}));
		clients_data[o].amount = clients_data[o].amount+temp_amount[o].amount;
			temp_amount[o].amount = 0;
			}
		}
	
	
	
}
function clear(){
	
		card_face = [];
	card_value = [];
	
		get_ = false;
		p1 = 0;
		p2 = 0;
		p3 = 0;
		b1 = 0;
		b2 = 0;
		b3 = 0;
		tot1 = 0;
		tot2 = 0;
		Player_score = 0;
Banker_score = 0;
game_num++;
win = [];
tim = true;
interval = 0;

	
  	
}
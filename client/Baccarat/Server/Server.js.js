#!/usr/bin/env node
var WebSocketServer = require('websocket').server;
var http = require('http');
var io = require('socket.io');

var clients = [];
 var temp =  {};
var t = false;

var server = http.createServer(function(request, response) {
    console.log((new Date()) + ' Received request for ' + request.url);
    response.writeHead(404);
    response.end();

});
server.listen(8080, function() {
    console.log((new Date()) + ' Server is listening on port 8080');
});
 
wsServer = io.listen(server);
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
wsServer.on('request', function(request) {
  

    if (!originIsAllowed(request.origin)) {
      // Make sure we only accept requests from an allowed origin 
      request.reject();
      console.log((new Date()) + ' Connection from origin ' + request.origin + ' rejected.');
      return;
    }
    

    var connection = request.accept('echo-protocol', request.origin);

// Specific id for this client & increment count
var id = count++;
// Store the connection method so we can loop through & contact all clients
clients[id] = connection;

 console.log((new Date()) + ' Connection accepted [' + id + ']');


    connection.on('message', function(message) {
			
        if (message.type === 'utf8') {
 var p =  {};


p = JSON.parse(message.utf8Data);

if(p.Server == "Server_1"){

		if(t){
clients[id].sendUTF(JSON.stringify(temp));
}else{
            	clients[id].sendUTF(JSON.stringify({amount: 2000,bet:{ 0:0,1:0,2:0,3:0,4:0,5:0,6:0,7:0,8:0,9:0}}));
}
}
else{
t = true;
temp = JSON.parse(message.utf8Data);
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

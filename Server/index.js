var io = require('socket.io')(process.env.io || 8080);
var Player = require('./src/Player.js');
var Bullet = require('./src/ServerObjects.js');

console.log('El server arranca');

var players = [];
var sockets = [];
var bullets = [];

Update(() =>{
     bullets.forEach(element =>{
        bullet.onUpdate();
        for(var otherplayer in players)
         if(otherplayer != player.id){
            socket.emit('updatePosition',socket[player.id])
         }
     });
}, 100,0);

io.on('connection',function(socket){    
    console.log('Se conectó un player');

    var player = new Player();
    players[player.id]= player;
    sockets[player.id]= socket;

    socket.emit('onRegister',{id: player.id});   
    socket.emit('spawn', player);
    socket.broadcast.emit('spawn',player)
    for(var otherPlayer in players){
        if(otherPlayer != player.id){            
            socket.emit('spawn',players[otherPlayer]);
            console.log(otherPlayer);
            console.log(player.id);
        }
    } 

    socket.on('updatePosition',function(data){
        player.position.x = data.position.x;
        player.position.y = data.position.y;
        player.position.z = data.position.z; 
        socket.broadcast.emit('updatePosition',player);
    })

    socket.on('updateRotation',function(data){
        player.rotation.x = data.rotation.x;
        player.rotation.y = data.rotation.y;
        player.rotation.z = data.rotation.z;
    })

    socket.on('disconnect',function(){
        console.log('player se desconecto');  
        delete(players[player.id]);
        delete(sockets[player.id]);
        socket.broadcast.emit('playerDisconnected',player)              
    });

    socket.on('fireBullet',function(){

    });    
});

function Update(func,wait,loops){
    var interv = function(w,l){
        return function(){
            if(typeof t === "undefined" || t-->0)
            {
                setTimeout(interv,w);
                try{
                    func.call(null);
                }
                catch(e){
                    t = 0; throw e.toString();
                }
            }
        };
    }(wait,loops);
    setTimeout(interv,wait);
}
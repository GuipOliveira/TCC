/*CLASSE RESPONSÁVEL PELO GERENCIAMENTO DA APLICAÇÃO NO SERVIDOR E COMUNICAÇÃO ENTRE OS DOIS CLIENTES*/
var express  = require('express');
var app      = express();
var http     = require('http').Server(app);
var io       = require('socket.io')(http);

var shortId 		= require('shortid');
app.use(express.static(__dirname ));
var clients			= [];
var radio			= 0;

/*objetos de conexão com o Mongo*/
var assert = require('assert');
var MongoClient = require('mongodb').MongoClient;
var url = "mongodb://tcc360:tcc360@cluster0-shard-00-00-62qzf.mongodb.net:27017,cluster0-shard-00-01-62qzf.mongodb.net:27017,cluster0-shard-00-02-62qzf.mongodb.net:27017/tcc360?ssl=true&replicaSet=Cluster0-shard-0&authSource=admin";
var ObjectId = require('mongodb').ObjectID;
/*objetos de conexão com o Mongo*/

/*insere usuarios na coleção de ranking*/
function inserirRanking(idsessao,user1,user2,sala)
{

	MongoClient.connect(url, function(err, db) {
	assert.equal(null, err);
		db.collection('ranking360').insertOne({
			"idsessao": idsessao,
			"usuario1" : user1,
			 "usuario2" : user2,
			 "sala": sala,
			 "inicio": Date.now(),
			 "fim":"-1",
			 "duracao":"-1"
		
		}, function(err, result) {
		if(err) throw err;
		
		assert.equal(err, null);
		console.log("Inserindo no bd");

		});

		db.close();
});
}

/*retorna ultimo id inserido*/
function ultimoInserido()
{
MongoClient.connect(url, function(err, db) {
  if (err) throw err;
  db.collection("ranking360").find({}).sort({"_id":-1}).limit(1).toArray(function(err, result) {
    if (err) throw err;
    console.log(result[0]._id.toString());

    db.close();
  });
});

}

/*retorna dados do BD do ultimo registro inserido ao primeiro*/
function lerBD()
{
MongoClient.connect(url, function(err, db) {
  if (err) throw err;
  db.collection("ranking360").find({}).sort({"_id":-1}).toArray(function(err, result) {
    if (err) throw err;
    console.log(result);

    db.close();
  });
});

}




io.on('connection', function(socket){
    
	var currentUser;
	
	console.log('A user ready for connection!');
	
	/*Método que loga o usuário no servidor*/
	socket.on('LOGIN', function(player){

	   var primeiroPlayer;
	   console.log('[INFO] Player ' + player.name + ' conectando!');
	   currentUser = {
			name:player.name,
			id:shortId.generate(),
			sessao:player.sessao,
			players:0,
			flgEnvio:0,
			idObj:0,
			porta:-1,
			vezMatriz:'V', //seta a vez do jogador jogar no puzzle de acender Matriz
			posicaoSelecionada: '', //posição selecionada na Matriz
			IdBd:'-1'
		}
		
		for(i = 0; i < clients.length; i++){
		
			if(clients[i].sessao == currentUser.sessao) //loop para verificar se existe uma sessão já criada
			{	
				
				if(currentUser.players != 2)
				{
				console.log('[INFO] Segundo player entrando na sessão: ' + currentUser.sessao);
				currentUser.players = 2;//segunda a entrar na sala
				currentUser.vezMatriz = 'F';
				primeiroPlayer = clients[i].name;
				}
				else{
				console.log('[INFO] Já existem dois players na sessão: ' + currentUser.sessao);
				currentUser.players = -1;
				}
			}
			
			
		}
		if(currentUser.players ==0) 
		{
		console.log('[INFO] Primeiro Player a entrar na sessão: ' + currentUser.sessao);
		currentUser.players = 1; //primeiro a entrar nesta sessão
		currentUser.vezMatriz = 'V';
		}
		
		if(currentUser.players != -1)
		{
			
			try
			{
				if(currentUser.players == 2)
				{
					inserirRanking(currentUser.id,primeiroPlayer,currentUser.name,currentUser.sessao); //insere
			
				}

			}catch(e)
			{
				console.log(e.toString());
			}
			clients.push(currentUser);

			console.log('Total players: ' + currentUser.players);
			console.log('Inserido player: ' + currentUser.id);
			console.log('Ultimo Inserido: \n' + currentUser.IdBd);
			socket.emit('LOGIN_SUCESS',currentUser);//usuário logado
		}
		else
		{
			console.log('Total players: ' + currentUser.players);
			socket.emit('LOGIN_INSUCESS');	//não foi possível logar, pois já existem dois jogadores na sessão	
		}
		
	});
	/*Método responsável pelo processamento da transferência de objetos entre salas*/
	socket.on('TRANSFERIR',function(player){
	console.log('Procurando');
		for(i = 0; i < clients.length; i++){


			if(clients[i].id == player.id) 
			{
				console.log('[INFO] Player ' + player.id + ' enviando objeto!');
				clients[i].flgEnvio = 1; //sinaliza que o usuário esta enviando um objeto
				clients[i].idObj = player.idObj;
				i = clients.length;
			}
				
		}
	});
	
	/*Método responsável por verificar se existe algum sinal porta enviado*/
	socket.on('AGUARDAR', function(player){

		for(i = 0; i < clients.length; i++){
			if(clients[i].sessao == player.sessao && clients[i].id != player.id) //verifica o outro usuário conectado a mesma sessão
			{
				if(clients[i].flgEnvio == 1)//se a flag estiver ativa, significa que o outro usuário enviou um sinal da porta 
				{
					console.log('[INFO] Player ' + player.id + ' recebendo sinal porta!');
					clients[i].flgEnvio = 0;
					
					socket.emit('RECEBE_SINALPORTA',clients[i]);
			
				}
			}
			else if(clients[i].sessao == player.sessao && clients[i].id == player.id) //verifica o usuário conectado 
			{
				socket.emit('ATUALIZA_VEZMATRIZ',clients[i]); //atualiza para os clientes, de quem é a vez de jogar no puzzle Matriz e envia a ultima posição de botão selecionada
			}
		}
					
	});
	

	
	
		/*Método responsável por troca de sinais para abertura de portas*/
	socket.on('SINALPORTA', function(player){
		for(i = 0; i < clients.length; i++){
			if(clients[i].sessao == player.sessao && clients[i].id == player.id) //verifica usuário conectado a sessão
			{
					clients[i].porta = player.porta;

					console.log('[INFO] Player ' + player.id + ' enviando sinal Porta!');
					clients[i].flgEnvio = 1; //sinaliza que o usuário esta enviando um objeto
					clients[i].porta = player.porta;
					i = clients.length;
				
			}
		}
	
	});
	
/*Método responsável por trocar a vez de jogada na Matriz*/
	socket.on('ALTERAR_VEZMATRIZ', function(player){
		 console.log('Botão apertado:' + player.posicao)
		for(i = 0; i < clients.length; i++){
			if(clients[i].sessao == player.sessao && clients[i].id == player.id) //verifica usuário conectado a sessão
			{
					clients[i].vezMatriz = 'F';
					clients[i].posicaoSelecionada = '';
					console.log('[INFO] Player ' + player.id + ' já jogou na Matriz!');

					socket.emit('ATUALIZA_VEZMATRIZ',clients[i]);
			}
			else if(clients[i].sessao == player.sessao && clients[i].id != player.id) //verifica o outro usuário conectado a sessão
			{
					clients[i].vezMatriz = 'V';
					clients[i].posicaoSelecionada = player.posicao;
					console.log('[INFO] Player ' + player.id + ' é o proximo a jogar na Matriz!');
			}
		}
	
	});
	
		/*Retorna o ranking com os melhores jogadores*/
	socket.on('GET_RANKING',function(data){
	console.log('Atualizando Ranking!');
	MongoClient.connect(url, function(err, db) {
	if (err) throw err;
	db.collection("ranking360").find({}).sort({"duracao":-1}).toArray(function(err, result) {
    if (err) throw err;
    
	console.log(result);
    db.close();
	
	currentRetorno ={
		ranking:''
		
	}
	for(i = 0; i < result.length;i++)
		currentRetorno.ranking += "Tempo: " + result[1].duracao + " Jogadores: " + result[i].usuario1 + " e " + result[i].usuario2 + "#";
	
	console.log(currentRetorno.ranking);
	
	socket.emit('ATUALIZA_RANKING',currentRetorno);
	  });
	});
	});


	
	
});



http.listen(process.env.PORT ||3000, function(){
	console.log('listening on *:3000');
});

console.log("------- server is running -------");
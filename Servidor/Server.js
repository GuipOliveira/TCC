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
function inserirRanking(_idsessao,user1,user2,sala)
{
	var qtd;
		/*Count para saber se o registro ja foi inserido*/
	MongoClient.connect(url, function(err, db) {
    var query = { idsessao:_idsessao};
		if (err) throw err;
			db.collection("ranking360").find(query).toArray(function(err, result)  {
		if (err) throw err;
			console.log("Contando sessões");
			qtd = result.length;
			console.log("Quantidade de registros inserido com esta sessão: " + qtd);
			
			if(qtd == 0)
			{
			MongoClient.connect(url, function(err, db) {
			assert.equal(null, err);
				db.collection('ranking360').insertOne({
					"idsessao": _idsessao,
					"usuario1" : user1,
					"usuario2" : user2,
					"sala": sala,
					"inicio": Date.now(),
					"fim":"-1",
					"duracao":"999999999"
				
				}, function(err, result) {
				if(err) throw err;
				
				assert.equal(err, null);
				console.log("Inserindo no bd");
			
				});
			
				db.close();
				
			});
			}
		});
	});	
	
	
}

function atualizarRanking(idBd,sessao,user1,user2,_sala,_inicio,termino,tempo)
{
	try{
	console.log('Atualizando Ranking! ' + idBd);
	
	MongoClient.connect(url, function(err, db) {
	if (err) throw err;
	var query = { _id: idBd };
	var valores = { idsessao:sessao, usuario1:user1,usuario2:user2,sala:_sala,inicio:_inicio, fim: termino, duracao: tempo };
	
	
	db.collection("ranking360").update(query, valores, function(err, res) {
		if (err) throw err;
		console.log("Atualizado");
		db.close();
		});
	});

	}catch(e)
	{
	
		console.log(e.toString());
	}

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
			ponteiroRelogio:'F',
			saiu:'F',
			portasAbertas:'-1',
			idSessao:shortId.generate()

		}
		var aux = 0;

		for(i = 0; i < clients.length; i++){
		
			if(clients[i].sessao == currentUser.sessao) //loop para verificar se existe uma sessão já criada
			{	
				
				if(currentUser.players != 2)
				{
				console.log('[INFO] Segundo player entrando na sessão: ' + currentUser.sessao);
				currentUser.players = 2;//segunda a entrar na sala
				currentUser.vezMatriz = 'F';
				primeiroPlayer = clients[i].name;
				aux = clients[i].players;
				currentUser.idSessao = clients[i].idSessao;
				currentUser.portasAbertas = clients[i].portasAbertas;
				}
				else{
				console.log('[INFO] Já existem dois players na sessão: ' + currentUser.sessao);
				currentUser.players = -1;
				aux = 0;
				}
			}
			
			
		}
		if(currentUser.players ==0 || aux == 2) //variavel auxiliar verifica se o primeiro player se desconectou durante o jogo e voltou novamente
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
					inserirRanking(currentUser.idSessao,primeiroPlayer,currentUser.name,currentUser.sessao); //insere
			
				}

			}catch(e)
			{
				console.log(e.toString());
			}
			clients.push(currentUser);

			console.log('Total players: ' + currentUser.players);
			console.log('Portas Abertas: ' + currentUser.portasAbertas);
			console.log('Player: ' + currentUser.id + ' Conectado a sessão: ' + currentUser.sessao + ' Id Sessão: ' + currentUser.idSessao);
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
	
	/*ATUALIZA NO SERVIDOR SE OS PONTEIROS ESTÃO APONTADOS CORRETOS, SE SIM AVISA AOS CLIENTES*/
	socket.on('ATUALIZAR_PONTEIRO', function(player){
		var pont1 = '';
		var pont2 = '';
		console.log('Atualizando Ponteiro Relogio');
		for(i = 0; i < clients.length; i++){
			
			if(clients[i].id == player.id)
			{
				clients[i].ponteiroRelogio = player.ponteiroRelogio;
				pont1 = clients[i].ponteiroRelogio;
			}
			else if(clients[i].sessao == player.sessao && clients[i].id != player.id)
			{
				pont2 = clients[i].ponteiroRelogio;
			}
		
		}
		
		if(pont1 == 'V' && pont2 == 'V')
		{
			socket.emit('PONTEIRO_OK',clients[0]);
			console.log('Player: ' + player.id+' Relógio ajustado!');
		}
		else
		{
		
		console.log('Player: ' + player.id+' Relógio não ajustado!');
		}
		console.log('Player: ' + player.ponteiroRelogio);
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
		
		console.log('Porta:' + player.porta)
		for(i = 0; i < clients.length; i++){
			if(clients[i].sessao == player.sessao && clients[i].id == player.id) //verifica usuário conectado a sessão
			{
					clients[i].porta = player.porta;
				
					
					clients[i].flgEnvio = 1; //sinaliza que o usuário esta enviando um objeto
					clients[i].portasAbertas = player.porta.toString();

				
			}else if(clients[i].sessao == player.sessao && clients[i].id != player.id)
			{
				clients[i].portasAbertas = player.porta.toString();
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

	/*Método responsável por finalizar o jogo e atualizar o BD*/
	socket.on('ULTIMA_PORTA',function(player){
		console.log('Game finalizado para o jogador: ' + player.id);
		var terminou = false;
		var _idSessao = '';
		for(i = 0; i < clients.length;i++)
		{
			if(clients[i].sessao == player.sessao && clients[i].id == player.id)
			{
				clients[i].saiu = 'V';
				_idSessao = clients[i].idSessao;
			}
			else if(clients[i].sessao == player.sessao && clients[i].id != player.id)
			{
				 if(clients[i].saiu == 'V')
				 {
					terminou = true;
				 }
			}
		}
	if(terminou)
	{
	try{
		console.log('Game finalizado para os dois Players, na sessão: ' + _idSessao);
		var inicio = ''
		var idBd = '';
		var query = { idsessao: _idSessao,fim:'-1' };
		var fim = Date.now();
		
		/*Busca o Id a ser atualizado*/
			MongoClient.connect(url, function(err, db) {

				if (err) throw err;
					db.collection("ranking360").find(query).limit(1).toArray(function(err, result)  {
				if (err) throw err;
					idBd = result[0]._id;
					var sessao = result[0].idsessao;
					var user1 = result[0].usuario1;
					var user2 = result[0].usuario2;
					var sala = result[0].sala;
					var inicio = result[0].inicio; 
					db.close();
					var duracao = fim - inicio;
					console.log('ID: ' + idBd +'Inicio: ' + inicio);
					
					atualizarRanking(idBd,sessao,user1,user2,sala,inicio,fim,duracao);
					
	
				});
			});

	}catch(e)
		{ console.log(e.toString());}
	}
	});
		/*Retorna o ranking com os melhores jogadores*/
	socket.on('GET_RANKING',function(data){
	console.log('Atualizando Ranking!');
	MongoClient.connect(url, function(err, db) {
	if (err) throw err;
	db.collection("ranking360").find({}).sort({"duracao":1}).limit(5).toArray(function(err, result) {
    if (err) throw err;
    
	console.log(result);
    db.close();
	
	currentRetorno ={
		ranking:''
		
	}
	
	for(i = 0; i < result.length;i++)
	{
		var dateTempo = new Date(result[i].duracao);

		currentRetorno.ranking += "Tempo: "  + dateTempo.getMinutes() +":" + dateTempo.getSeconds() + " Jogadores: " + result[i].usuario1 + " e " + result[i].usuario2 + "#";
	}
	console.log(currentRetorno.ranking);
	
	socket.emit('ATUALIZA_RANKING',currentRetorno);
	  });
	});
	});
	
	/*MÉTODO QUE RETIRA O CLIENTE DO SERVER*/
	socket.on('SAIR',function(player){
	console.log('Removendo jogador da sessão: ' + player.sessao);
	try{
	var qtd = clients.length - 1;
	var aux = false;

	
	for(i = 0; i < clients.length;i++)
	{
		if(player.id == clients[i].id && player.sessao == clients[i].sessao)
		{
			console.log('Retirando jogador: ' + clients[i].id);
			clients[i].sessao = '-1';
		}
		
		if(i == qtd && clients[i].sessao == '-1')
		{
			aux = true;
		}
			
	}

	
	if(aux = true)
	{
		var inativos = true;
		for(j = qtd; j >= 0 && inativos; j--)
		{
			if(clients[j].sessao == '-1')
			{
				console.log('Pop: ' + clients[j].id);
				clients.pop();
			}
			else
			{
				console.log('Jogadores Ativos');
				inativos = false;
			}
		}
	}
	}
	catch(e)
	 {console.log(e.toString());}
	});
	
	
});



http.listen(process.env.PORT ||3000, function(){
	console.log('listening on *:3000');
});

console.log("------- server is running -------");
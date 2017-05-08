/*CLASSE RESPONSÁVEL PELO GERENCIAMENTO DA APLICAÇÃO NO SERVIDOR E COMUNICAÇÃO ENTRE OS DOIS CLIENTES*/
var io = require('socket.io')({
	transports: ['websocket'],
});
var shortId 		= require('shortid');
var clients			= [];
var radio			= 0;
io.attach(4567);


io.on('connection', function(socket){
    
	var currentUser;
  
    
	console.log('A user ready for connection!');
	
	/*Método que loga o usuário no servidor*/
	socket.on('LOGIN', function(player){
	   
	   console.log('[INFO] Player ' + player.name + ' conectando!');
	   currentUser = {
			name:player.name,
			id:shortId.generate(),
			sessao:player.sessao,
			players:0,
			flgEnvio:0,
			idObj:0
		}
		
		for(i = 0; i < clients.length; i++){
		
			if(clients[i].sessao == currentUser.sessao) //loop para verificar se existe uma sessão já criada
			{	
				
				if(currentUser.players != 2)
				{
				console.log('[INFO] Segundo player entrando na sessão: ' + currentUser.sessao);
				currentUser.players = 2;//segunda a entrar na sala
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
		}
		
		if(currentUser.players != -1)
		{
			clients.push(currentUser);
			console.log('Total players: ' + currentUser.players);
			console.log('Inserido player: ' + currentUser.id);
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
	
	/*Método responsável por verificar se existe algum objeto enviado*/
	socket.on('AGUARDAR', function(player){
		for(i = 0; i < clients.length; i++){
			if(clients[i].sessao == player.sessao && clients[i].id != player.id) //verifica o outro usuário conectado a mesma sessão
			{
				if(clients[i].flgEnvio == 1)//se a flag estiver ativa, significa que o outro usuário enviou um objeto
				{
					console.log('[INFO] Player ' + player.id + ' recebendo objeto!');
					clients[i].flgEnvio = 0;
					
					socket.emit('RECEBE_OBJ',clients[i]);
					i = clients.length;
				}
			}
		}
	
	});

	
	
});


console.log("------- server is running -------");
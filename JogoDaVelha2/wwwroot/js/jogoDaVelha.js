
const {createApp} = Vue

createApp({
    data() {
        return {
            jogo: null,
            meuGuid: null,
            guidAdversario: null,

            //https://docs.microsoft.com/pt-br/aspnet/core/signalr/introduction?WT.mc_id=dotnet-35129-website&view=aspnetcore-6.0
            connection: null,
        }
    },
    methods: {
        jogadorX(id) {
            if (this.jogo.vetor[id] == 'x')
                return true;
            return false;
        },
        jogadorY(id) {
            if (this.jogo.vetor[id] == '0')
                return true;
            return false;
        },
        async jogar(linha, coluna) {

            let retorno = await fetchData.fetchGetJson('jogo/jogar/' + this.jogo.id + '/' + linha + '/' + coluna);

            if (retorno) {

                console.log("Status da jogada: " + retorno.statusDaJogada + " linha: " + linha + " coluna: " + coluna);

                this.jogo = retorno;

                if (retorno.statusDaJogada) {

                    // mensagem que não deve ser exibida na tela do usuário
                    // usada para atualizar a tela do jogador que está aguardado
                    const guid = '#' + this.jogo.id + '#';
                    this.enviarMensagem(guid);

                    console.log("Variável ganhador: " + this.jogo.ganhador);
                    this.atualizarBotoes(true);
                }
                else {
                    console.log("Jogada não efetivada!");
                }
            }
        },
        atualizarBotoes(status) {

            // Fonte: https://developer.mozilla.org/pt-BR/docs/Web/API/Document/getElementsByClassName
            var botoes = document.getElementsByClassName("sendButton");
            var id = 0;

            Array.prototype.filter.call(botoes, (botao) => {

                botao.value = this.jogo.vetor[id];
                console.log("Botão " + id + ": " + botao.value);
                id++;

                if (this.jogo.ganhador == 'x' || this.jogo.ganhador == '0') {
                    botao.disabled = true;
                }
                else if (botao.value != '-') {
                    botao.disabled = true;
                }
                else {
                    console.log("Status: " + status);
                    botao.disabled = status;
                }
            });
        },
        async convidarAmigo() {
            var email = document.getElementById("campoEmail").value;
            var expressao = /\S+@\S+\.\S+/; // https://www.horadecodar.com.br/2020/09/13/como-validar-email-com-javascript/
            var valido = expressao.test(email);

            if (valido) {
                let retorno = await fetchData.fetchGetJson('jogo/convidarAmigo/' + email);

                if (retorno != null) {
                    console.log("Id: " + retorno.result.id);
                    console.log("Email: " + retorno.result.email);

                    this.guidAdversario = retorno.result.id;
                    this.enviarMensagem(this.meuGuid);

                    alert("Convite enviado com sucesso!");
                }
                else {
                    console.log("Email não encontrado!");
                    alert("Email não encontrado. Verifique se o email está correto!");
                }
            }
            else {
                alert("Email inválido. Insira um email válido.");
            }
        },
        async buscarJogo(guid) {

            let retorno = await fetchData.fetchGetJson('jogo/buscarJogo/' + guid);

            if (retorno) {
                this.jogo = retorno;
                this.atualizarBotoes(false);
            }
            else {
                console.log("Falha ao buscar jogo.");
            }
        },
        iniciarComunicacao() {

            if (this.connection == null) {

                // Fonte: https://docs.microsoft.com/pt-br/aspnet/core/tutorials/signalr?view=aspnetcore-6.0&tabs=visual-studio

                "use strict";

                this.connection = new signalR.HubConnectionBuilder().withUrl("/chatHub").build();

                //Disable the send button until connection is established.
                var botao1 = document.getElementById("idBotaoEnviarMsg");
                if (botao1)
                    botao1.disabled = true;

                /*
                    https://www.w3schools.com/js/js_arrow_function.asp
    
                    https://developer.mozilla.org/pt-BR/docs/Web/JavaScript/Reference/Functions/Arrow_functions
                */

                this.connection.on("ReceiveMessage", async (user, message) => {

                    // We can assign user-supplied strings to an element's textContent because it
                    // is not interpreted as markup. If you're assigning in any other way, you
                    // should be aware of possible script injection concerns.

                    //#11a5ac7a-ec15-4826-b289-cbeeb6e8d47f# = 38

                    console.log("Recebido: " + message);
                    const tracos = (message.match(/-/g) || []).length;
                    const cerquilha = (message.match(/#/g) || []).length;

                    console.log("Guid adversário: " + this.guidAdversario);
                    console.log("traços: " + tracos);
                    console.log("Tamanho da messagem? " + message.length);

                    // atualiza jogo do adversário
                    if (message.length == 38 && tracos == 4 && cerquilha == 2) {

                        var guidLimpo = message.replace(/#/g, '');

                        console.log("Atualizar jogo no adversário..." + guidLimpo);

                        this.buscarJogo(guidLimpo);
                    }
                    // envia o guid para o adversário para configurar envio de mensagens
                    else if (message.length == 36 && tracos == 4 && this.guidAdversario == null) {
                        console.log("Entrou no else if....");
                        this.guidAdversario = message;
                    }
                    else {
                        var p = document.createElement("p");

                        console.log("User: " + user + "  Meu guid: " + this.meuGuid);

                        if (user == this.meuGuid) {
                            p.classList.add('alinhaDireita');
                            document.getElementById("messagesList").appendChild(p);
                            p.textContent = `${message} : Adiversário`;
                        }
                        else {
                            p.classList.add('alinhaEsquerda');
                            document.getElementById("messagesList").appendChild(p);
                            p.textContent = `Você: ${message}`;
                        }
                    }
                });

                this.connection.start().then(function () {
                    var botao2 = document.getElementById("idBotaoEnviarMsg");
                    if (botao2)
                        botao2.disabled = false;
                }).catch(function (err) {
                    return console.error(err.toString());
                });
            }
        },
        async criarJogo() {

            let retorno = await fetchData.fetchGetJson('jogo/novoJogo');

            if (retorno) {

                console.log("Meu id: " + retorno.id);

                this.jogo = retorno;
                this.meuGuid = retorno.idUser;
                //var message = document.getElementById("messageInput").value = retorno.retorno.id;
            }
            else {
                console.log("Não foi possível criar novo jogo ....");
            }

            if (this.connection == null) {
                this.iniciarComunicacao();
            }
            else { // nova partida entre os jogadores já conectados
                // não basta atulizar, preciso limpar todos os botões
                this.atualizarBotoes(false);
            }
        },
        enviarMensagem(guid) {

            var message = document.getElementById("messageInput").value;

            document.getElementById("messageInput").value = "";

            if (guid != null)
                message = guid;

            console.log("User: " + this.guidAdversario + " Message: " + message);

            this.connection.invoke("SendPrivateMessage", this.guidAdversario, message).catch(function (err) {
                return console.error(err.toString());
            });

            if (guid == null) {
                // insere a mensagem em sua lista
                var p = document.createElement("p");
                p.classList.add('alinhaEsquerda');
                document.getElementById("messagesList").appendChild(p);
                p.textContent = `Você: ${message}`;
            }
        }
    },
    watch: {

    },
    beforeMount() {

        document.addEventListener("keypress", function (e) {

            // se a tecla Enter for pressionada
            if (e.key === 'Enter') {

                var btn = document.querySelector("#idBotaoEnviarMsg");

                // ativa o botão de envio de mensagem
                btn.click();

            }
        });

    }
}).mount('#app')
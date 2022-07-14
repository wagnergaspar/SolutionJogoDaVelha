
//// Fonte: https://docs.microsoft.com/pt-br/aspnet/core/tutorials/signalr?view=aspnetcore-6.0&tabs=visual-studio

//"use strict";

//var connection = new signalR.HubConnectionBuilder().withUrl("/chatHub").build();

////Disable the send button until connection is established.
//var botao1 = document.getElementById("sendButton");
//if (botao1)
//    botao1.disabled = true;

//connection.on("ReceiveMessage", function (user, message) {
//    var li = document.createElement("li");
//    document.getElementById("messagesList").appendChild(li);
//    // We can assign user-supplied strings to an element's textContent because it
//    // is not interpreted as markup. If you're assigning in any other way, you
//    // should be aware of possible script injection concerns.

//    jogar(-1,-1);
//    li.textContent = `${user} says ${message}`;
//});

//connection.start().then(function () {
//    var botao2 = document.getElementById("sendButton");
//    if (botao2)
//        botao2.disabled = false;
//}).catch(function (err) {
//    return console.error(err.toString());
//});

////var botao3 = document.getElementById("sendButton");
////if (botao3) {
////    botao3.addEventListener("click", function (event) {
////        var user = document.getElementById("userInput").value;
////        var message = document.getElementById("messageInput").value;
////        connection.invoke("SendMessage", user, message).catch(function (err) {
////            return console.error(err.toString());
////        });
////        event.preventDefault();
////    });
////}
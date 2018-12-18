// Initialize the Chat widget and implement the handlers for its post and typingStart events.

$(function () {
    let chat = $("#chat").kendoChat({
        // Each instance of the app will generate a unique username.
        // In this way, the SignalR Hub "knows" who is the user that sends the message
        // and who are the clients that have to receive that message.
        user: {
            id: kendo.guid(),
            // Get the username from identity and set it to name:
            name: username,
            iconUrl: "http://demos.telerik.com/kendo-ui/content/chat/avatar.png"
        },
        // This will notify the SignallR Hub that the current client is typing.
        // The Hub, in turn, will notify all the other clients
        // that the user has started typing.
        typingStart: function () {
            chatHub.invoke("sendTyping", chat.getUser())
        },
        // The post handler will send the user data and the typed text to the SignalR Hub.
        // The Hub will then forward that info to the other clients.
        post: function (args) {
            chatHub.invoke("send", chat.getUser(), args.text, username);
        }
    }).data("kendoChat");

    window.chatHub = new signalR.HubConnectionBuilder()
        .withUrl('https://localhost:5001/chat')
        .build();

    chatHub.start()
        .catch(function (err) {
            console.error(err.toString());
        });

    chatHub.on('broadcastMessage', function (sender, message) {
        var message = {
            type: 'text',
            text: message
        };

        // Render the received message in the Chat
        chat.renderMessage(message, sender)
    })

    chatHub.on('typing', function (sender) {
        // Display typing notification in the Chat
        chat.renderMessage({ type: 'typing' }, sender);
    })
})

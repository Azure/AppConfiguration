// Write your Javascript code.

function displayMessages(messages, refreshRate) {

    var messageBox = document.getElementById("messageBox");

    if (messageBox) {

        var index = 0;

        messageBox.textContent = messages[index];

        window.setInterval(function () {

            index = (index + 1) % messages.length;

            messageBox.textContent = messages[index];

        }, refreshRate || 1000)
    }
}
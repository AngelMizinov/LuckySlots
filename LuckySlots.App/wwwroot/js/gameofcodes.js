$('document').ready(function () {
    let gameName = window.location.pathname.split('/').pop()
    let canvas = document.querySelector('canvas')
    let canvasHeight
    let canvasWidth
    let imageHeight
    let imageWidth
    let apple = new Image()
    let banana = new Image()
    let pineapple = new Image()
    let wildcard = new Image()

    setupGameCanvas(gameName)

    function setupGameCanvas(gameName) {
        if (gameName == 'gameofcodes') {
            canvasHeight = 400
            canvasWidth = 300
            imageHeight = 100
            imageWidth = 100
            apple.src = '../images/games/game-of-codes/haskell.png'
            banana.src = '../images/games/game-of-codes/csharp.svg'
            pineapple.src = '../images/games/game-of-codes/javascript.png'
            wildcard.src = '../images/games/game-of-codes/wildcard.png'
        } else if (gameName == "tuttifrutti") {
            canvasHeight = 400
            canvasWidth = 400
            imageHeight = 80
            imageWidth = 80
            apple.src = '../images/games/tuttifrutti/apple.png'
            banana.src = '../images/games/tuttifrutti/banana.png'
            pineapple.src = '../images/games/tuttifrutti/pineapple.png'
            wildcard.src = '../images/games/tuttifrutti/wildcard.png'
        } else if (gameName == 'treasuresofegypt') {
            canvasHeight = 400
            canvasWidth = 400
            imageHeight = 50
            imageWidth = 80
            apple.src = '../images/games/treasures-of-egypt/mummy.png'
            banana.src = '../images/games/treasures-of-egypt/nefertiti.png'
            pineapple.src = '../images/games/treasures-of-egypt/pyramid.png'
            wildcard.src = '../images/games/treasures-of-egypt/seven.png'
        }
    }

    canvas.height = canvasHeight
    canvas.width = canvasWidth
    let ctx = canvas.getContext('2d')

    $('#spin-button').on('click', function () {
        $.ajax({
            method: 'POST',
            url: '/game/getgame',
            data: {
                gameName: gameName,
                stake: $('#stake-input').val()
            },
            dataType: 'json'
        }).done(function (data) {
            drawGame(data)

            let balanceSpan = $('#balance-amount')
            let balance = +balanceSpan.text()

            if (data.Winnings > 0) {
                balance += +data.Winnings
            } else {
                balance -= $('#stake-input').val()
            }

            balanceSpan.html(balance.toFixed(2))
        })
    })

    function drawGame(data) {
        ctx.clearRect(0, 0, canvas.width, canvas.height)
        for (let i = 0; i < (canvasWidth / imageWidth); i++) {
            for (let j = 0; j < (canvasHeight / imageHeight); j++) {
                let currentImage

                if (data.GameGrid[j][i] == 'apple') {
                    currentImage = apple
                } else if (data.GameGrid[j][i] == 'banana') {
                    currentImage = banana
                } else if (data.GameGrid[j][i] == 'pineapple') {
                    currentImage = pineapple
                } else {
                    currentImage = wildcard
                }

                ctx.drawImage(currentImage, i * imageWidth, j * imageHeight, imageWidth, imageHeight)
            }
        }

        for (let i = 0; i < data.WinningRows.length; i++) {
            let currentRow = data.WinningRows[i]
            ctx.strokeStyle = 'red'
            ctx.lineWidth = 15
            ctx.beginPath()
            ctx.moveTo(i, currentRow * imageHeight + imageHeight / 2)
            ctx.lineTo(canvasWidth, currentRow * imageHeight + imageHeight / 2)
            ctx.stroke()
        }

        //alert(`You win: ${data.Winnings}`)
    }
})
function startGame_TaiwanMahjong(tableId) {
    if (!isFlashPlayerInstalled()) {
        window.location = 'https://get.adobe.com/tw/flashplayer';
        return;
    }

    var swfUrl = 'http://game1.betis73168.com:81/games/TWMAHJONGVIDEO/FlashTwMj.swf?v=78bd3c69-e1aa-430e-8695-b4c5f15ae3ab';
    var flashvars = {
        gameUrl: 'http://game1.betis73168.com:81/games/TWMAHJONGVIDEO',
        gameName: 'TWMAHJONGVIDEO',
        gameCulture: 'zh-CN',
        gameFileToken: '78bd3c69-e1aa-430e-8695-b4c5f15ae3ab',
        serverUrl: 'gs1.betis73168.com',
        serverPort: '5001',
        accessToken: '104f03e805c34be39bd6ed3f92715848',
        rtmpUrl: 'rtmp://wowza.igig178.com/qachatnow',
        recognitionUrl: 'http://gs1.betis73168.com:9090/recognitionservice/rest/',
        tableId: tableId
    };

    $('#lobby').remove();
    embedSwf(swfUrl, flashvars, 'game', '100%', '100%');
}

function startGame_DouDizhu(tableId) {
    if (!isFlashPlayerInstalled()) {
        window.location = 'https://get.adobe.com/tw/flashplayer';
        return;
    }

    var swfUrl = 'http://game1.betis73168.com:81/games/DOUDIZHUVIDEO/Main.swf?v=70af4bca-a7e6-49ab-a807-0c42b278f573';
    var flashvars = {
        gameUrl: 'http://game1.betis73168.com:81/games/DOUDIZHUVIDEO',
        gameName: 'DOUDIZHUVIDEO',
        gameCulture: 'zh-CN',
        gameFileToken: '70af4bca-a7e6-49ab-a807-0c42b278f573',
        serverUrl: 'gs1.betis73168.com',
        serverPort: '9032',
        accessToken: '104f03e805c34be39bd6ed3f92715848',
        rtmpUrl: 'rtmp://wowza.igig178.com/qachatnow',
        recognitionUrl: 'http://gs1.betis73168.com:9090/recognitionservice/rest/',
        tableId: tableId
    };

    $('#lobby').remove();
    embedSwf(swfUrl, flashvars, 'game', '100%', '100%');
}

function startGame_TexasHoldem(tableId) {
    if (!isFlashPlayerInstalled()) {
        window.location = 'https://get.adobe.com/tw/flashplayer';
        return;
    }

    var swfUrl = 'http://game1.betis73168.com:81/games/TEXASHOLDEMVIDEO/Main.swf?v=41be1a9c-25a3-4f30-a967-dd6d91e2fe7a';
    var flashvars = {
        gameUrl: 'http://game1.betis73168.com:81/games/TEXASHOLDEMVIDEO',
        gameName: 'TEXASHOLDEMVIDEO',
        gameCulture: 'zh-CN',
        gameFileToken: '41be1a9c-25a3-4f30-a967-dd6d91e2fe7a',
        serverUrl: 'gs1.betis73168.com',
        serverPort: '7979',
        accessToken: 'dcd2c7948e234bb59e9379f8bef15532',
        rtmpUrl: 'rtmp://wowza.igig178.com/qachatnow',
        recognitionUrl: 'http://gs1.betis73168.com:9090/recognitionservice/rest/',
        tableId: tableId
    };

    $('#lobby').remove();
    embedSwf(swfUrl, flashvars, 'game', '100%', '100%');
}

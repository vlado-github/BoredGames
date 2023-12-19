import { Application, Sprite, Container } from 'pixi.js'
import axios from 'axios'

axios.get('https://localhost:7075/api/game/titles', {
    accept: 'application/json',
    headers: {
        'X-BORED-GAMES-API-KEY': 'BoredGames'
    }
  })
  .then(function (response) {
    console.log('>>>'+JSON.stringify(response.data));
  })
  .catch(function (error) {
    console.log(error);
  })
  .finally(function () {
    // always executed
  });  



const app = new Application({
    resolution: window.devicePixelRatio || 1,
    view: document.querySelector('#pixi-canvas'),
    autoDensity: true,
    backgroundColor: 0x6495ed,
    resizeTo: window
});

const conty = new Container();
app.stage.addChild(conty);

const imageWidth = 250;
const imageHeight = 250
const w = app.screen.width/3;
const h = app.screen.height/3;

const offline = Sprite.from("offline.png");
offline.x = app.screen.width - w - imageWidth;
offline.y = app.screen.height - 2*h - imageHeight;
conty.addChild(offline);

const rock = Sprite.from("rock.png");
rock.x = app.screen.width - 2*w - imageWidth;
rock.y = app.screen.height - imageHeight;
conty.addChild(rock);
rock.eventMode = 'static';
rock.cursor = 'pointer';
rock.on('pointerdown', rockClick);

const paper = Sprite.from("paper.png");
paper.x = app.screen.width - w - imageWidth;
paper.y = app.screen.height - imageHeight;
conty.addChild(paper);
paper.eventMode = 'static';
paper.cursor = 'pointer';
paper.on('pointerdown', paperClick);

const scissors = Sprite.from("scissors.png");
scissors.x = app.screen.width - imageWidth;
scissors.y = app.screen.height - imageHeight;
conty.addChild(scissors);
scissors.eventMode = 'static';
scissors.cursor = 'pointer';
scissors.on('pointerdown', scissorsClick);


function scissorsClick()
{
    scissors.x = app.screen.width/2;
    conty.removeChild(rock);
    conty.removeChild(paper);
}

function rockClick()
{
    rock.x = app.screen.width/2;
    conty.removeChild(scissors);
    conty.removeChild(paper);
}

function paperClick()
{
    paper.x = app.screen.width/2;
    conty.removeChild(rock);
    conty.removeChild(scissors);
}
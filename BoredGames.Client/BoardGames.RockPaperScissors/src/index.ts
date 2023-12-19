import { Application, Sprite, Container } from 'pixi.js'

const app = new Application({
    view: document.getElementById("pixi-canvas") as HTMLCanvasElement,
    resolution: window.devicePixelRatio || 1,
    autoDensity: true,
    backgroundColor: 0x6495ed,
    resizeTo: window
});

const conty: Container = new Container();
app.stage.addChild(conty);

const imageWidth = 250;
const imageHeight = 250
const w = app.screen.width/3;
const h = app.screen.height/3;

const offline: Sprite = Sprite.from("offline.png");
offline.x = app.screen.width - w - imageWidth;
offline.y = app.screen.height - 2*h - imageHeight;
conty.addChild(offline);

const rock: Sprite = Sprite.from("rock.png");
rock.x = app.screen.width - 2*w - imageWidth;
rock.y = app.screen.height - imageHeight;
conty.addChild(rock);
// Opt-in to interactivity
rock.eventMode = 'static';
// Shows hand cursor
rock.cursor = 'pointer';
// Pointers normalize touch and mouse (good for mobile and desktop)
rock.on('pointerdown', rockClick);

const paper: Sprite = Sprite.from("paper.png");
paper.x = app.screen.width - w - imageWidth;
paper.y = app.screen.height - imageHeight;
conty.addChild(paper);
// Opt-in to interactivity
paper.eventMode = 'static';
// Shows hand cursor
paper.cursor = 'pointer';
// Pointers normalize touch and mouse (good for mobile and desktop)
paper.on('pointerdown', paperClick);

const scissors: Sprite = Sprite.from("scissors.png");
scissors.x = app.screen.width - imageWidth;
scissors.y = app.screen.height - imageHeight;
conty.addChild(scissors);
// Opt-in to interactivity
scissors.eventMode = 'static';
// Shows hand cursor
scissors.cursor = 'pointer';
// Pointers normalize touch and mouse (good for mobile and desktop)
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
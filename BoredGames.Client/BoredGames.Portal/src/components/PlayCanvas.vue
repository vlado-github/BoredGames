<script>
import * as PIXI from 'pixi.js';
import backsideImage from '../assets/backside.png';
import rockImage from '../assets/rock.png';
import paperImage from '../assets/paper.png';
import scissorsImage from '../assets/scissors.png';

export default {
  mounted() {
    // Create PIXI application
    this.app = new PIXI.Application({
      resolution: window.devicePixelRatio || 1,
      view: document.querySelector('#pixi-canvas'),
      autoDensity: true,
      backgroundColor: 0x1099bb,
      resizeTo: window
    });

    this.container = new PIXI.Container();

    this.app.stage.addChild(this.container);

    const imageWidth = 250;
    const imageHeight = 250
    const w = this.app.screen.width/3;
    const h = this.app.screen.height/3;

    this.foldedCard01 = PIXI.Sprite.from(backsideImage);
    this.foldedCard01.x = this.app.screen.width - 2*w - imageWidth;
    this.foldedCard01.y = this.app.screen.height - 2*h - imageHeight;
    this.container.addChild(this.foldedCard01);

    this.foldedCard02 = PIXI.Sprite.from(backsideImage);
    this.foldedCard02.x = this.app.screen.width - w - imageWidth;
    this.foldedCard02.y = this.app.screen.height - 2*h - imageHeight;
    this.container.addChild(this.foldedCard02);

    this.foldedCard03 = PIXI.Sprite.from(backsideImage);
    this.foldedCard03.x = this.app.screen.width - imageWidth;
    this.foldedCard03.y = this.app.screen.height - 2*h - imageHeight;
    this.container.addChild(this.foldedCard03);

    this.rock = PIXI.Sprite.from(rockImage);
    this.rock.x = this.app.screen.width - 2*w - imageWidth;
    this.rock.y = this.app.screen.height - imageHeight;
    this.container.addChild(this.rock);
    this.rock.eventMode = 'static';
    this.rock.cursor = 'pointer';
    this.rock.on('pointerdown', this.rockClick);

    this.paper = PIXI.Sprite.from(paperImage);
    this.paper.x = this.app.screen.width - w - imageWidth;
    this.paper.y = this.app.screen.height - imageHeight;
    this.container.addChild(this.paper);
    this.paper.eventMode = 'static';
    this.paper.cursor = 'pointer';
    this.paper.on('pointerdown', this.paperClick);

    this.scissors = PIXI.Sprite.from(scissorsImage);
    this.scissors.x = this.app.screen.width - imageWidth;
    this.scissors.y = this.app.screen.height - imageHeight;
    this.container.addChild(this.scissors);
    this.scissors.eventMode = 'static';
    this.scissors.cursor = 'pointer';
    this.scissors.on('pointerdown', this.scissorsClick);
  },
  beforeDestroy() {
    // Destroy PIXI application when the component is destroyed
    this.app.destroy();
  },
  methods: {
    scissorsClick(event)
    {
        this.scissors.x = this.app.screen.width/2;
        this.container.removeChild(this.rock);
        this.container.removeChild(this.paper);
    },

    rockClick(event)
    {
        this.rock.x = this.app.screen.width/2;
        this.container.removeChild(this.scissors);
        this.container.removeChild(this.paper);
    },

    paperClick(event)
    {
        this.paper.x = this.app.screen.width/2;
        this.container.removeChild(this.rock);
        this.container.removeChild(this.scissors);
    },
  },
};
</script>

<template>
  <div id="pixi-content"><canvas id="pixi-canvas" /></div>
</template>


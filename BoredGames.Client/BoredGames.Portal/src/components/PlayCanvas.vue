<script>
import * as PIXI from 'pixi.js';
import apiService from '@/api/api';
import GameStatusEnum from '@/consts/gameStatusEnum';

import backsideImage from '../assets/backside.png';
import rockImage from '../assets/rock.png';
import paperImage from '../assets/paper.png';
import scissorsImage from '../assets/scissors.png';

export default {
  data() {
    return {
      gameId: '',
      gameStatus: 0
    }
  },
  beforeUnmount() {
    clearInterval(this.refreshInterval);
  },
  async mounted() {
    // Join game if not already joined
    let gameId = localStorage.getItem('gameId');
    if (!gameId) {
      const response = await joinGame();
      gameId = response.gameId; 
    }
    localStorage.setItem('gameId', gameId);
    this.gameId = gameId;
    await this.updateGameStatus();

    // Check game state periodically
    this.refreshInterval = setInterval(async () => {
      await this.updateGameStatus();
    }, 1000);

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
    const screenWidth = this.app.screen.width;
    const screenHeight = this.app.screen.height;

    if (this.gameStatus === GameStatusEnum.AwaitingPlayers) {
      const awaitingPlayerMessage = new PIXI.Text('Awaiting player', {
        fontFamily: 'Arial',
        fontSize: 36,
        fill: 0xffffff,
        x: screenWidth - 2*w - imageWidth,
        y: screenHeight - 2*h - imageHeight
      });
      this.container.addChild(awaitingPlayerMessage);
    }
    else if (this.gameStatus === GameStatusEnum.InPlay) {
      this.foldedCard01 = PIXI.Sprite.from(backsideImage);
      this.foldedCard01.x = screenWidth - 2*w - imageWidth;
      this.foldedCard01.y = screenHeight - 2*h - imageHeight;
      this.container.addChild(this.foldedCard01);

      this.foldedCard02 = PIXI.Sprite.from(backsideImage);
      this.foldedCard02.x = screenWidth - w - imageWidth;
      this.foldedCard02.y = screenHeight - 2*h - imageHeight;
      this.container.addChild(this.foldedCard02);

      this.foldedCard03 = PIXI.Sprite.from(backsideImage);
      this.foldedCard03.x = screenWidth - imageWidth;
      this.foldedCard03.y = screenHeight - 2*h - imageHeight;
      this.container.addChild(this.foldedCard03);
    }
    else {
      const awaitingPlayerMessage = new PIXI.Text('score:', {
        fontFamily: 'Arial',
        fontSize: 36,
        fill: 0xffffff,
        x: screenWidth/2,
        y: screenHeight/2
      });
      this.container.addChild(awaitingPlayerMessage);
    }

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
    async joinGame() {
      return await apiService.joinGame({
        gameId: this.$route.params.gameInstanceId,
        playerNickName: "player02"
      });
    },
    async updateGameStatus() {
      const response = await apiService.getGameState(this.gameId);
      this.gameStatus = response.gameStatus;
    },

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


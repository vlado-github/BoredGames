<script>
import * as PIXI from 'pixi.js';
import apiService from '@/api/api';
import GameStatusEnum from '@/consts/gameStatusEnum';
import LocalStorageKeys from '@/consts/localStorageKeys';

import backsideImage from '../assets/backside.png';
import rockImage from '../assets/rock.png';
import paperImage from '../assets/paper.png';
import scissorsImage from '../assets/scissors.png';

const imageWidth = 250;
const imageHeight = 250

export default {
  name: 'cardTable',

  data() {
    return {
      gameId: '',
      gameStatus: 0,
      screenWidth: 800,
      screenHeight: 600
    }
  },
  watch: {
    gameStatus: async function (currentValue) {
      if (currentValue === GameStatusEnum.InPlay)
      {
        this.setOpponentHand();
      } else if (currentValue === GameStatusEnum.Finished) {
        await this.displayScore();
      }
    }
  },
  async beforeMount() {
    // Join game if not already joined
    await this.joinGame();

    // Check game state periodically
    this.refreshInterval = setInterval(async () => {
      await this.updateGameStatus();
    }, 1000);
  },
  beforeUnmount() {
    clearInterval(this.refreshInterval);
  },
  beforeDestroy() {
    this.app.destroy();
  },
  async mounted() {
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

    this.screenWidth = this.app.screen.width;
    this.screenHeight = this.app.screen.height;

    if (this.gameStatus === GameStatusEnum.AwaitingPlayers) {
      this.displayAwaitingMessage();
    }

    this.setPlayerHand();
  },
  methods: {
    async joinGame() {
      let gameId = localStorage.getItem(LocalStorageKeys.GameId);
      let routeParam = this.$route.params.gameInstanceId;

      if (!gameId || gameId != routeParam) {
        const response = await apiService.joinGame({
          gameId: routeParam,
          playerNickName: "player02"
        });
        gameId = response.gameId; 
      }
      localStorage.setItem(LocalStorageKeys.GameId, gameId);
      this.gameId = gameId;
      await this.updateGameStatus();
    },

    async updateGameStatus() {
      const response = await apiService.getGameState(this.gameId);
      this.gameStatus = response.gameStatus;
    },

    async makeMove(move) {
      return await apiService.makeMove({
        gameId: this.gameId, 
        actionType: move 
      });
    },

    async getWinner() {
      return await apiService.getGameWinner(this.gameId);
    },

    async getScore() {
      return await apiService.getGameScore(this.gameId);
    },

    setPlayerHand() {
      const w = this.screenWidth/3;
      const screenWidth = this.screenWidth;
      const screenHeight = this.screenHeight;

      this.rock = PIXI.Sprite.from(rockImage);
      this.rock.width = imageWidth;
      this.rock.height = imageHeight;
      this.rock.x = screenWidth - 2*w - imageWidth;
      this.rock.y = screenHeight - imageHeight;
      this.container.addChild(this.rock);
      this.rock.eventMode = 'static';
      this.rock.cursor = 'pointer';
      this.rock.on('pointerdown', this.rockClick);

      this.paper = PIXI.Sprite.from(paperImage);
      this.paper.width = imageWidth;
      this.paper.height = imageHeight;
      this.paper.x = screenWidth - w - imageWidth;
      this.paper.y = screenHeight - imageHeight;
      this.container.addChild(this.paper);
      this.paper.eventMode = 'static';
      this.paper.cursor = 'pointer';
      this.paper.on('pointerdown', this.paperClick);

      this.scissors = PIXI.Sprite.from(scissorsImage);
      this.scissors.width = imageWidth;
      this.scissors.height = imageHeight;
      this.scissors.x = this.screenWidth - imageWidth;
      this.scissors.y = this.screenHeight - imageHeight;
      this.container.addChild(this.scissors);
      this.scissors.eventMode = 'static';
      this.scissors.cursor = 'pointer';
      this.scissors.on('pointerdown', this.scissorsClick);
    },

    setOpponentHand() {
      const w = this.screenWidth/3;
      const h = this.screenHeight/3;
      const screenWidth = this.screenWidth;
      const screenHeight = this.screenHeight;

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

      this.container.removeChild(this.awaitingPlayerMessage);
    },

    displayAwaitingMessage(){
      const h = this.screenHeight/3;

      this.awaitingPlayerMessage = new PIXI.Text('Awaiting player', {
        fontFamily: 'Arial',
        fontSize: 36,
        fill: 0xffffff,
        x: this.screenWidth/2,
        y: this.screenHeight - 2*h - imageHeight
      });
      this.container.addChild(this.awaitingPlayerMessage);
    },

    async displayScore(){
      const response = await apiService.getGameWinner(this.gameId);
      this.awaitingPlayerMessage = new PIXI.Text(`score: ${response[0].nickName}`, {
        fontFamily: 'Arial',
        fontSize: 36,
        fill: 0xffffff,
        x: this.screenWidth/2,
        y: this.screenHeight/2
      });
      this.container.addChild(this.awaitingPlayerMessage);
    },

    async scissorsClick(event)
    {
        this.scissors.x = this.app.screen.width/2;
        this.container.removeChild(this.rock);
        this.container.removeChild(this.paper);
        await this.makeMove('scissors');
    },

    async rockClick(event)
    {
        this.rock.x = this.app.screen.width/2;
        this.container.removeChild(this.scissors);
        this.container.removeChild(this.paper);
        await this.makeMove('rock');
    },

    async paperClick(event)
    {
        this.paper.x = this.app.screen.width/2;
        this.container.removeChild(this.rock);
        this.container.removeChild(this.scissors);
        await this.makeMove('paper');
    },
  },
};
</script>

<template>
  <div id="pixi-content"><canvas id="pixi-canvas" /></div>
</template>


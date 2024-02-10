<script>
import * as PIXI from 'pixi.js';
import apiService from '@/api/api';
import GameStatusEnum from '@/consts/gameStatusEnum';
import LocalStorageKeys from '@/consts/localStorageKeys';
import { Table } from '@/games/rockpaperscissors'

const imageWidth = 250;
const imageHeight = 250;

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
      console.log('***'+currentValue)
      if (currentValue === GameStatusEnum.AwaitingPlayers) {
        displayAwaitingMessage();
      }
      else if (currentValue === GameStatusEnum.Finished) {
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
    }, 500);
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

    const gameSettings = {
      gameId: this.gameId,
      cardsPerHand: 3,
      screenWidth: this.app.screen.width,
      screenHeight: this.app.screen.height
    };

    const table = new Table();
    table.setup(gameSettings);

    this.app.stage.addChild(table);
  },
  methods: {
    async joinGame() {
      let gameId = localStorage.getItem(LocalStorageKeys.GameId);
      let routeParam = this.$route.params.gameInstanceId;

      if (!gameId || gameId != routeParam) {
        const response = await apiService.joinGame({
          gameId: routeParam,
          playerNickName: "Robert"
        });
        gameId = response.gameId; 
      }
      localStorage.setItem(LocalStorageKeys.GameId, gameId);
      this.gameId = gameId;
      await this.updateGameStatus();
    },

    async updateGameStatus() {
      const response = await apiService.getGameState(this.gameId);
      console.log('>>>'+response.gameStatus)
      this.gameStatus = response.gameStatus;
    },

    async getWinner() {
      return await apiService.getGameWinner(this.gameId);
    },

    async getScore() {
      return await apiService.getGameScore(this.gameId);
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
    }
  }
};
</script>

<template>
  <div id="pixi-content"><canvas id="pixi-canvas" /></div>
</template>


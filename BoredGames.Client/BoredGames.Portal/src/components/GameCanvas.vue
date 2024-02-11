<script>
import * as PIXI from 'pixi.js';
import apiService from '@/api/api';
import GameStatusEnum from '@/consts/gameStatusEnum';
import LocalStorageKeys from '@/consts/localStorageKeys';
import { Table, MessageBox } from '@/games/rockpaperscissors'

export default {
  name: 'gameScreen',

  data() {
    return {
      gameId: '',
      playerNickName: '',
      gameStatus: 0,
      screenWidth: 800,
      screenHeight: 600
    }
  },

  watch: {
    gameStatus: async function (currentValue) {
      if (currentValue !== GameStatusEnum.AwaitingPlayers) {
        const awaitingMessage = this.app.stage.children.find(x => x.name === 'awaitingMessage');
        if (awaitingMessage){
          awaitingMessage.visible = false;
        }
      }
      if (currentValue === GameStatusEnum.Finished) {
        const response = await apiService.getGameWinner(this.gameId);
        const gameOverMessage = this.app.stage.children.find(x => x.name === 'gameOverMessage');
        if (gameOverMessage){
          gameOverMessage.visible = true;
          if (response[0].id == localStorage.getItem(LocalStorageKeys.PlayerId)){
            gameOverMessage.text = 'Victory!';
          }
          else {
            gameOverMessage.text = 'Defeat';
          }
          
        }
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

    const displaySettings = {
      screenWidth: this.app.screen.width,
      screenHeight: this.app.screen.height,
      font: 'Arial',
      messageFillColor: '0xffffff',
    }

    const awaitingMessage = new MessageBox('awaitingMessage', 'Awaiting for player', 36);
    awaitingMessage.setup(displaySettings);
    this.app.stage.addChild(awaitingMessage);

    const gameOverMessage = new MessageBox('gameOverMessage', '', 36);
    gameOverMessage.setup(displaySettings);
    gameOverMessage.visible = false;
    this.app.stage.addChild(gameOverMessage);

    const table = new Table();
    table.setup(gameSettings);

    this.app.stage.addChild(table);
  },

  methods: {
    async joinGame() {
      let gameId = localStorage.getItem(LocalStorageKeys.GameId);
      let routeParam = this.$route.params.gameInstanceId;
      const request = {
          gameId: routeParam,
          playerNickName: "Robert"
      };

      if (!gameId || gameId != routeParam) {
        const response = await apiService.joinGame(request);
        gameId = response.gameId; 
      }
      localStorage.setItem(LocalStorageKeys.GameId, gameId);
      localStorage.setItem(LocalStorageKeys.PlayerNickName, request.playerNickName);
      this.gameId = gameId;
      this.playerNickName = request.playerNickName;

      await this.updateGameStatus();
    },

    async updateGameStatus() {
      const response = await apiService.getGameState(this.gameId);
      this.gameStatus = response.gameStatus;
      console.log('>>>'+response.gameStatus)
    },

    async getWinner() {
      return await apiService.getGameWinner(this.gameId);
    },

    async getScore() {
      return await apiService.getGameScore(this.gameId);
    }
  }
};
</script>

<template>
  <div id="pixi-content"><canvas id="pixi-canvas" /></div>
</template>


<script>
import * as PIXI from 'pixi.js';
import apiService from '@/api/api';
import GameStatusEnum from '@/consts/gameStatusEnum';
import LocalStorageKeys from '@/consts/localStorageKeys';
import { Table, MessageBox, Hand } from '@/games/rockpaperscissors';

const spritesheet = await PIXI.Assets.load(`${import.meta.env.VITE_BASE_URL}/assets/spritesheet.json`);

export default {
  name: 'gameScreen',

  data() {
    return {
      gameId: '',
      playerNickName: '',
      gameStatus: 0
    }
  },

  beforeUnmount() {
    clearInterval(this.refreshInterval);
  },

  beforeDestroy() {
    this.app.destroy();
  },

  async mounted() {  
    // Join game if not already joined
    await this.joinGame();

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
      cardDeck: ['rock', 'paper', 'scissors']
    };

    const screenRation = this.app.screen.width/this.app.screen.height;

    const displaySettings = {
      screenWidth: this.app.screen.width,
      screenHeight: this.app.screen.height,
      cardWidth: screenRation * 150,
      cardHeight: screenRation * 150,
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

    // Setup card table
    const table = new Table();
    const opponentHand = new Hand(gameSettings, displaySettings, true);
    const playerHand = new Hand(gameSettings, displaySettings, false);
    table.addChild(opponentHand);
    table.addChild(playerHand);
    this.app.stage.addChild(table);

    // Check game state periodically
    // todo: use SSE
    this.refreshInterval = setInterval(async () => {
      await this.updateGameStatus();
      if (this.gameStatus !== GameStatusEnum.AwaitingPlayers) {
          const awaitingMessage = this.app.stage.children.find(x => x.name === 'awaitingMessage');
          if (awaitingMessage){
            awaitingMessage.visible = false;
          }
      }
      if (this.gameStatus === GameStatusEnum.Finished) {
        await handleGameOver(this.app, this.gameId);
        clearInterval(this.refreshInterval);
      }
    }, 500);

    async function handleGameOver(app, gameId) {
      const winnerResponse = await apiService.getGameWinner(gameId);
      const scoreResponse = await apiService.getGameScore(gameId);
      const gameOverMessage = app.stage.children.find(x => x.name === 'gameOverMessage');
      if (gameOverMessage) {
        gameOverMessage.visible = true;
        const playerId = localStorage.getItem(LocalStorageKeys.PlayerId);

        if (winnerResponse[0].id == playerId) {
          gameOverMessage.text = 'Victory!';
          const opponentScore = scoreResponse.playerScores.find(x => x.playerId != playerId);
          if (opponentScore) {
            const opponentMove = opponentScore.roundLosses[0].playerMove;
            const handMiddlePosition = opponentHand.width/2;
            opponentHand.children.forEach(card => {
              if (card.cardType == opponentMove) {
                card.isSelected = true;
                card.texture = spritesheet.textures[`${opponentMove}.png`];
                card.x = handMiddlePosition;
                card.anchor.x = 0.5;
                card.visible = true;
              } else {
                card.visible = false;
              }
            });
          }
        }
        else {
          gameOverMessage.text = 'Defeat';
          const opponentScore = scoreResponse.playerScores.find(x => x.playerId != playerId);
          if (opponentScore) {
            const opponentMove = opponentScore.roundWins[0].playerMove;
            const handMiddlePosition = opponentHand.width/2;
            opponentHand.children.forEach(card => {
              if (card.cardType == opponentMove) {
                card.isSelected = true;
                card.texture = spritesheet.textures[`${opponentMove}.png`];
                card.x = handMiddlePosition;
                card.anchor.x = 0.5;
                card.visible = true;
              } else {
                card.visible = false;
              }
            });
          }
        }
      }
    }
  },

  methods: {
    async joinGame() {
      let gameId = localStorage.getItem(LocalStorageKeys.GameId);
      const routeParam = this.$route.params.gameInstanceId;
      const playerNickName = 'Jean-Luc Picard';

      if (!gameId || gameId != routeParam) {
        const response = await apiService.joinGame({
          gameId: routeParam,
          playerNickName: playerNickName
        });
        gameId = response.gameId; 
      }
      localStorage.setItem(LocalStorageKeys.GameId, gameId);
      localStorage.setItem(LocalStorageKeys.PlayerNickName, playerNickName);
      this.gameId = gameId;
      this.playerNickName = playerNickName;

      await this.updateGameStatus();
    },

    async updateGameStatus() {
      const response = await apiService.getGameState(this.gameId);
      this.gameStatus = response.gameStatus;
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


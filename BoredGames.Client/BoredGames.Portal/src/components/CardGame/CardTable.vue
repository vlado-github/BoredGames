<script>
import PlayerHand from './PlayerHand.vue'
import GameEndDialog from './../GameEndDialog.vue'
import PlayerDialog from './../PlayerDialog.vue'
import ScoreBoard from './ScoreBoard.vue'
import apiService from '@/api/api';
import LocalStorageKeys from '@/consts/localStorageKeys';

export default {
  name: 'cardTable',
  props: {
    gameInstanceId: ''
  },
  components: {
    PlayerHand,
    PlayerDialog,
    GameEndDialog,
    ScoreBoard
  },

  async mounted() {
    this.loadScore();
    //todo: card deck should be received as server response
    this.cardDeck = ['rock','paper','scissors'];
    this.currentPlayerId = localStorage.getItem(LocalStorageKeys.PlayerId);
    this.joinGame();
    this.refreshGameStatus();
  },

  data() {
    return {
      gameStatusInterval: Number,
      gameStatus: 0, //AwaitingPlayers
      currentRound: 1,
      latestRound: 1,
      playersScores: [],
      cardDeck: [],
      currentPlayerId: '',
      currentPlayerNickName: ''
    };
  },

  methods: {
    async loadScore() {
      let score = await apiService.getGameScore(this.gameInstanceId);
      this.playersScores = score.playerScores;
    },

    async joinGame() {
      this.currentPlayerNickName = localStorage.getItem(LocalStorageKeys.PlayerNickName);

      if (!this.currentPlayerNickName) {
        this.$refs.playerDialog.show();
      } else {
        apiService.joinGame({
            gameId: this.gameInstanceId,
            playerNickName: this.currentPlayerNickName
        });
      }
    },

    async refreshGameStatus() {
      this.gameStatusInterval = setInterval(() => {
          apiService.getGameState(this.gameInstanceId)
            .then(async (response) => {
              this.gameStatus = response.gameStatus;
              this.latestRound = response.roundNumber;
              

              if (this.latestRound > this.currentRound) { //Round completed
                this.currentRound = this.latestRound;
                this.isRoundCompleted = true;
                this.loadScore();
              }

              if (this.gameStatus == 2) { //Game Finished
                clearInterval(this.gameStatusInterval);
                await this.$refs.gameEndDialog.show();
              }
            })
            .catch(err => {
              console.log(err)
            })
        }, 1000);
    },
  }
}
</script>

<template>
  <div class="cardtable">
    <PlayerHand
        :cards="cardDeck"
        :player="{ foe: true, joined: this.gameStatus != 0 }"
        :gameInstanceId="gameInstanceId"
        :roundNumber="latestRound"
        :playerScore="playersScores.filter(x => x.playerId != this.currentPlayerId)[0]"
    />
    <ScoreBoard 
      :playersScores="playersScores"
      :currentPlayerId="currentPlayerId"/>
    <PlayerHand
        :cards="cardDeck"
        :player="{ foe: false, joined: true}"
        :gameInstanceId="gameInstanceId"
        :roundNumber="latestRound"
        :playerScore="playersScores.filter(x => x.playerId == this.currentPlayerId)[0]"
    />
    <GameEndDialog ref="gameEndDialog" :gameInstanceId="gameInstanceId" />
    <PlayerDialog ref="playerDialog" 
      :gameInstanceId="this.gameInstanceId" />
  </div>
</template>

<style>
    .cardtable {
        display: flex;
        align-items: center;
        justify-content: center;
        background: #56a87e;
        position: fixed;
        width: 100%;
        height: 100%;
        left: 0;
        top: 0; 
        align-items: center;
        justify-content: center;
    }
</style>

<script>
import PlayerHand from './PlayerHand.vue'
import GameEndDialog from './GameEndDialog.vue'
import apiService from '@/api/api';
import LocalStorageKeys from '@/consts/localStorageKeys';

export default {
  name: 'cardTable',
  props: {
    gameInstanceId: '',
  },
  components: {
    PlayerHand,
    GameEndDialog
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
      playerJoined: false,
      gameStatus: 0, //AwaitingPlayers
      currentRound: 1,
      latestRound: 1,
      playersScores: [],
      cardDeck: [],
      currentPlayerId: ''
    };
  },

  methods: {
    async loadScore() {
      let score = await apiService.getGameScore(this.gameInstanceId);
      this.playersScores = score.playerScores;
    },

    async joinGame() {
      if (!this.playerJoined) {
        //todo: show dialog to input name
        apiService.joinGame({
          gameId: this.gameInstanceId,
          playerNickName: 'Player'
        }).then(response => {
          this.playerJoined = true;
        }).catch(err => {
          console.log(err);
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
                console.log(">> card table: round" + this.currentRound+ " completed")
                this.loadScore();
              }

              if (this.gameStatus == 2) { //Game Finished
                console.log("game finished");
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
    <PlayerHand
        :cards="cardDeck"
        :player="{ foe: false, joined: true}"
        :gameInstanceId="gameInstanceId"
        :roundNumber="latestRound"
        :playerScore="playersScores.filter(x => x.playerId == this.currentPlayerId)[0]"
    />
    <GameEndDialog ref="gameEndDialog"
        :gameInstanceId="gameInstanceId"
    />
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

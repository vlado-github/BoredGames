<script>
import PlayerHand from './PlayerHand.vue'
import GameEndDialog from './GameEndDialog.vue'
import apiService from '@/api/api';

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
    this.cardDeck = ['rock','paper','scissors'];
    this.joinGame();
    this.refreshGameStatus();
  },

  data() {
    return {
      gameStatusInterval: Number,
      playerJoined: false,
      gameStatus: 0, //AwaitingPlayers
      cardDeck: Array
    };
  },

  methods: {
    async joinGame() {
      if (!this.playerJoined) {
        console.log(this.gameInstanceId);
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
              console.log(this.gameStatus);
              if (this.gameStatus == 2) { //Finished
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
    />
    <PlayerHand
        :cards="cardDeck"
        :player="{ foe: false, joined: true}"
        :gameInstanceId="gameInstanceId"
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

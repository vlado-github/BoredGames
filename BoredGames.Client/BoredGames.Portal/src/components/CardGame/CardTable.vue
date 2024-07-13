<script>
import PlayerHand from './PlayerHand.vue'
import apiService from '@/api/api';

export default {
  name: 'cardTable',
  props: {
    gameInstanceId: '',
  },
  components: {
    PlayerHand
  },

  async mounted() {
    this.joinGame();
    this.refreshGameStatus();
  },

  data() {
    return {
      gameStatusInterval: Number,
      playerJoined: false,
      gameStatus: 0, //AwaitingPlayers
      cardDeck: ['rock','paper','scissors']
    };
  },

  methods: {
    async joinGame() {
      if (!this.playerJoined) {
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
            .then(response => {
              this.gameStatus = response.gameStatus;
              console.log(this.gameStatus);
              if (this.gameStatus == 2) { //Finished
                alert("game finished");
                clearInterval(this.gameStatusInterval);
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

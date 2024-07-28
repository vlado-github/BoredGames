<script>
import Card from './Card.vue'
import apiService from '@/api/api';
import { useToast, POSITION } from "vue-toastification";
import GameResultEnum from '@/consts/gameResultEnum';

export default {
  name: 'playerHand',
  props: {
    cards: Array,
    player: {
      foe: false,
      joined: false
    },
    gameInstanceId: '',
    roundNumber: 0,
    playerScore: Object
  },
  watch: { 
    playerScore: function(newValue, oldValue) {
      this.onRoundCompleted(newValue);
    }
  },
  components: {
    Card
  },

  data() {
    return {
      selectedCard: '',
      isRoundCompleted: false
    }
  },

  methods: {
    async onCardSelect(cardSelected) {
      if (this.selectedCard == '') {
        await apiService.makeMove({
            gameId: this.gameInstanceId,
            actionType: cardSelected,
        });
        this.selectedCard = cardSelected;
      }
    },

    async onRoundCompleted() {
      this.isRoundCompleted = true;
     
      let previousRound = this.roundNumber - 1;
      let win = this.playerScore.roundWins?.filter(x => x.roundNumber == previousRound);
      if (win.some(x => x)){
        this.selectedCard = win[0].playerMove;
        if (!this.player.foe) {
          this.showRoundEndMessage(GameResultEnum.Win, "Round " + previousRound +" won!");
        }
      } else {
        let loss = this.playerScore.roundLosses?.filter(x => x.roundNumber == previousRound);
        if (loss.some(x => x)){
          this.selectedCard = loss[0].playerMove;
          if (!this.player.foe) {
            this.showRoundEndMessage(GameResultEnum.Loss, "Round " + previousRound +" lost.");
          }
        } else {
          let draw = this.playerScore.roundDraws?.filter(x => x.roundNumber == previousRound);
          if (draw.some(x => x)) {
            this.selectedCard = draw[0].playerMove;
            if (!this.player.foe) {
              this.showRoundEndMessage(GameResultEnum.Draw, "Round " + previousRound +" is a draw.");
            }
          }
        }
      }

      this.resetPlayerHand();
    },

    showRoundEndMessage(gameResult, message){        
      console.log(message);  
      const toast = useToast();   
      if (gameResult == GameResultEnum.Win) {  
        toast.success(message, {
          timeout: 2000,
          position: POSITION.TOP_CENTER,
        }); 
      } else if (gameResult == GameResultEnum.Loss) {
        toast.error(message, {
          timeout: 2000,
          position: POSITION.TOP_CENTER,
        });
      } else if (gameResult == GameResultEnum.Draw) {
        toast.warning(message, {
          timeout: 2000,
          position: POSITION.TOP_CENTER,
        });
      }         
    },

    resetPlayerHand() {
        setTimeout(() => {
          this.selectedCard = '';
          console.log(">>> reset hand")
        }, 500);
    }
  }
}
</script>

<template>
  <div v-if="player.joined === false" class='playerhand-against'>
    <label>Waiting for player to join</label>
  </div>
  <div v-else :class="player.foe ? 'playerhand-against' : 'playerhand-me'">
    <Card
      v-for="card in cards"
      :cardType="card"
      :isAgainstPlayer="player.foe"
      :gameInstanceId="gameInstanceId"
      :isRoundCompleted="isRoundCompleted"
      :isSelected="selectedCard == card"
      :isRendered="selectedCard == '' || selectedCard == card"
      @cardSelected="onCardSelect"
    />
  </div>
</template>

<style>
    .playerhand-me {
        display: table-row;
        position: fixed;
        bottom: 0;
        padding: 20px; 
        width: auto;
        height: 250px;
    }
    .playerhand-against {
        display: table-row;
        position: fixed;
        top: 0;
        padding: 20px; 
        width: auto;
        height: 250px;
    }
</style>

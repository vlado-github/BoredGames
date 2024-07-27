<script>
import Card from './Card.vue'
import apiService from '@/api/api';
import { useToast, POSITION } from "vue-toastification";

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
    playerScore: function(newValue, oldValue) { // watch it
      console.log('playerScore changed: ', JSON.stringify(newValue));
      //console.log('round'+ this.roundNumber)
      if (!this.player.foe) {
        this.onRoundCompleted(newValue);
      }
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
      await apiService.makeMove({
          gameId: this.gameInstanceId,
          actionType: cardSelected,
      });
      this.selectedCard = cardSelected;
    },

    async onRoundCompleted() {
      this.isRoundCompleted = true;

     
      let previousRound = this.roundNumber - 1;
      let win = this.playerScore.roundWins?.some(x => x.roundNumber == previousRound);
      if (win){
        this.showRoundEndMessage("Round " +previousRound +" won!")
      } else {
        let loss = this.playerScore.roundLosses?.some(x => x.roundNumber == previousRound);
        if (loss){
          this.showRoundEndMessage("Round " +previousRound +" lost.");
        } else {
          let draw = this.playerScore.roundDraws?.some(x => x.roundNumber == previousRound);
          if (draw) {
            this.showRoundEndMessage("Round " +previousRound +" is a draw.")
          }
        }
      }
      
    },

    showRoundEndMessage(message){        
      console.log(message);  
      const toast = useToast();     
      toast.success(message, {
        timeout: 2000,
        position: POSITION.TOP_CENTER
      });          
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

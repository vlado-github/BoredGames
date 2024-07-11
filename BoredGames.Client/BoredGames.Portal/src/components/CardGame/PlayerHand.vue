<script>
import Card from './Card.vue'
import apiService from '@/api/api';

export default {
  name: 'playerHand',
  props: {
    cards: [],
    player: {
      foe: false,
      joined: false
    },
    gameInstanceId: ''
  },
  
  components: {
    Card
  },

  mounted() {
    console.log(this.cards)
  },

  methods: {
    async receiveSelectedCard(selectedCardType) {
      alert(selectedCardType);

      await apiService.makeMove(new {
          gameId: this.gameInstanceId,
          actionType: selectedCardType,
      });
    }
  }
}
</script>

<template>
  <div v-if="player.joined === false">
    <label>Waiting for player to join</label>
  </div>
  <div v-else :class="player.foe ? 'playerhand-against' : 'playerhand-me'">
    <Card
      v-for="card in cards"
      :cardType="card"
      :isAgainstPlayer="player.foe"
      :gameInstanceId="gameInstanceId"
      @selectedCard="receiveSelectedCard"
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

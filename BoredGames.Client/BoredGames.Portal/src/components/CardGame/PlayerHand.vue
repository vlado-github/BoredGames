<script>
import Card from './Card.vue'
import apiService from '@/api/api';

export default {
  name: 'playerHand',
  props: {
    cards: Array,
    player: {
      foe: false,
      joined: false
    },
    gameInstanceId: ''
  },
  
  components: {
    Card
  },

  methods: {
    async receiveSelectedCard(cardSelected) {
      await apiService.makeMove({
          gameId: this.gameInstanceId,
          actionType: cardSelected,
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
      @cardSelected="receiveSelectedCard"
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

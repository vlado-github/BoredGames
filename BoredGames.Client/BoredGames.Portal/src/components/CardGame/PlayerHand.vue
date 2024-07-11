<script>
import Card from './Card.vue'

export default {
  name: 'playerHand',
  props: {
    cards: [],
    isAgainstPlayer: false,
  },
  components: {
    Card
  },

  // data() {
  //   return {
  //     cards: [],
  //     isAgainstPlayer: false
  //   };
  // }
}
</script>

<template>
  <div :class="isAgainstPlayer ? 'playerhand-against' : 'playerhand-me'">
    <Card
      v-for="card in cards"
      :cardType="card"
      :isAgainstPlayer="isAgainstPlayer"
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

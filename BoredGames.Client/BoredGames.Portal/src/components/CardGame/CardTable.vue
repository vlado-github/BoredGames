<script>
import PlayerHand from './PlayerHand.vue'

export default {
  name: 'cardTable',
  props: {
    gameInstanceId: '',
    cardDeck: [],
  },
  components: {
    PlayerHand
  },

  async mounted() {
    //const gameAssets = await apiService.getGameAssets(response.gameId);
    const gameAssets = ["rock","paper","scissors"];
    this.cardDeck = gameAssets;
  },

  data() {
    return {
        gameInstanceId: '',
        cardDeck: [],
    };
  }
}
</script>

<template>
  <div class="cardtable">
    <PlayerHand
        :cards=cardDeck
        :isAgainstPlayer=true
    />
    <PlayerHand
        :cards=cardDeck
        :isAgainstPlayer=false
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

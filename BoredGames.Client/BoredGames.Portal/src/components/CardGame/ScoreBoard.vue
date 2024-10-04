<script>
import DefaultConsts from '@/consts/defaultContsts';

export default {
  name: 'scoreBoard',
  props: {
    cardDeckSize: 0,
    playersScores: Array,
    currentPlayerId: ''
  },

  computed: {
    cssProps() {
      if (this.cardDeckSize == 0) {
        return {
          '--deck-size': (DefaultConsts.CardDeckSize + 1)
        }
      }
      return {
        '--deck-size': (this.cardDeckSize + 1)
      }
    },
    foeName: function() {
        return this.playersScores.filter(x => x.playerId != this.currentPlayerId)[0].playerNickName;
    },
    myName: function() {
        return this.playersScores.filter(x => x.playerId == this.currentPlayerId)[0].playerNickName;
    },
    foeWins: function() {
        let wins = this.playersScores.filter(x => x.playerId != this.currentPlayerId)[0].roundWins;
        if (wins.some(x => x)) {
            return wins.length;
        }
        return 0;
    },
    myWins: function() {
        let wins = this.playersScores.filter(x => x.playerId == this.currentPlayerId)[0].roundWins;
        if (wins.some(x => x)) {
            return wins.length;
        }
        return 0;
    },
  }
}
</script>

<template>
  <div class="scoreboard" :style="cssProps">
    <div v-if="this.playersScores.some(x => x)">
      <h1><b>{{ this.foeName }} {{ this.foeWins }} : {{ this.myWins }} {{ this.myName }}</b></h1>
    </div>
    <div v-else>
      <h1><b>0 : 0</b></h1>
    </div>
  </div>
</template>

<style>
    .scoreboard {
      border: solid;
      border-color: gold;
      grid-column-start: 1;
      grid-column-end: var(--deck-size);
      text-align: center;
      align-content: center;
    }
</style>

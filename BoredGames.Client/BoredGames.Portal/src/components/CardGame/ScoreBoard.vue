<script>

export default {
  name: 'scoreBoard',
  props: {
    playersScores: [],
    currentPlayerId: ''
  },

  computed: {
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
  <div v-if="this.playersScores.some(x => x)" class="scoreboard">
    <label>{{ this.foeName }} {{ this.foeWins }} : {{ this.myWins }} {{ this.myName }}</label>
  </div>
</template>

<style>
    .scoreboard {
      text-align: center;
      align-items: center;
    }
</style>

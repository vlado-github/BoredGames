<script>
import PlayerDialog from './PlayerDialog.vue'
import GameDialog from './GameDialog.vue'
import LocalStorageKeys from '@/consts/localStorageKeys';

export default {
  name: "gameTitleTile",
  props: {
    titleId: Number,
    titleName: String,
    iconUrl: String,
    formSchema: Array
  },

  components: {
    PlayerDialog,
    GameDialog
  },

  data() {
    return {
      playerName: ''
    }
  },

  methods: {
    async pickGame() {
      this.playerName = localStorage.getItem(LocalStorageKeys.PlayerNickName);
      if (!this.playerName) {
        await this.$refs.playerDialog.show();

      }
      else {
        await this.$refs.gameDialog.show();
      }
    }
  }
}
</script>

<template>
  <div class="title">
    <div>
        <img @click="pickGame" v-bind:id="titleId" width="250" height="250" v-bind:src="iconUrl" v-bind:alt="titleName"/>
        <span class="caption">{{ titleName }}</span>
    </div>
    <PlayerDialog ref="playerDialog" 
      :titleId="this.titleId" />
    <GameDialog ref="gameDialog" 
      :titleId="this.titleId" 
      :formSchema="this.formSchema"
      :playerName="this.playerName" />
  </div>  
</template>

<style scoped>
  .title {
    display: table-cell;
    cursor: pointer;
    text-align: center;
    vertical-align: top;
    display: inline-block;
    text-align: center;
  }
  .caption {
    display: block;
    color: whitesmoke;
}
</style>

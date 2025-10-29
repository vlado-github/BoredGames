<script>
import GamePlayer from './GamePlayer.vue';
import router from '@/router';

export default {
  name: "gameTitleTile",
  props: {
    titleId: Number,
    titleName: String,
    iconUrl: String,
  },

  components: {
    GamePlayer
  },

  methods: {
    play() {
      router.push({ name: 'game', query: { gameTitle: this.titleName} })
    }
  }
}
</script>

<template>
  <div class="title">
    <div>
        <img @click="play" :id="titleId" width="250" height="250" :src="iconUrl" :alt="titleName"/>
        <span class="caption">{{ titleName }}</span>
    </div>
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

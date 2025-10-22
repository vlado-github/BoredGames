<script>
export default {
  name: 'gamePlayer',
  props: {
    gameTitle: '',
    gameInstanceId: '' | undefined
  },

  data() {
    return {   
      linkToGameUrl: '',
    }
  },

  async mounted() {
    if (this.gameInstanceId) {
      this.linkToGameUrl = `${import.meta.env.VITE_BASE_URL}/games/${this.gameTitle}/index.html?gameInstanceId=${this.gameInstanceId}`;
    } else {
      this.linkToGameUrl = `${import.meta.env.VITE_BASE_URL}/games/${this.gameTitle}/index.html`;
    }
  }
}
</script>

<template>
  <iframe id="gameplayer" allow="clipboard-read; clipboard-write self http://localhost:5173/game?gameTitle=ClashOfHands" class="gameplayer-container" :src="linkToGameUrl" /> 
</template>

<style>
  .gameplayer-container {
    display: flex;
    min-height: 100vh;
    min-width: 100vw;
    margin:auto;
    border-style:none;
  }
</style>
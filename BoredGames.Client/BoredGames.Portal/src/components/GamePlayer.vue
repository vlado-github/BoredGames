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
  <iframe 
    sandbox="allow-scripts allow-same-origin"
    allow="clipboard-write" 
    class="gameplayer-container" 
    :src="linkToGameUrl" /> 
</template>

<style>
  .gameplayer-container {
    display: flex;
    min-height: 80vh;
    min-width: 100vw;
    margin:auto;
    border-style:none;
  }
</style>
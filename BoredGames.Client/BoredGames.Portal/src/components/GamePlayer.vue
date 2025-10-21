<script>
export default {
  name: 'gamePlayer',
  props: {
    gameTitle: '',
    gameInstanceId: ''
  },

  data() {
    return {   
      linkToGameUrl: '',
    }
  },

  async mounted() {
    this.linkToGameUrl = `${import.meta.env.VITE_BASE_URL}/games/${this.gameTitle}/index.html`;
  }
}
</script>

<template>
  <iframe class="gameplayer-container" :src="linkToGameUrl" /> 
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
<script setup>
  import router from '@/router';
  import apiService from '@/api/api';

  const props = defineProps({
    id: Number,
    title: String,
    iconUrl: String
  })

  async function startGame() {
    const response = await apiService.createGame({
      gameTitle: props.id,
      numberOfPlayers: 2,
      requiredNumberOfWins: 1,
      numberOfRounds: 1,
      description: "test",
      playerNickName: "vlado"
    });

    localStorage.setItem('gameId', response.gameId);
    
    router.push({ name: 'play', params: { gameInstanceId: response.gameId} })
  }
</script>

<template>
  <div>
    <img @click="startGame" v-bind:src="iconUrl" v-bind:id="id" width="50" height="50"/> 
    <span>{{ title }}</span>
  </div>
</template>

<style scoped>
</style>
